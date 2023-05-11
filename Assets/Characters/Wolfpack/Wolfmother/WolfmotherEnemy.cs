using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WolfmotherEnemy : ParentEnemy
{
    void Start()
    {
        character.nameOfChar = "Wolfmother";
        character.Gender = "female";
        //tags
        string[] tags = { "Leader", "Attacker", "Support", "Wolfpack", "Metal" };
        character.mainElement = "Metal";
        character.stunImmunity = true;
        character.turnMeterDownImmunity = true;
        character.abilityBlockImmunity = true;

        character.Armor = true;
        character.physArmor = true; //if you have true armor, just enable both
        character.physArmorAmount = .7f; // these values are almost always either .7f or 1, unless their kit is specified not to
        character.magArmorAmount = 1;

        #region stats
        if (character.isLeader)
        {
            gameObject.tag = "EnemySelected";
        }
        //setting the stats of the character
        GetComponentInChildren<Animator>().CrossFade("Base Layer." + character.currentIdle, .25f);
        if (!character.Armor || character.level <= 50)
        {
            character.armor = 0;
        }
        if (character.level < 50)
            character.protection = 0;
        dmg = character.DamageAmount;
        // if you want these to be larger, make it 79; if you want it to accumulate to less make it 129
        character.health = (int)(character.health * ((character.level - 1) * 8 / 99f + 1) * ((Mathf.Pow((float)character.star - 1, 2) / 4f) + 1));
        character.maxHealth = character.health;
        character.protection = (int)(character.protection * ((character.level - 1) * 8 / 99f + 1) * ((Mathf.Pow((float)character.star - 1, 2) / 4f) + 1));
        character.maxProtection = character.protection;
        character.offense = (int)(character.offense * ((character.level - 1) * 4 / 99f + 1) * ((Mathf.Pow((float)character.star - 1, 2) / 4f) + 1));
        dmg = (int)(dmg * ((character.level - 1) * 14 / 99f + 1) * ((Mathf.Pow((float)character.star - 1, 2) / 4f) + 1));
        character.defense = (int)(character.defense * ((character.level - 1) * 4 / 99f + 1) * ((Mathf.Pow((float)character.star - 1, 2) / 6f) + 1));
        character.speed = (int)(character.speed * ((character.level - 1) * 1 / 199f + 1));
        // these are the final values and start from 0, so thonk about it a little bit
        character.Potency += (int)(character.level * .5f);
        character.Tenacity += (int)(character.level * .5f);
        #endregion

        StartCoroutine(start());
        setTags(tags);
        StartCoroutine(establishCooldowns());
    }
    IEnumerator start()
    {
        yield return new WaitForSeconds(.01f);
        // this start() method is used for certain things that have to wait for all characters' stats to finalize before activating, such as leader abilities
        if (character.isLeader)
        {
            GlobalVariables.EnemyLeaders[0] = "Wolfmother";
            GlobalVariables.OnCrit += methodOnCrit;

            if (GlobalVariables.enemiesSelected[0].GetComponent<Character>().tags.Contains("Wolfpack"))
            {
                GlobalVariables.enemiesSelected[0].GetComponent<Character>().TauntIgnore = true;
                GlobalVariables.enemiesSelected[0].GetComponent<Character>().abilityBlock = true;
                GlobalVariables.enemiesSelected[0].GetComponent<Character>().turnMeter = 99;
            }
            foreach (GameObject boy in GlobalVariables.hostileArray)
            {
                if (boy.GetComponent<Character>().tags.Contains("Wolfpack"))
                {
                    boy.GetComponent<Character>().TauntIgnore = true;
                    boy.GetComponent<Character>().abilityBlock = true;
                    boy.GetComponent<Character>().turnMeter = 99;
                }
            }
        }
    }

    // / / / / / / / / / / / / / / / / / / / / / / / / / / Abilities / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Basic Ability / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void Basic(GameObject enemy2)
    {
        enemy = enemy2;
        gameObject.tag = "Enemy";
        enemy.gameObject.tag = "PlayerSelected";
        if (!character.attackingOutOfTurn)
        {
            character.turnMeter -= 100;
            GlobalVariables.addToWhomstJustUsedBasic(this.GetComponent<Character>());
        }
        //look at enemy
        StartCoroutine(MoveToEnemy(1.45f, "Basic", 5));
        StartCoroutine(LookAtEnemy());
    }
    IEnumerator basic()
    {
        gameObject.tag = "Enemy";
        yield return new WaitForSeconds(.4f);
        GlobalSounds.playSlash(character.audioSource1);
        yield return new WaitForSeconds(.6f);

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
            // if character.criticalhit or not
            if (!enemy.GetComponent<Character>().critImmunity && ((Random.Range(0, 100) < (character.CriticalChance - enemy.GetComponent<Character>().CriticalAvoidance)) || character.Focus))
            {
                character.DamageAmount = (int)((dmg + Random.Range(0, 26)) * 2.25f * character.CritMultiplier);

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
                character.DamageAmount = (int)((dmg + Random.Range(0, 26)) * 2.25f);

            }
            //elemental Damage
            character.elementDamage(character.mainElement, enemy.GetComponent<Character>().tags);
            enemy.GetComponent<Character>().SendMessage(character.nameOfChar + "Basic", this.GetComponent<Character>());
            GlobalVariables.DamageNumberRed.GetComponent<TextMeshProUGUI>().text = "" + enemy.GetComponent<Character>().damage;
            // countering
            if (Random.Range(0, 100) < enemy.GetComponent<Character>().counter && !character.attackingOutOfTurn && !character.buffsAndDebuffs.BuffList.Contains(character.buffsAndDebuffs.Stealth))
            {
                StartCoroutine(Counter(1f));
            }
            // character.health steal
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
    public void Special1(GameObject enemy2)
    {
        enemy = enemy2;
        gameObject.tag = "Enemy";
        character.turnMeter = 0;
        GlobalVariables.addToWhomstJustUsedSpecial(this.GetComponent<Character>());
        StartCoroutine(special1());
    }
    IEnumerator special1()
        {
        gameObject.tag = "Enemy";
        GetComponentInChildren<Animator>().CrossFade("Special1", .025f);
        yield return new WaitForSeconds(.57f);
        //Do something
        if (GlobalVariables.enemiesSelected[0].GetComponent<Character>().tags.Contains("Wolfpack"))
        {
            //35% turn meter
            if (!GlobalVariables.enemiesSelected[0].GetComponent<Character>().turnMeterUpImmunity)
            {
                GlobalVariables.enemiesSelected[0].GetComponent<Character>().turnMeter += 35;
                if (GlobalVariables.enemiesSelected[0].GetComponent<Character>().turnMeter > 100)
                {
                    GlobalVariables.enemiesSelected[0].GetComponent<Character>().turnMeter = 100;
                }
            }
            //offense up
            if (!GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.DebuffList.Contains(GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.BuffImmunity))
            {
                GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.OffenseUp.add(2);
                if (!GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.BuffList.Contains(GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.OffenseUp))
                    GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.SendMessage("offenseUpActivate");
                GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.ImageArray();
            }
            //attackers
            if (GlobalVariables.enemiesSelected[0].GetComponent<Character>().canAttackOutOfTurn && GlobalVariables.enemiesSelected[0].GetComponent<Character>().tags.Contains("Attacker"))
            {
                if (enemy.GetComponent<Character>().isLeader)
                    GlobalVariables.enemiesSelected[0].SendMessage("AttackOutOfTurn", 1);
                else
                    GlobalVariables.enemiesSelected[0].SendMessage("AttackOutOfTurn", .5f);
            }
            //tanks
            if (GlobalVariables.enemiesSelected[0].GetComponent<Character>().tags.Contains("Tank") && !GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.DebuffList.Contains(GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.BuffImmunity))
            {
                GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.DefenseUp.add(2);
                if (!GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.BuffList.Contains(GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.DefenseUp))
                    GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.SendMessage("defenseUpActivate");
                GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.ResilienceUp.add(2);
                if (!GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.BuffList.Contains(GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.ResilienceUp))
                    GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.SendMessage("resilienceUpActivate");
                GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.ImageArray();
            }
            //supports
            if (GlobalVariables.enemiesSelected[0].GetComponent<Character>().tags.Contains("Support"))
            {
                if (!GlobalVariables.enemiesSelected[0].GetComponent<Character>().turnMeterUpImmunity)
                {
                    GlobalVariables.enemiesSelected[0].GetComponent<Character>().turnMeter += 35;
                    if (GlobalVariables.enemiesSelected[0].GetComponent<Character>().turnMeter > 100)
                    {
                        GlobalVariables.enemiesSelected[0].GetComponent<Character>().turnMeter = 100;
                    }
                }
                if (!GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.DebuffList.Contains(GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.BuffImmunity))
                {
                    GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.SpeedUp.add(2);
                    if (!GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.BuffList.Contains(GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.SpeedUp))
                        GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.SendMessage("speedUpActivate");
                    GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.ImageArray();
                }
            }
        }
        foreach (GameObject boy in GlobalVariables.hostileArray)
        {
            if (boy.GetComponent<Character>().tags.Contains("Wolfpack"))
            {
                //35% turn meter
                if (!boy.GetComponent<Character>().turnMeterUpImmunity)
                {
                    boy.GetComponent<Character>().turnMeter += 35;
                    if (boy.GetComponent<Character>().turnMeter > 100)
                    {
                        boy.GetComponent<Character>().turnMeter = 100;
                    }
                }
                //offense up
                if (!boy.GetComponent<Character>().buffsAndDebuffs.DebuffList.Contains(boy.GetComponent<Character>().buffsAndDebuffs.BuffImmunity))
                {
                    boy.GetComponent<Character>().buffsAndDebuffs.OffenseUp.add(2);
                    if (!boy.GetComponent<Character>().buffsAndDebuffs.BuffList.Contains(boy.GetComponent<Character>().buffsAndDebuffs.OffenseUp))
                        boy.GetComponent<Character>().buffsAndDebuffs.SendMessage("offenseUpActivate");
                    boy.GetComponent<Character>().buffsAndDebuffs.ImageArray();
                }
                //attackers
                if (boy.GetComponent<Character>().canAttackOutOfTurn && boy.GetComponent<Character>().tags.Contains("Attacker"))
                {
                    if (enemy.GetComponent<Character>().isLeader)
                        boy.SendMessage("AttackOutOfTurn", 1);
                    else
                        boy.SendMessage("AttackOutOfTurn", .5f);
                }
                //tanks
                if (boy.GetComponent<Character>().tags.Contains("Tank") && !boy.GetComponent<Character>().buffsAndDebuffs.DebuffList.Contains(boy.GetComponent<Character>().buffsAndDebuffs.BuffImmunity))
                {
                    boy.GetComponent<Character>().buffsAndDebuffs.DefenseUp.add(2);
                    if (!boy.GetComponent<Character>().buffsAndDebuffs.BuffList.Contains(boy.GetComponent<Character>().buffsAndDebuffs.DefenseUp))
                        boy.GetComponent<Character>().buffsAndDebuffs.SendMessage("defenseUpActivate");
                    boy.GetComponent<Character>().buffsAndDebuffs.ResilienceUp.add(2);
                    if (!boy.GetComponent<Character>().buffsAndDebuffs.BuffList.Contains(boy.GetComponent<Character>().buffsAndDebuffs.ResilienceUp))
                        boy.GetComponent<Character>().buffsAndDebuffs.SendMessage("resilienceUpActivate");
                    boy.GetComponent<Character>().buffsAndDebuffs.ImageArray();
                }
                //supports
                if (boy.GetComponent<Character>().tags.Contains("Support"))
                {
                    if (!boy.GetComponent<Character>().turnMeterUpImmunity)
                    {
                        boy.GetComponent<Character>().turnMeter += 35;
                        if (boy.GetComponent<Character>().turnMeter > 100)
                        {
                            boy.GetComponent<Character>().turnMeter = 100;
                        }
                    }
                    if (!boy.GetComponent<Character>().buffsAndDebuffs.DebuffList.Contains(boy.GetComponent<Character>().buffsAndDebuffs.BuffImmunity))
                    {
                        boy.GetComponent<Character>().buffsAndDebuffs.SpeedUp.add(2);
                        if (!boy.GetComponent<Character>().buffsAndDebuffs.BuffList.Contains(boy.GetComponent<Character>().buffsAndDebuffs.SpeedUp))
                            boy.GetComponent<Character>().buffsAndDebuffs.SendMessage("speedUpActivate");
                        boy.GetComponent<Character>().buffsAndDebuffs.ImageArray();
                    }
                }
            }
        }


        character.buffsAndDebuffs.ImageArray();
        yield return new WaitForSeconds(.8f);
        GetComponentInChildren<Animator>().CrossFade("Base Layer." + character.currentIdle, .25f);
        if (!character.attackingOutOfTurn)
            cooldownChange(-1);
        enemy.GetComponent<Character>().SendMessage("abilityUpdate");
        GlobalVariables.TakingTurn = false;
        character.buffsAndDebuffs.SendMessage("EndOfTurn");
        character.turnUpdate = true;
    }
    public void Special2(GameObject enemy2)
    {
        enemy = enemy2;
        gameObject.tag = "Enemy";
        character.turnMeter = 0;
        GlobalVariables.addToWhomstJustUsedSpecial(this.GetComponent<Character>());
        StartCoroutine(special2());
    }
    IEnumerator special2()
        {
        gameObject.tag = "Enemy";
        GetComponentInChildren<Animator>().CrossFade("Special2", .025f);
        yield return new WaitForSeconds(.57f);
        //Do something
        if (GlobalVariables.enemiesSelected[0].GetComponent<Character>().canAttackOutOfTurn && GlobalVariables.enemiesSelected[0] != this.gameObject)
        {
            if (!enemy.GetComponent<Character>().critImmunity && ((Random.Range(0, 100) < (GlobalVariables.enemiesSelected[0].GetComponent<Character>().CriticalChance - enemy.GetComponent<Character>().CriticalAvoidance)) || character.Focus))
            {
                GlobalVariables.enemiesSelected[0].GetComponent<Character>().Focus = true;
                character.boutons[1].currentCooldown--;
                if (character.boutons[1].currentCooldown < 0)
                    character.boutons[1].currentCooldown = 0;
            }
            if (enemy.GetComponent<Character>().isLeader)
            {
                if (GlobalVariables.enemiesSelected[0].GetComponent<Character>().tags.Contains("Wolfpack"))
                    GlobalVariables.enemiesSelected[0].SendMessage("AttackOutOfTurn", 1.7f);
                else
                    GlobalVariables.enemiesSelected[0].SendMessage("AttackOutOfTurn", 1.4f);
            }
            else
            {
                if (GlobalVariables.enemiesSelected[0].GetComponent<Character>().tags.Contains("Wolfpack"))
                    GlobalVariables.enemiesSelected[0].SendMessage("AttackOutOfTurn", .85f);
                else
                    GlobalVariables.enemiesSelected[0].SendMessage("AttackOutOfTurn", .7f);
            }
        }
        foreach (GameObject boy in GlobalVariables.hostileArray)
        {
            if (boy.GetComponent<Character>().canAttackOutOfTurn && boy != this.gameObject)
            {
                if (!enemy.GetComponent<Character>().critImmunity && ((Random.Range(0, 100) < (boy.GetComponent<Character>().CriticalChance - enemy.GetComponent<Character>().CriticalAvoidance)) || character.Focus))
                {
                    boy.GetComponent<Character>().Focus = true;
                    character.boutons[1].currentCooldown--;
                    if (character.boutons[1].currentCooldown < 0)
                        character.boutons[1].currentCooldown = 0;
                }
                if (enemy.GetComponent<Character>().isLeader)
                {
                    if (boy.GetComponent<Character>().tags.Contains("Wolfpack"))
                        boy.SendMessage("AttackOutOfTurn", 1.7f);
                    else
                        boy.SendMessage("AttackOutOfTurn", 1.4f);
                }
                else
                {
                    if (boy.GetComponent<Character>().tags.Contains("Wolfpack"))
                        boy.SendMessage("AttackOutOfTurn", .85f);
                    else
                        boy.SendMessage("AttackOutOfTurn", .7f);
                }
            }
        }
        character.buffsAndDebuffs.ImageArray();
        yield return new WaitForSeconds(.8f);
        GetComponentInChildren<Animator>().CrossFade("Base Layer." + character.currentIdle, .25f);
        if (!character.attackingOutOfTurn)
            cooldownChange(-1);
        enemy.GetComponent<Character>().SendMessage("abilityUpdate");
        GlobalVariables.TakingTurn = false;
        character.buffsAndDebuffs.SendMessage("EndOfTurn");
        character.turnUpdate = true;
    }

// / / / / / / / / / / / / / / / / / / / / / / / / / / Delete these if the character is all ranged / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
IEnumerator MoveToEnemy(float wait, string ability, int distance)
    {
        //For the Regions marked "Don't Edit" you can in fact edit them, but usually that's only for characters who have custom animation combos like Templar Pikeman and Virion
        #region Don't Edit
        int speed = 0;
        Vector3 enemyPos = new Vector3(enemy.GetComponent<Character>().initial.x, enemy.GetComponent<Character>().initial.y, enemy.GetComponent<Character>().initial.z);
        Vector3 lTargetDir = enemyPos - transform.position;
        lTargetDir.y = 0.0f;
        GetComponentInChildren<Animator>().CrossFade("Base Layer.Sprint", .01f);
        while (Vector3.Distance(enemyPos, transform.position) > 2 + distance)
        {
            transform.position = Vector3.MoveTowards(transform.position, enemyPos, speed * Time.deltaTime);
            if (speed < 420)
                speed += 40;
            yield return new WaitForSeconds(.03f);
        }
        #endregion
        switch (ability) //Edit the elements of the switch stament however you need to, but don't mess with the statement itself
        {
            case "Basic":
                GetComponentInChildren<Animator>().CrossFade("Base Layer.Basic", .3f);
                StartCoroutine(basic());
                break;
            case "Special1":
                GetComponentInChildren<Animator>().CrossFade("Base Layer.Special1", .3f);
                StartCoroutine(special1());
                break;
        }
        #region Don't Edit
        yield return new WaitForSeconds(wait);
        speed = 0;
        GetComponentInChildren<Animator>().CrossFade("Base Layer.Hop", .25f);
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
        if (!character.attackingOutOfTurn) { GetComponentInChildren<Animator>().CrossFade("Base Layer." + character.currentIdle, .5f); } else { GetComponentInChildren<Animator>().Play("Base Layer." + character.currentIdle); }
        GlobalVariables.TakingTurn = false;
        if (!character.attackingOutOfTurn)
            character.buffsAndDebuffs.SendMessage("EndOfTurn");
        character.turnUpdate = true;
        #endregion
    }
    public void methodOnCrit()
    {
        if (GlobalVariables.whomstJustCrit[0].tags.Contains("Wolfpack") && GlobalVariables.whomstJustCrit[0].name.Contains("Enemy"))
        {
            if (!GlobalVariables.whomstJustCrit[0].turnMeterUpImmunity)
            {
                GlobalVariables.whomstJustCrit[0].turnMeter += 10;
            }
            if (!GlobalVariables.enemiesSelected[0].GetComponent<Character>().turnMeterUpImmunity && GlobalVariables.enemiesSelected[0].GetComponent<Character>().tags.Contains("Wolfpack") && GlobalVariables.enemiesSelected[0].GetComponent<Character>() != GlobalVariables.whomstJustCrit[0])
            {
                GlobalVariables.enemiesSelected[0].GetComponent<Character>().turnMeter += 5;
            }
            foreach (GameObject boy in GlobalVariables.hostileArray)
            {
                if (!boy.GetComponent<Character>().turnMeterUpImmunity && boy.GetComponent<Character>().tags.Contains("Wolfpack") && boy.GetComponent<Character>() != GlobalVariables.whomstJustCrit[0])
                {
                    boy.GetComponent<Character>().turnMeter += 5;
                    if (boy.GetComponent<Character>().turnMeter > 100 && !(boy.GetComponent<Character>().abilityBlock && !boy.GetComponent<Character>().buffsAndDebuffs.DebuffList.Contains(boy.GetComponent<Character>().buffsAndDebuffs.AbilityBlock)))
                    {
                        boy.GetComponent<Character>().turnMeter = 100;
                    }
                }
            }
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
        enemy = currentEnemy;
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
        else if (character.abilities.CurrentAbilityCooldowns[1] == 0 && !character.abilityBlock)
        {
            Special2(currentEnemy);
            character.abilities.CurrentAbilityCooldowns[1] = character.abilities.AbilityCooldowns[1];
        }
        else
        {
            Basic(currentEnemy);
        }
    }
}
