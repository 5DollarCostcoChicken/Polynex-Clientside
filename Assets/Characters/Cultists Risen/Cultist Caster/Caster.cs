using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Caster : ParentCharacter
{
    
    public GameObject hand;
    public GameObject arrow;
    int BasicCount = 1;
    // Start is called before the first frame update
    void Start()
    {
        //GlobalVariables.OnGotSelectedAlly += beans;
        GlobalVariables.OnEndTurn += MethodOnEndTurn;
        character.nameOfChar = "Caster";
        character.Gender = "female";
        character.tags.Add("Cultist");
        character.tags.Add("Support");
        character.tags.Add("Nightmare");
        character.tags.Add("Magic");
        character.mainElement = "Nightmare";
        //button shit (unneeded for enemy versions)

        //setting the stats of the character
        GetComponent<Animator>().CrossFade("Base Layer." + character.currentIdle, .25f);
        if (character.level < 50)
            character.protection = 0;
        dmg = character.DamageAmount;
        character.Armor = false;
        character.physArmor = true; //if you have true armor, just enable both
        character.physArmorAmount = .7f;
        character.magArmorAmount = 1;
        character.health = (int)(character.health * ((character.level - 1) * 8 / 99f + 1) * ((Mathf.Pow((float)character.star - 1, 2) / 4f) + 1));
        character.maxHealth = character.health;
        character.protection = (int)(character.protection * ((character.level - 1) * 8 / 99f + 1) * ((Mathf.Pow((float)character.star - 1, 2) / 4f) + 1));
        character.maxProtection = character.protection;
        character.offense = (int)(character.offense * ((character.level - 1) * 4 / 79f + 1) * ((Mathf.Pow((float)character.star - 1, 2) / 4f) + 1));
        dmg = (int)(dmg * ((character.level - 1) * 14 / 99f + 1) * ((Mathf.Pow((float)character.star - 1, 2) / 4f) + 1));
        character.defense = (int)(character.defense * ((character.level - 1) * 3 / 89f + 1) * ((Mathf.Pow((float)character.star - 1, 2) / 6f) + 1));
        character.speed = (int)(character.speed * ((character.level - 1) * 1 / 199f + 1));
        character.Potency += (int)(character.level * .75f);
        character.Tenacity += (int)(character.level * .15f);
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
        //character.blockSound = GlobalSounds.Anvil1;
        //character.parrySound = GlobalSounds.Dink1;
    }
    // / / / / / / / / / / / / / / / / / / / / / / / Basic Ability / / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void Basic(GameObject enemy2)
    {
        enemy = enemy2;
        gameObject.tag = "Ally";
        BasicCount = 1;
        int count = 0;
        foreach (GameObject boy in GlobalVariables.friendlyArray)
        {
            if (boy.GetComponent<Character>().tags.Contains("Cultist") && boy.GetComponent<Character>() != character)
                count++;
        }
        if (count > 1)
            BasicCount++;
        if (enemy.GetComponent<Character>().buffsAndDebuffs.DebuffList.Contains(enemy.GetComponent<Character>().buffsAndDebuffs.OffenseDown))
            BasicCount++;
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
        StartCoroutine(basic());
    }
    IEnumerator basic()
    {
        gameObject.tag = "Ally";
        for (int i = 0; i < BasicCount; i++)
        {
            GetComponent<Animator>().Play("Basic");
            yield return new WaitForSeconds(.36f);
            StartCoroutine(projectile(enemy, "Basic"));
            character.buffsAndDebuffs.ImageArray();
            GetComponent<Animator>().CrossFade("Base Layer." + character.currentIdle, .08f);
            yield return new WaitForSeconds(.2f);
        }
        yield return new WaitForSeconds(.71f);
        GetComponent<Animator>().CrossFade("Base Layer." + character.currentIdle, .25f);
        GlobalVariables.TakingTurn = false;
        character.buffsAndDebuffs.SendMessage("EndOfTurn");
        character.turnUpdate = true;
    }
    
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Special Ability 1 / / / / / / / / / / / / / / / / / / / / / / / / / / / / /

    public void Special2()
    {
        gameObject.tag = "Ally";
        basicButton.GetComponent<BasicButton>().moveDownAndBack(character.boutons.Count, Buttons);
        character.turnMeter = 0;
        GlobalVariables.addToWhomstJustUsedSpecial(character);
        StartCoroutine(special2());
    }
    IEnumerator special2()
    {
        gameObject.tag = "Ally";
        GetComponent<Animator>().CrossFade("Special2", .025f);
        yield return new WaitForSeconds(.57f);
        for (int i = 0; i < GlobalVariables.friendlyArray.Length; i++)
        {
            if (GlobalVariables.friendlyArray[i].GetComponent<Character>().tags.Contains("Cultist") && !GlobalVariables.friendlyArray[i].GetComponent<Character>().buffImmunity)
            {
                GlobalVariables.friendlyArray[i].GetComponent<Character>().buffsAndDebuffs.HealthUp.add(3);
                if (!GlobalVariables.friendlyArray[i].GetComponent<Character>().buffsAndDebuffs.BuffList.Contains(GlobalVariables.friendlyArray[i].GetComponent<Character>().buffsAndDebuffs.HealthUp))
                    GlobalVariables.friendlyArray[i].GetComponent<Character>().buffsAndDebuffs.SendMessage("healthUpActivate");
                GlobalVariables.friendlyArray[i].GetComponent<Character>().buffsAndDebuffs.OffenseUp.add(3);
                if (!GlobalVariables.friendlyArray[i].GetComponent<Character>().buffsAndDebuffs.BuffList.Contains(GlobalVariables.friendlyArray[i].GetComponent<Character>().buffsAndDebuffs.OffenseUp))
                    GlobalVariables.friendlyArray[i].GetComponent<Character>().buffsAndDebuffs.SendMessage("offenseUpActivate");
                GlobalVariables.friendlyArray[i].GetComponent<Character>().buffsAndDebuffs.PotencyUp.add(3);
                if (!GlobalVariables.friendlyArray[i].GetComponent<Character>().buffsAndDebuffs.BuffList.Contains(GlobalVariables.friendlyArray[i].GetComponent<Character>().buffsAndDebuffs.PotencyUp))
                    GlobalVariables.friendlyArray[i].GetComponent<Character>().buffsAndDebuffs.SendMessage("potencyUpActivate");
                GlobalVariables.friendlyArray[i].GetComponent<Character>().abilityUpdate();
            }
        }
        for (int i = 0; i < GlobalVariables.hostileArray.Length; i++)
        {
            if (GlobalVariables.hostileArray[i].GetComponent<Character>().buffsAndDebuffs.DebuffList.Contains(GlobalVariables.hostileArray[i].GetComponent<Character>().buffsAndDebuffs.Corruption) && !GlobalVariables.hostileArray[i].GetComponent<Character>().turnMeterDownImmunity)
            {
                GlobalVariables.hostileArray[i].GetComponent<Character>().turnMeter -= 25;
                if (GlobalVariables.hostileArray[i].GetComponent<Character>().turnMeter < 0)
                    GlobalVariables.hostileArray[i].GetComponent<Character>().turnMeter = 0;
            }
        }
        yield return new WaitForSeconds(.8f);
        GlobalVariables.TakingTurn = false;
        for (int i = 0; i < buttons.Count; i++)
        {
            Destroy(buttons[i]);
        }
        buttons.Clear();
        if (!character.attackingOutOfTurn)
            basicButton.GetComponent<BasicButton>().moveUp(Buttons);
        character.buffsAndDebuffs.SendMessage("EndOfTurn");
        character.turnUpdate = true;
    }
    public void Special1()
    {
        enemy = GlobalVariables.enemiesSelected[0];
        gameObject.tag = "Ally";
        basicButton.GetComponent<BasicButton>().moveDownAndBack(character.boutons.Count, Buttons);
        character.turnMeter = 0;
        GlobalVariables.addToWhomstJustUsedSpecial(character);
        StartCoroutine(special1(enemy));
    }
    IEnumerator special1(GameObject enemy3)
    {
        gameObject.tag = "Ally";
        GetComponent<Animator>().CrossFade("Special1", .025f);
        foreach (GameObject boy in GlobalVariables.friendlyArray)
        {
            if (boy.GetComponent<Character>().tags.Contains("Cultist") && boy != this.gameObject)
                boy.SendMessage("AttackOutOfTurn", .4f);
        }
        yield return new WaitForSeconds(.7f);
        enemy = GlobalVariables.enemiesSelected[0];
        yield return new WaitForSeconds(.01f);
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
                    character.DamageAmount = (int)((dmg + Random.Range(0, 26)) * 1.2f * character.CritMultiplier);

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
                    character.DamageAmount = (int)((dmg + Random.Range(0, 26)) * 1.2f);

                }
            //elemental Damage
            character.elementDamage(character.mainElement, enemy.GetComponent<Character>().tags);
            enemy.GetComponent<Character>().SendMessage("CasterSpecial1Target", character);
            
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
        enemy.GetComponent<Character>().SendMessage("abilityUpdate");
        yield return new WaitForSeconds(.01f);
        foreach (GameObject enemy2 in GlobalVariables.hostileArray)
        {
            enemy.tag = "Enemy";
            enemy2.tag = "EnemySelected";
            enemy = enemy2;
            yield return new WaitForSeconds(.01f);
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
                        character.DamageAmount = (int)((dmg + Random.Range(0, 26)) * 1.2f * character.CritMultiplier);

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
                        character.DamageAmount = (int)((dmg + Random.Range(0, 26)) * 1.2f);

                    }
                //elemental Damage
                character.elementDamage(character.mainElement, enemy.GetComponent<Character>().tags);
                enemy.GetComponent<Character>().SendMessage("CorruptCultistBasic", character);
                
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
            enemy.GetComponent<Character>().SendMessage("abilityUpdate");
            yield return new WaitForSeconds(.001f);
        }
        GetComponent<Animator>().CrossFade("Base Layer." + character.currentIdle, .2f);
        yield return new WaitForSeconds(1f);
        GlobalVariables.TakingTurn = false;
        for (int i = 0; i < buttons.Count; i++)
        {
            Destroy(buttons[i]);
        }
        buttons.Clear();
        if (!character.attackingOutOfTurn)
            basicButton.GetComponent<BasicButton>().moveUp(Buttons);        
        character.buffsAndDebuffs.SendMessage("EndOfTurn");
        character.turnUpdate = true;
    }

    IEnumerator projectile(GameObject enemy2, string ability)
    {
        enemy = enemy2;
        GameObject instance = Instantiate(arrow, hand.transform);
        instance.transform.position = hand.transform.position;
        instance.transform.parent = null;
        instance.transform.rotation = Quaternion.LookRotation(new Vector3(enemy.transform.position.x - transform.position.x, 0, enemy.transform.position.z - transform.position.z), Vector3.up);
        instance.transform.localScale *= 1.4f;
        int speed = 720;
        Vector3 enemyPos = new Vector3(enemy.GetComponent<Character>().initial.x, instance.transform.position.y, enemy.GetComponent<Character>().initial.z);
        Vector3 lTargetDir = enemyPos - instance.transform.position;
        lTargetDir.y = 0.0f;
        while (Vector3.Distance(enemyPos, instance.transform.position) > 2)
        {
            instance.transform.position = Vector3.MoveTowards(instance.transform.position, enemyPos, speed * Time.deltaTime);
            yield return new WaitForSeconds(.03f);
        }
        Destroy(instance);
        int a = character.accuracy;
        if (character.Stealth)
            a = 100;
        // if enemy dodges
        if ((Random.Range(0, 100) < enemy.GetComponent<Character>().dodge - a + 10) || enemy.GetComponent<Character>().Agility || character.Blind || enemy.GetComponent<Character>().TrueSight)
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
            if (!enemy.GetComponent<Character>().critImmunity && ((Random.Range(0, 100) < (character.CriticalChance - enemy.GetComponent<Character>().CriticalAvoidance)) || character.Focus || character.Stealth))
            {
                character.DamageAmount = (int)((dmg + Random.Range(0, 26)) * 1.75f * character.CritMultiplier);
                // 
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
                // 
            }
            //elemental Damage
            character.elementDamage(character.mainElement, enemy.GetComponent<Character>().tags);
            enemy.GetComponent<Character>().SendMessage("Caster" + ability, character);
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
        enemy.GetComponent<Character>().SendMessage("abilityUpdate");
    }
    
    public void MethodOnCultistSpecial()
    {
        bool armuk = false;
        for (int i = 0; i < GlobalVariables.friendlyArray.Length; i++)
        {
            if (GlobalVariables.friendlyArray[i].GetComponent<Character>().nameOfChar.Equals("Armuk") && !GlobalVariables.friendlyArray[i].GetComponent<Character>().dead)
            {
                armuk = true;
            }
        }
        if (armuk && !GlobalVariables.whomstJustUsedSpecial[0].name.Contains("Enemy") && GlobalVariables.whomstJustUsedSpecial[0].tags.Contains("Cultist") && Random.Range(0, 100) < 50 && GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.Corruption.getAmount() < 5 && GlobalVariables.enemiesSelected[0].GetComponent<Character>().nameOfChar.Contains("PlagueDoctor"))
        {
            GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.Corruption.burnAdd(1);
            if (!GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.DebuffList.Contains(GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.Corruption))
                GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.SendMessage("corruptionActivate");
            else
                GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.SendMessage("corruptionAdd");
            GlobalVariables.addToWhomstJustInflictedDebuff(character, "corruption");
        }
    }
    public void MethodOnEndTurn()
    {
        if (GlobalVariables.whomstJustEndedTurn[0].GetComponent<Character>().tags.Contains("Cultist") && !GlobalVariables.whomstJustEndedTurn[0].name.Contains("Enemy"))
        {
                foreach (GameObject boy in GlobalVariables.friendlyArray)
                {
                    if (boy.GetComponent<Character>().tags.Contains("Cultist") && !boy.GetComponent<Character>().healingImmunity && !boy.GetComponent<Character>().name.Contains("Enemy"))
                    {
                        GlobalVariables.DamageNumberGreen.GetComponent<TextMeshProUGUI>().text = "" + (int)(character.maxHealth * .02f * GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.Corruption.getAmount());
                        character.health += character.maxHealth * .02f * GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.Corruption.getAmount();
                    string[] e = new string[2]; e[0] = "Green"; e[1] = "me";
                    boy.GetComponent<Character>().damageNombers(e);
                    if (boy.GetComponent<Character>().health > boy.GetComponent<Character>().maxHealth)
                        boy.GetComponent<Character>().health = boy.GetComponent<Character>().maxHealth;
                        if (boy.GetComponent<Character>().health < boy.GetComponent<Character>().maxHealth / 4)
                            boy.GetComponent<Character>().healthArray[0].color = Color.red;
                        else if (boy.GetComponent<Character>().health < boy.GetComponent<Character>().maxHealth / 2)
                            boy.GetComponent<Character>().healthArray[0].color = Color.yellow;
                        else
                            boy.GetComponent<Character>().healthArray[0].color = Color.green;
                    }
                if (boy.GetComponent<Character>().tags.Contains("Risen") && !boy.GetComponent<Character>().name.Contains("Enemy"))
                {
                    boy.GetComponent<Character>().defense = (int)(boy.GetComponent<Character>().defense * (1 + .02f * GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.Corruption.getAmount()));
                    boy.GetComponent<Character>().offense = (int)(boy.GetComponent<Character>().offense * (1 + .02f * GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.Corruption.getAmount()));
                }
                }
            
        }
    }
}
