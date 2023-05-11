using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shaubeny : ParentCharacter
{
    
    // Start is called before the first frame update
    void Start()
    {
        GlobalVariables.OnKilled += OnCrusaderKilled;
        GlobalVariables.OnGotSelectedAlly += basic;
        character.turnMeterDownImmunity = true;
        character.stunImmunity = true;
        character.abilityBlockImmunity = true;
        character.nameOfChar = "Shaubeny";
        character.Gender = "male";
        //tags
        character.tags.Add("Leader");
        character.tags.Add("Attacker");
        character.tags.Add("Crusader");
        character.tags.Add("Metal");
        character.mainElement = "Metal";


        //setting the stats of the character
        GetComponent<Animator>().CrossFade("Base Layer." + character.currentIdle, .25f);
        if (character.level < 50)
            character.protection = 0;
        dmg = character.DamageAmount;
        character.Armor = true;
        character.physArmor = true; //if you have true armor, just enable both
        character.physArmorAmount = .7f;
        character.magArmorAmount = 1;
        character.health = (int)(character.health * ((character.level - 1) * 8 / 84f + 1) * ((Mathf.Pow((float)character.star - 1, 2) / 4f) + 1));
        character.maxHealth = character.health;
        character.protection = (int)(character.protection * ((character.level - 1) * 8 / 74f + 1) * ((Mathf.Pow((float)character.star - 1, 2) / 4f) + 1));
        character.maxProtection = character.protection;
        character.offense = (int)(character.offense * ((character.level - 1) * 4 / 129f + 1) * ((Mathf.Pow((float)character.star - 1, 2) / 4f) + 1));
        dmg = (int)(dmg * ((character.level - 1) * 14 / 99f + 1) * ((Mathf.Pow((float)character.star - 1, 2) / 4f) + 1));
        character.defense = (int)(character.defense * ((character.level - 1) * 3 / 79f + 1) * ((Mathf.Pow((float)character.star - 1, 2) / 6f) + 1));
        character.speed = (int)(character.speed * ((character.level - 1) * 1 / 199f + 1));
        character.Potency += (int)(character.level * .45f);
        character.Tenacity += (int)(character.level * .6f);
        if (!character.Armor || character.level <= 50)
        {
            character.armor = 0;
        }
        StartCoroutine(start());
        StartCoroutine(establishCooldowns());
    }
    IEnumerator start()
    {
        yield return new WaitForSeconds(.1f);
        //character.blockSound = GlobalSounds.Anvil1;
        //character.parrySound = GlobalSounds.Dink1;
        if (character.isLeader)
        {
            GlobalVariables.Leaders[0] = "Shaubeny";
            foreach (GameObject boy in GlobalVariables.friendlyArray)
            {
                if (boy.GetComponent<Character>().tags.Contains("Crusader"))
                {
                    boy.GetComponent<Character>().defense = (int)(boy.GetComponent<Character>().defense * (1.25f));
                    boy.GetComponent<Character>().offense = (int)(boy.GetComponent<Character>().offense * (1.25f));
                    boy.GetComponent<Character>().counter += 60;
                }
            }
        }
    }
    
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Basic Ability / / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void Basic(GameObject hoeWhoCountered)
    {
        if (hoeWhoCountered == null)
        {
            GlobalVariables.allySelectable = true;
            StartCoroutine(basicWaiting());
        }
        else
        {
            enemy = hoeWhoCountered;
            GlobalVariables.enemiesSelected[0] = hoeWhoCountered;
            StartCoroutine(MoveToEnemy(.95f, "Basic", 7));
            StartCoroutine(LookAtEnemy());
        }
    }
    IEnumerator basicWaiting()
    {
        while (GlobalVariables.allySelectable && gameObject.CompareTag("AllyTurn"))
        {
            yield return new WaitForSeconds(.0000000f);
        }
        if (GlobalVariables.allySelectable == false && gameObject.CompareTag("AllyTurn"))
        {
            gameObject.tag = "Ally";
            if (!character.attackingOutOfTurn)
            {
                character.turnMeter = 0;
                GlobalVariables.addToWhomstJustUsedBasic(character);
                basicButton.GetComponent<BasicButton>().moveDownAndBack(character.boutons.Count, Buttons);
            }
            if (!character.attackingOutOfTurn)
            {
                for (int i = 0; i < character.boutons.Count; i++)
                {
                    character.boutons[i].currentCooldown -= 1;
                }
            }
            GlobalVariables.addToWhomstJustSelectedAlly(character);
        }
        GlobalVariables.allySelectable = false;
    }
    private void basic()
    {
        if (!GlobalVariables.whomstJustGotSelectedAlly[0].name.Contains("Enemy") && gameObject.CompareTag("AllyTurn"))
        {
            enemy = GlobalVariables.enemiesSelected[0];
            StartCoroutine(MoveToEnemy(.95f, "Basic", 7));
            StartCoroutine(LookAtEnemy());
        }
    }
    IEnumerator basic2(Character selected)
    {
        if (selected != null && selected != character)
        {
            if (selected.canAttackOutOfTurn)
                selected.SendMessage("AttackOutOfTurn", 1f);
        }
        else if (selected != character)
        {
            bool called = false;
            for (int i = 0; i < GlobalVariables.friendlyArray.Length; i++)
            {
                if (!called && GlobalVariables.friendlyArray[i].GetComponent<Character>().canAttackOutOfTurn && GlobalVariables.friendlyArray[i] != this)
                {
                    GlobalVariables.friendlyArray[i].SendMessage("AttackOutOfTurn", 1f);
                    called = true;
                }
            }
        }
        gameObject.tag = "Ally";
        yield return new WaitForSeconds(.1f);
        yield return new WaitForSeconds(.4f);

        // if enemy dodges
        if (((Random.Range(0, 100) < enemy.GetComponent<Character>().dodge - character.accuracy) || enemy.GetComponent<Character>().Agility || character.Blind || enemy.GetComponent<Character>().TrueSight) && !enemy.GetComponent<Character>().tags.Contains("Behemoth"))
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Evaded!";
            enemy.SendMessage("Evasion");
            damageNumbers("White", "enemy");
            GlobalVariables.addToWhomstJustDodged(enemy.GetComponent<Character>()); //tells globalvariables that the enemy just dodged
            if (enemy.GetComponent<Character>().Agility)
            {
                enemy.GetComponent<Character>().buffsAndDebuffs.Agility.setAmount(0);
                enemy.GetComponent<Character>().buffsAndDebuffs.SendMessage("agilityDeactivate");
            }
            else if (character.Blind)
            {
                character.buffsAndDebuffs.Blind.setAmount(0);
                character.buffsAndDebuffs.SendMessage("blindDeactivate");
            }
            else if (enemy.GetComponent<Character>().TrueSight)
            {
                enemy.GetComponent<Character>().buffsAndDebuffs.TrueSight.setAmount(0);
                enemy.GetComponent<Character>().buffsAndDebuffs.SendMessage("trueSightDeactivate");
            }
        }
        //if enemy parries
        else if (Random.Range(0, 100) < enemy.GetComponent<Character>().parry)
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Parried!";
            character.buffsAndDebuffs.Stagger.setAmount(2);
            if (!character.buffsAndDebuffs.DebuffList.Contains(character.buffsAndDebuffs.Stagger))
            {
                character.buffsAndDebuffs.SendMessage("staggerActivate");
            }
            enemy.SendMessage("Parrying", this.gameObject);
            character.buffsAndDebuffs.ImageArray();
            damageNumbers("White", "enemy");
            GlobalVariables.addToWhomstJustParried(enemy.GetComponent<Character>());
        }
        //if enemy blocks
        else if (Random.Range(0, 100) < enemy.GetComponent<Character>().block)
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Blocked!";
            enemy.SendMessage("Blocking");
            damageNumbers("White", "enemy");
            GlobalVariables.addToWhomstJustBlocked(enemy.GetComponent<Character>());
        }
        else
        {
            // if critical hit or not
            if (!enemy.GetComponent<Character>().critImmunity && ((Random.Range(0, 100) < (character.CriticalChance - enemy.GetComponent<Character>().CriticalAvoidance)) || character.Focus))
            {
                character.DamageAmount = (int)((dmg + Random.Range(0, 26)) * 1.75f * character.CritMultiplier);

                GlobalVariables.addToWhomstJustGotCrit(enemy.GetComponent<Character>());
                GlobalVariables.addToWhomstJustCrit(this.GetComponent<Character>());
                character.critical = true;
                if (character.Focus)
                {
                    character.buffsAndDebuffs.Focus.setAmount(0);
                    character.buffsAndDebuffs.SendMessage("focusDeactivate");
                }
            }
            else
            {
                character.DamageAmount = (int)((dmg + Random.Range(0, 26)) * 1.75f);

            }
            //elemental Damage
            character.elementDamage(character.mainElement, enemy.GetComponent<Character>().tags);
            enemy.GetComponent<Character>().SendMessage("ShaubenyBasic", character);
            // countering
            if (Random.Range(0, 100) < enemy.GetComponent<Character>().counter && !character.attackingOutOfTurn && !character.buffsAndDebuffs.BuffList.Contains(character.buffsAndDebuffs.Stealth))
            {
                StartCoroutine(Counter(1f));
            }
            // health steal
            if (character.health < character.maxHealth && !character.healingImmunity)
            {
                GlobalVariables.DamageNumberGreen.GetComponent<TextMeshProUGUI>().text = "" + (int)(enemy.GetComponent<Character>().damage * (character.HealthSteal / 100));
                character.health += (int)(enemy.GetComponent<Character>().damage * (character.HealthSteal / 100));
                damageNumbers("Green", "me");
                if (character.health < character.maxHealth / 4)
                    character.healthArray[0].color = Color.red;
                else if (character.health < character.maxHealth / 2)
                    character.healthArray[0].color = Color.yellow;
                else
                    character.healthArray[0].color = Color.green;
            }
            yield return new WaitForSeconds(.01f);
        }
        yield return new WaitForSeconds(.1f);
        enemy.GetComponent<Character>().SendMessage("abilityUpdate");
    }
   // / / / / / / / / / / / / / / / / / / / / / / / You Assisting / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public override IEnumerator AttackOutOfTurn(float DamageMultiplier)
    {
        character.actionQueue++;
        if (!character.attackingOutOfTurn)
        {
            yield return new WaitForSeconds(.5f);
            if (character.canAttackOutOfTurn && !character.dead)
            {
                 GlobalVariables.numCounters++; dmg = (int)(dmg * DamageMultiplier); if (GlobalVariables.Leaders[0].Equals("Shaubeny")) character.HealthSteal += 70;
                character.attackingOutOfTurn = true; enemy = GlobalVariables.enemiesSelected[0];
                
                float counter = character.counter; character.counter = 0;
                while (character.actionQueue > 0 && enemy.GetComponent<Character>().health > 0 && enemy.GetComponent<Character>().health > 0)
                {
                    GlobalVariables.assisters++;
                    GlobalVariables.addToWhomstJustAttackedOutOfTurn(character);
                    StartCoroutine(MoveToEnemy(.95f, "Basic", 7));
                    StartCoroutine(LookAtEnemy());
                    yield return new WaitForSeconds(1.5f);
                    GlobalVariables.assisters--;
                    yield return new WaitForSeconds(.6f);
                    character.actionQueue--;
                }
                character.counter = counter;
                 GlobalVariables.numCounters--; dmg = (int)(dmg / DamageMultiplier); if (GlobalVariables.Leaders[0].Equals("Shaubeny")) character.HealthSteal -= 70;
                character.attackingOutOfTurn = false;
            }
            else character.actionQueue--;

        }
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Special Ability 1 / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void Special2(GameObject enemy2)
    {
        enemy = enemy2;
        gameObject.tag = "Ally";
        basicButton.GetComponent<BasicButton>().moveDownAndBack(character.boutons.Count, Buttons);
        character.turnMeter = 0;
        GlobalVariables.addToWhomstJustUsedSpecial(character);
        //character.IntroCutscene(enemy.GetComponent<Character>());
        if (enemy.GetComponent<Character>().buffsAndDebuffs.BlueBuffList.Contains(enemy.GetComponent<Character>().buffsAndDebuffs.Trial))
            Debug.Log("Heehoo");
        StartCoroutine(MoveToEnemy(1.25f, "Special2", 7));
        StartCoroutine(LookAtEnemy());
    }
    IEnumerator special2()
    {
        gameObject.tag = "Ally";
        yield return new WaitForSeconds(.6f);

       // if critical hit or not
            if (!enemy.GetComponent<Character>().critImmunity && ((Random.Range(0, 100) < (character.CriticalChance - enemy.GetComponent<Character>().CriticalAvoidance)) || character.Focus))
            {
                character.DamageAmount = (int)((dmg + Random.Range(0, 26)) * 1.25f * character.CritMultiplier);

                GlobalVariables.addToWhomstJustGotCrit(enemy.GetComponent<Character>());
                GlobalVariables.addToWhomstJustCrit(this.GetComponent<Character>());
                character.critical = true;
                if (character.Focus)
                {
                    character.buffsAndDebuffs.Focus.setAmount(0);
                    character.buffsAndDebuffs.SendMessage("focusDeactivate");
                }
            }
            else
            {
                character.DamageAmount = (int)((dmg + Random.Range(0, 26)) * 1.25f);

            }
        //elemental Damage
        character.elementDamage(character.mainElement, enemy.GetComponent<Character>().tags);
        enemy.GetComponent<Character>().SendMessage("ShaubenySpecial2", character);
            // countering
            if (Random.Range(0, 100) < enemy.GetComponent<Character>().counter && !character.attackingOutOfTurn && character.buffsAndDebuffs.BuffList.Contains(character.buffsAndDebuffs.Stealth))
            {
                StartCoroutine(Counter(1f));
            }
            // health steal
            if (character.health < character.maxHealth && !character.healingImmunity)
            {
                GlobalVariables.DamageNumberGreen.GetComponent<TextMeshProUGUI>().text = "" + (int)(enemy.GetComponent<Character>().damage * (character.HealthSteal / 100));
                character.health += (int)(enemy.GetComponent<Character>().damage * (character.HealthSteal / 100));
                damageNumbers("Green", "me");
                if (character.health < character.maxHealth / 4)
                    character.healthArray[0].color = Color.red;
                else if (character.health < character.maxHealth / 2)
                    character.healthArray[0].color = Color.yellow;
                else
                    character.healthArray[0].color = Color.green;
            }
            yield return new WaitForSeconds(.01f);
        
        enemy.GetComponent<Character>().SendMessage("abilityUpdate");
    }

    // / / / / / / / / / / / / / / / / / / / / / / / / / / Special Ability 2 / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void Special1(GameObject enemy2)
    {
        gameObject.tag = "Ally";
        basicButton.GetComponent<BasicButton>().moveDownAndBack(character.boutons.Count, Buttons);
        character.turnMeter = 0;
        GlobalVariables.addToWhomstJustUsedSpecial(character);
        StartCoroutine(special1(enemy2));
    }
    IEnumerator special1(GameObject enemy2)
    {
        enemy = enemy2;
        gameObject.tag = "Ally";
        GetComponent<Animator>().CrossFade("Special1", .025f);
        yield return new WaitForSeconds(.57f);
        //character.audioSource3.clip = GlobalSounds.MaleGrunt2;
        //character.audioSource3.Play();
        for (int i = 0; i < GlobalVariables.friendlyArray.Length; i++)
        {
            if (!GlobalVariables.friendlyArray[i].GetComponent<Character>().turnMeterDownImmunity)
                GlobalVariables.friendlyArray[i].GetComponent<Character>().turnMeter = 0;
        }
        for (int i = 0; i < GlobalVariables.hostileArray.Length; i++)
        {
            if (!GlobalVariables.hostileArray[i].GetComponent<Character>().turnMeterDownImmunity)
                GlobalVariables.hostileArray[i].GetComponent<Character>().turnMeter = 0;
        }
        character.turnMeter = 98;
        enemy.GetComponent<Character>().turnMeter = 105;
        enemy.GetComponent<Character>().SendMessage("ShaubenySpecial1", character);
        enemy.GetComponent<Character>().SendMessage("abilityUpdate");
        yield return new WaitForSeconds(.8f);
        GlobalVariables.TakingTurn = false;
        if (!character.attackingOutOfTurn)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                Destroy(buttons[i]);
            }
            buttons.Clear();

            basicButton.GetComponent<BasicButton>().moveUp(Buttons);
        }
        character.buffsAndDebuffs.SendMessage("EndOfTurn");
        character.turnUpdate = true;
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Move To Enemy / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    IEnumerator MoveToEnemy(float wait, string ability, int distance)
    {
        int speed = 0;
        Character selected;
        Vector3 enemyPos = new Vector3(enemy.GetComponent<Character>().initial.x, enemy.GetComponent<Character>().initial.y, enemy.GetComponent<Character>().initial.z);
        Vector3 lTargetDir = enemyPos - transform.position;
        lTargetDir.y = 0.0f;
        if (GlobalVariables.whomstJustGotSelectedAlly.Count > 0)
            selected = GlobalVariables.whomstJustGotSelectedAlly[0].GetComponent<Character>();
        else
            selected = null;
        GetComponent<Animator>().CrossFade("Base Layer.Sprint", .01f);
        while (Vector3.Distance(enemyPos, transform.position) > 2 + distance)
        {
            transform.position = Vector3.MoveTowards(transform.position, enemyPos, speed * Time.deltaTime);
            if (speed < 360)
                speed += 40;
            yield return new WaitForSeconds(.03f);
        }
        switch (ability)
        {
            case "Basic":
                GetComponent<Animator>().CrossFade("Base Layer.Basic", .3f);
                StartCoroutine(basic2(selected));
                break;
            case "Special2":
                GetComponent<Animator>().CrossFade("Base Layer.Special2", .3f);
                StartCoroutine(special2());
                break;
        }
        yield return new WaitForSeconds(wait);
        speed = 0;
        GetComponent<Animator>().CrossFade("Base Layer.Hop", .25f);
        yield return new WaitForSeconds(.4f);
        lTargetDir = transform.position - character.initial;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lTargetDir), Time.time * 1);
        while (Vector3.Distance(character.initial, transform.position) > .01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, character.initial, speed * Time.deltaTime);
            if (speed < 480)
                speed += 60;
            yield return new WaitForSeconds(.03f);
        }
        lTargetDir = new Vector3(0, 0, 180);
        for (int i = 0; i < 20; i++)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lTargetDir), Time.time * 1f);
        }
        if (character.health <= character.maxHealth / 2)
            character.currentIdle = "hurtIdle";
        else
            character.currentIdle = "BattleIdle";
        if (!character.attackingOutOfTurn) {GetComponent<Animator>().CrossFade("Base Layer." + character.currentIdle, .5f);} else {GetComponent<Animator>().Play("Base Layer." + character.currentIdle);}
        GlobalVariables.TakingTurn = false;
        if (!character.attackingOutOfTurn)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                Destroy(buttons[i]);
            }
            buttons.Clear();

            basicButton.GetComponent<BasicButton>().moveUp(Buttons);
        }
        if (!character.attackingOutOfTurn)
            character.buffsAndDebuffs.SendMessage("EndOfTurn");
        character.turnUpdate = true;
    }
    
    private void OnCrusaderKilled()
    {
        if (!GlobalVariables.whomstJustKilled[0].gameObject.name.Contains("Enemy") && GlobalVariables.whomstJustKilled[0].GetComponent<Character>().tags.Contains("Crusader"))
        {
            foreach (GameObject boy in GlobalVariables.friendlyArray)
            {
                if (boy.GetComponent<Character>().tags.Contains("Crusader"))
                {
                    boy.GetComponent<Character>().turnMeter += 25;
                    if (boy.GetComponent<Character>().turnMeter > 100)
                        boy.GetComponent<Character>().turnMeter = 100;
                    if (!boy.GetComponent<Character>().buffsAndDebuffs.DebuffList.Contains(boy.GetComponent<Character>().buffsAndDebuffs.BuffImmunity))
                    {
                        boy.GetComponent<Character>().buffsAndDebuffs.OffenseUp.add(2);
                        if (!boy.GetComponent<Character>().buffsAndDebuffs.BuffList.Contains(boy.GetComponent<Character>().buffsAndDebuffs.OffenseUp))
                            boy.GetComponent<Character>().buffsAndDebuffs.SendMessage("offenseUpActivate");
                    }
                    boy.GetComponent<Character>().abilityUpdate();
                }
            }
        }
    }

}
