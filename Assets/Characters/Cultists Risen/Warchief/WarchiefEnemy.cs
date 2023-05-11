using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WarchiefEnemy : ParentEnemy
{

    // Start is called before the first frame update
    void Start()
    {
        /*
        if (character.isLeader)
        {
            character.buffsAndDebuffs.Stealth.add(1);
            if (!character.buffsAndDebuffs.BuffList.Contains(character.buffsAndDebuffs.Stealth))
                character.buffsAndDebuffs.SendMessage("stealthActivate");
            character.abilityUpdate();
        }    
        */
        //particle effect shits
        GlobalVariables.OnGainedBuff += MethodOnGainBuff;
        GlobalVariables.OnGainedDebuff += MethodOnGainDebuff;
        GlobalVariables.OnEndTurn += MethodOnEndTurn;

        character.nameOfChar = "Warchief";
        character.tags.Add("Risen");
        character.tags.Add("Support");
        character.tags.Add("Attacker");
        character.tags.Add("Nightmare");
        character.tags.Add("Magic");
        character.tags.Add("Fire");
        character.mainElement = "Nightmare";
        if (character.isLeader)
        {
            gameObject.tag = "EnemySelected";
        }
        //character.turnMeter = (preloaded turn meter int);
        if (character.level < 50)
            character.protection = 0;
        GetComponent<Animator>().CrossFade("Base Layer." + character.currentIdle, .25f);
        dmg = character.DamageAmount;
        character.Armor = true;
        character.magArmor = true; //if you have true armor, just enable both
        character.physArmorAmount = 1;
        character.magArmorAmount = .7f;
        character.health = (int)(character.health * ((character.level - 1) * 8 / 99f + 1) * ((Mathf.Pow((float)character.star - 1, 2) / 4f) + 1));
        character.maxHealth = character.health;
        character.protection = (int)(character.protection * ((character.level - 1) * 8 / 99f + 1) * ((Mathf.Pow((float)character.star - 1, 2) / 4f) + 1));
        character.maxProtection = character.protection;
        character.offense = (int)(character.offense * ((character.level - 1) * 4 / 99f + 1) * ((Mathf.Pow((float)character.star - 1, 2) / 4f) + 1));
        dmg = (int)(dmg * ((character.level - 1) * 14 / 99f + 1) * ((Mathf.Pow((float)character.star - 1, 2) / 4f) + 1));
        character.defense = (int)(character.defense * ((character.level - 1) * 3 / 99f + 1) * ((Mathf.Pow((float)character.star - 1, 2) / 6f) + 1));
        character.speed = (int)(character.speed * ((character.level - 1) * 1 / 199f + 1));
        character.Potency += (int)(character.level * .4f);
        character.Tenacity += (int)(character.level * .5f);
        if (!character.Armor || character.level <= 50)
        {
            character.armor = 0;
        }
        StartCoroutine(start());
        StartCoroutine(establishCooldowns());
    }
    IEnumerator start()
    {
        yield return new WaitForSeconds(.01f);
        character.blockSound = GlobalSounds.Anvil1;
        character.parrySound = GlobalSounds.Dink1;
    }
    
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Basic Ability / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void Basic(GameObject enemy2)
    {
        enemy = enemy2;
        gameObject.tag = "Enemy";
        enemy.tag = "PlayerSelected";
        if (!character.attackingOutOfTurn)
        {
            character.turnMeter = 0;
            GlobalVariables.addToWhomstJustUsedBasic(character);
        }
        //look at enemy
        StartCoroutine(MoveToEnemy(1.05f, "Basic", 7));
        StartCoroutine(LookAtEnemy());
    }
    IEnumerator basic()
    {
        gameObject.tag = "Enemy";
        yield return new WaitForSeconds(.6f);

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
            damageNumbers("White", "player");
            GlobalVariables.addToWhomstJustParried(enemy.GetComponent<Character>()); //tells globalvariables that the enemy just dodged
        }
        //if enemy blocks
        else if (Random.Range(0, 100) < enemy.GetComponent<Character>().block)
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Blocked!";
            enemy.SendMessage("Blocking");
            damageNumbers("White", "player");
            GlobalVariables.addToWhomstJustBlocked(enemy.GetComponent<Character>()); //tells globalvariables that the enemy just dodged
        }
        else
        {
            // if critical hit or not
            if (!enemy.GetComponent<Character>().critImmunity && ((Random.Range(0, 100) < (character.CriticalChance - enemy.GetComponent<Character>().CriticalAvoidance)) || character.Focus))
            {
                character.DamageAmount = (int)((dmg + Random.Range(0, 26)) * 2.5f * character.CritMultiplier);

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
                character.DamageAmount = (int)((dmg + Random.Range(0, 26)) * 2.5f);

            }
            //elemental Damage
            character.elementDamage(character.mainElement, enemy.GetComponent<Character>().tags);
            bool moreOffense = true;
            for (int i = 0; i < GlobalVariables.hostileArray.Length; i++)
            {
                if (!GlobalVariables.hostileArray[i].GetComponent<Character>().tags.Contains("Risen") && !GlobalVariables.hostileArray[i].GetComponent<Character>().tags.Contains("Cultist"))
                    moreOffense = false;
            }
            if (moreOffense)
                character.offense = (int)(character.offense * 1.2f);
            enemy.GetComponent<Character>().SendMessage("WarchiefBasic", character);
            if (moreOffense)
                character.offense = (int)(character.offense / 1.2f);
            GlobalVariables.DamageNumberRed.GetComponent<TextMeshProUGUI>().text = "" + enemy.GetComponent<Character>().damage;
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
        if (!character.attackingOutOfTurn)
            cooldownChange(-1);
        enemy.GetComponent<Character>().SendMessage("abilityUpdate");
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / /  Special Abilities / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void Special1(GameObject enemy2)
    {
        enemy = enemy2;
        gameObject.tag = "Enemy";
        character.turnMeter = 0;
        GlobalVariables.addToWhomstJustUsedSpecial(character);
        StartCoroutine(special1());
    }
    IEnumerator special1()
    {
        gameObject.tag = "Enemy";
        GetComponent<Animator>().CrossFade("Special1", .025f);
        yield return new WaitForSeconds(.37f);
        character.buffsAndDebuffs.cleanseBuffs();
        character.buffsAndDebuffs.cleanseDebuffs();
        for (int i = 0; i < GlobalVariables.hostileArray.Length; i++)
        {
            if ((GlobalVariables.hostileArray[i].GetComponent<Character>().tags.Contains("Risen") || GlobalVariables.hostileArray[i].GetComponent<Character>().tags.Contains("Cultist")) && Random.Range(0, 100) < 60)
            {
                GlobalVariables.hostileArray[i].GetComponent<Character>().buffsAndDebuffs.Stealth.add(3);
                if (!GlobalVariables.hostileArray[i].GetComponent<Character>().buffsAndDebuffs.BuffList.Contains(GlobalVariables.hostileArray[i].GetComponent<Character>().buffsAndDebuffs.Stealth))
                    GlobalVariables.hostileArray[i].GetComponent<Character>().buffsAndDebuffs.SendMessage("stealthActivate");
                GlobalVariables.hostileArray[i].GetComponent<Character>().abilityUpdate();
            }
        }
        character.buffsAndDebuffs.Insanity.add(1);
        if (!character.buffsAndDebuffs.BlueBuffList.Contains(character.buffsAndDebuffs.Insanity))
            character.buffsAndDebuffs.SendMessage("insanityActivate");
        yield return new WaitForSeconds(.4f);
        if (!character.attackingOutOfTurn) {GetComponent<Animator>().CrossFade("Base Layer." + character.currentIdle, .5f);} else {GetComponent<Animator>().Play("Base Layer." + character.currentIdle);}
        if (!character.attackingOutOfTurn)
            cooldownChange(-1);
        enemy.GetComponent<Character>().SendMessage("abilityUpdate");
        GlobalVariables.TakingTurn = false;
        character.buffsAndDebuffs.SendMessage("EndOfTurn");
        character.turnUpdate = true;
    }
    
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Move to Enemy / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    IEnumerator MoveToEnemy(float wait, string ability, int distance)
    {
        int speed = 0;
        Vector3 enemyPos = new Vector3(enemy.GetComponent<Character>().initial.x, enemy.GetComponent<Character>().initial.y, enemy.GetComponent<Character>().initial.z);
        Vector3 lTargetDir = enemyPos - transform.position;
        lTargetDir.y = 0.0f;
        GetComponent<Animator>().CrossFade("Base Layer.Sprint", .01f);
        while (Vector3.Distance(enemyPos, transform.position) > 2 + distance)
        {
            transform.position = Vector3.MoveTowards(transform.position, enemyPos, speed * Time.deltaTime);
            if (speed < 420)
                speed += 40;
            yield return new WaitForSeconds(.03f);
        }
        switch (ability)
        {
            case "Basic":
                GetComponent<Animator>().CrossFade("Base Layer.Basic", .25f);
                StartCoroutine(basic());
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
            if (speed < 490)
                speed += 70;
            yield return new WaitForSeconds(.03f);
        }
        lTargetDir = new Vector3(0, 0, -180);
        for (int i = 0; i < 20; i++)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lTargetDir), Time.time * 1f);
        }
        if (character.health < character.maxHealth / 2)
            character.currentIdle = "hurtIdle";
        else
            character.currentIdle = "BattleIdle";
        if (!character.attackingOutOfTurn) {GetComponent<Animator>().CrossFade("Base Layer." + character.currentIdle, .5f);} else {GetComponent<Animator>().Play("Base Layer." + character.currentIdle);}
        GlobalVariables.TakingTurn = false;
        if (!character.attackingOutOfTurn)
            character.buffsAndDebuffs.SendMessage("EndOfTurn");
        character.turnUpdate = true;
    }
    
    private void MethodOnGainDebuff()
    {
        if (GlobalVariables.whomstJustGainedDebuff[0] == character)
        {
            character.defense = (int)(character.defense * 1.05f);
            if (character.buffsAndDebuffs.DebuffList.Contains(character.buffsAndDebuffs.Insanity))
            {
                character.defense = (int)(character.defense * 1.05f);
                character.maxHealth = (int)(character.maxHealth * 1.05f);
                character.health = (int)(character.health * 1.05f);
            }
        }
        if (!GlobalVariables.whomstJustGainedDebuff[0].name.Contains("Enemy") && GlobalVariables.debuffGained[0].Equals("corruption"))
        {
            character.counter += 2;
        }
    }
    private void MethodOnGainBuff()
    {
        if (GlobalVariables.whomstJustGainedBuff[0] == character)
        {
            character.Tenacity = (int)(character.Tenacity * 1.05f);
            if (character.buffsAndDebuffs.DebuffList.Contains(character.buffsAndDebuffs.Insanity))
            {
                character.buffsAndDebuffs.SendMessage("insanityDeactivate");
                character.buffsAndDebuffs.BlueBuffList.Remove(character.buffsAndDebuffs.Insanity);
            }
        }
        bool armuk = false;
        for (int i = 0; i < GlobalVariables.hostileArray.Length; i++)
        {
            if (GlobalVariables.hostileArray[i].GetComponent<Character>().nameOfChar.Equals("Armuk") && !GlobalVariables.hostileArray[i].GetComponent<Character>().dead)
            {
                armuk = true;
            }
        }
        if (armuk && !GlobalVariables.whomstJustGainedDebuff[0].name.Contains("Enemy") && GlobalVariables.buffGained[0].Equals("stealth"))
        {
            character.buffsAndDebuffs.EvasionUp.add(2);
            if (!character.buffsAndDebuffs.BuffList.Contains(character.buffsAndDebuffs.EvasionUp))
                character.buffsAndDebuffs.SendMessage("evasionUpActivate");
        }
    }
    private void MethodOnEndTurn()
    {
        if (GlobalVariables.whomstJustEndedTurn[0] == character && character.buffsAndDebuffs.DebuffList.Count > 0 && !character.buffImmunity)
        {
            character.buffsAndDebuffs.HealOverTime.add(2);
            if (!character.buffsAndDebuffs.BuffList.Contains(character.buffsAndDebuffs.HealOverTime))
                character.buffsAndDebuffs.SendMessage("healActivate");
        }
    }

    // / / / / / / / / / / / / / / / / / / / / / / / / / / Ability AI / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public override void AbilityAI(List<GameObject> enemies)
    {
        int count = 0;
        GameObject currentEnemy = enemies[0];
        while (count < enemies.Count)
        {
            if ((enemies[count].GetComponent<Character>().health + enemies[count].GetComponent<Character>().protection) > (currentEnemy.GetComponent<Character>().health + currentEnemy.GetComponent<Character>().protection))
                currentEnemy = enemies[count];
            count++;
        }
        GlobalVariables.playersSelected = null;
        currentEnemy.tag = "PlayerSelected";
        GlobalVariables.playersSelected = GameObject.FindGameObjectsWithTag("PlayerSelected"); GlobalVariables.playersSelected[0] = currentEnemy;
        if (character.buffsAndDebuffs.BlueBuffList.Contains(character.buffsAndDebuffs.Trial) && !character.abilityBlock)
        {
            character.Plead();
            if (!character.attackingOutOfTurn)
                cooldownChange(-1);
            gameObject.tag = "EnemySelected";

        }
        else if (character.abilities.CurrentAbilityCooldowns[0] == 0 && !character.abilityBlock)
        {
            Special1(currentEnemy);
            character.abilities.CurrentAbilityCooldowns[0] = character.abilities.AbilityCooldowns[0];
        }
        else
        {
            Basic(currentEnemy);
        }
    }
}
