using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Praetorian : ParentCharacter
{
    
    // Start is called before the first frame update
    void Start()
    {
        GlobalVariables.OnGainedBuff += OnEnemyBuff;
        character.nameOfChar = "Praetorian";
        character.Gender = "other";
        //tags
        character.tags.Add("Tank");
        character.tags.Add("BlackLegion");
        character.tags.Add("Metal");
        character.tags.Add("Magic");
        character.mainElement = "Metal";
        //                                              Cooldown ---\      /---   Button in the index


        character.Armor = true;
        character.physArmor = true; //if you have true armor, just enable both
        character.physArmorAmount = .7f; // these values are almost always either .7f or 1, unless their kit is specified not to
        character.magArmorAmount = 1;

        #region stats
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
        StartCoroutine(establishCooldowns());
    }
    IEnumerator start()
    {
        yield return new WaitForSeconds(.01f);
        // this start() method is used for certain things that have to wait for all characters' stats to finalize before activating, such as leader abilities
    }
    

    // / / / / / / / / / / / / / / / / / / / / / / / / / / Abilities / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Basic Ability / / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void Basic(GameObject enemy2)
    {
        enemy = enemy2;
        gameObject.tag = "Ally";
        if (!character.attackingOutOfTurn)
        {
            character.turnMeter = 0;
            GlobalVariables.addToWhomstJustUsedBasic(this.GetComponent<Character>());
            basicButton.GetComponent<BasicButton>().moveDownAndBack(character.boutons.Count, Buttons);
        }
        if (!character.attackingOutOfTurn)
        {
            for (int i = 0; i < character.boutons.Count; i++)
            {
                character.boutons[i].currentCooldown -= 1;
            }
        }
        //look at enemy
        StartCoroutine(MoveToEnemy(.8f, "Basic", 5));
        StartCoroutine(LookAtEnemy());
    }
    IEnumerator basic()
    {
        gameObject.tag = "Ally";
        yield return new WaitForSeconds(.3f);

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
            // if character.criticalhit or not
            if (!enemy.GetComponent<Character>().critImmunity && ((Random.Range(0, 100) < (character.CriticalChance - enemy.GetComponent<Character>().CriticalAvoidance)) || character.Focus))
            {
                character.DamageAmount = (int)((dmg + Random.Range(0, 26)) * 1.5f * character.CritMultiplier);

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
                character.DamageAmount = (int)((dmg + Random.Range(0, 26)) * 1.5f);

            }
            //elemental Damage
            character.elementDamage(character.mainElement, enemy.GetComponent<Character>().tags);
            enemy.GetComponent<Character>().SendMessage("PraetorianBasic", this.GetComponent<Character>());
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
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Special Ability 1 / / / / / / / / / / / / / / / / / / / / / / / / / / / / /    
    public void Special1()
    {
        gameObject.tag = "Ally";
        basicButton.GetComponent<BasicButton>().moveDownAndBack(character.boutons.Count, Buttons);
    character.turnMeter = 0;
        GlobalVariables.addToWhomstJustUsedSpecial(this.GetComponent<Character>());
    StartCoroutine(special1());
    }
IEnumerator special1()
    {
    gameObject.tag = "Ally";
        GetComponentInChildren<Animator>().CrossFade("Guard", .25f);
        yield return new WaitForSeconds(.57f);
        if (!character.buffImmunity)
        {
            character.buffsAndDebuffs.Taunt.add(2);
            if (!character.buffsAndDebuffs.BuffList.Contains(character.buffsAndDebuffs.Taunt))
                character.buffsAndDebuffs.SendMessage("tauntActivate");
        }
        character.buffsAndDebuffs.Guard.add(2);
        if (!character.buffsAndDebuffs.BlueBuffList.Contains(character.buffsAndDebuffs.Guard))
            character.buffsAndDebuffs.SendMessage("guardActivate");
        character.buffsAndDebuffs.ImageArray();
        foreach (GameObject ally in GlobalVariables.friendlyArray)
        {
            if (!ally.GetComponent<Character>().buffsAndDebuffs.DebuffList.Contains(ally.GetComponent<Character>().buffsAndDebuffs.BuffImmunity))
            {
                ally.GetComponent<Character>().buffsAndDebuffs.DefenseUp.add(2);
                if (!ally.GetComponent<Character>().buffsAndDebuffs.BuffList.Contains(character.buffsAndDebuffs.DefenseUp))
                    ally.GetComponent<Character>().buffsAndDebuffs.SendMessage("defenseUpActivate");
                ally.GetComponent<Character>().buffsAndDebuffs.BonusProtection.add(2);
                if (!ally.GetComponent<Character>().buffsAndDebuffs.BuffList.Contains(character.buffsAndDebuffs.BonusProtection))
                    ally.GetComponent<Character>().buffsAndDebuffs.SendMessage("bonusProtectionActivate", 10);
                else
                    ally.GetComponent<Character>().buffsAndDebuffs.SendMessage("bonusProtectionAdd", 10);
                ally.GetComponent<Character>().buffsAndDebuffs.ImageArray();
                GlobalVariables.addToWhomstJustInflictedBuff(character, "defenseUp");
                GlobalVariables.addToWhomstJustInflictedBuff(character, "bonusProtection");
            }
            ally.GetComponent<Character>().buffsAndDebuffs.DarkEnergy.burnAdd(1);
            if (!ally.GetComponent<Character>().buffsAndDebuffs.BlueBuffList.Contains(character.buffsAndDebuffs.DarkEnergy))
                ally.GetComponent<Character>().buffsAndDebuffs.SendMessage("darkEnergyActivate");
            ally.GetComponent<Character>().buffsAndDebuffs.ImageArray();
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
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Special Ability 1 / / / / / / / / / / / / / / / / / / / / / / / / / / / / /    
    public void Special2(GameObject enemy2)
    {
        enemy = enemy2;
        gameObject.tag = "Ally";
        basicButton.GetComponent<BasicButton>().moveDownAndBack(character.boutons.Count, Buttons);
    character.turnMeter = 0;
        GlobalVariables.addToWhomstJustUsedSpecial(this.GetComponent<Character>());
    StartCoroutine(special2());
    }
IEnumerator special2()
    {
    gameObject.tag = "Ally";
    GetComponentInChildren<Animator>().CrossFade("Special2", .025f);
    yield return new WaitForSeconds(.57f);
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
                character.DamageAmount = (int)((dmg + Random.Range(0, 26)) * 2f * character.CritMultiplier);

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
                character.DamageAmount = (int)((dmg + Random.Range(0, 26)) * 2f);

            }
            //elemental Damage
            character.elementDamage(character.mainElement, enemy.GetComponent<Character>().tags);
            enemy.GetComponent<Character>().SendMessage("PraetorianSpecial2", character);
            if (!enemy.GetComponent<Character>().stunImmunity) {
                enemy.GetComponent<Character>().buffsAndDebuffs.Stun.add(1);
                if (!enemy.GetComponent<Character>().buffsAndDebuffs.DebuffList.Contains(enemy.GetComponent<Character>().buffsAndDebuffs.Stun))
                    enemy.GetComponent<Character>().buffsAndDebuffs.SendMessage("stunActivate");
            }
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
                    character.DamageAmount = (int)((dmg + Random.Range(0, 26)) * 2f * character.CritMultiplier);

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
                    character.DamageAmount = (int)((dmg + Random.Range(0, 26)) * 2f);

                }
                //elemental Damage
                character.elementDamage(character.mainElement, enemy.GetComponent<Character>().tags);
                enemy.GetComponent<Character>().SendMessage("PraetorianSpecial2", character);
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
        character.buffsAndDebuffs.ImageArray();
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
            if (speed < 360)
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
        if (!character.attackingOutOfTurn) { GetComponentInChildren<Animator>().CrossFade("Base Layer." + character.currentIdle, .5f); } else { GetComponentInChildren<Animator>().Play("Base Layer." + character.currentIdle); }
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
        #endregion
    }

    private void OnEnemyBuff()
    {
        if (GlobalVariables.whomstJustGainedBuff[0].name.Contains("Enemy"))
        {
            if (!character.buffImmunity)
            {
                character.buffsAndDebuffs.HealthUp.add(1);
                if (!character.buffsAndDebuffs.BuffList.Contains(character.buffsAndDebuffs.HealthUp))
                    character.buffsAndDebuffs.SendMessage("healthUpActivate");
            }
            if (!character.turnMeterUpImmunity)
            {
                character.turnMeter += 5;
                if (character.turnMeter > 100)
                    character.turnMeter = 100;
            }
            character.buffsAndDebuffs.ImageArray();
        }
        else if (GlobalVariables.whomstJustGainedBuff[0] == character && GlobalVariables.buffGained[0].Equals("bonusProtection") && !character.buffImmunity)
        {
            character.buffsAndDebuffs.Taunt.add(1);
            if (!character.buffsAndDebuffs.BuffList.Contains(character.buffsAndDebuffs.Taunt))
                character.buffsAndDebuffs.SendMessage("tauntActivate");
            character.buffsAndDebuffs.ImageArray();
        }
    }
}
