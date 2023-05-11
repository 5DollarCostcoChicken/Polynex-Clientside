using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlagueDoctor : ParentCharacter
{
    
    // Start is called before the first frame update
    void Start()
    {
        //GlobalVariables.OnGotSelectedAlly += beans;


        character.nameOfChar = "PlagueDoctor";
        character.Gender = "male";
        //tags
        character.tags.Add("Support");
        character.tags.Add("Resistance");
        character.tags.Add("Fire");
        character.tags.Add("Nightmare");
        character.tags.Add("Magic");
        character.mainElement = "Fire";

        //setting the stats of the character
        GetComponent<Animator>().CrossFade("Base Layer." + character.currentIdle, .25f);
        if (character.level < 50)
            character.protection = 0;
        dmg = character.DamageAmount;
        character.Armor = true;
        character.magArmor = true; //if you have true armor, just enable both
        character.physArmorAmount = 1;
        character.magArmorAmount = .7f;
        character.health = (int)(character.health * ((character.level - 1) * 8 / 79f + 1) * ((Mathf.Pow((float)character.star - 1, 2) / 4f) + 1));
        character.maxHealth = character.health;
        character.protection = (int)(character.protection * ((character.level - 1) * 8 / 79f + 1) * ((Mathf.Pow((float)character.star - 1, 2) / 4f) + 1));
        character.maxProtection = character.protection;
        character.offense = (int)(character.offense * ((character.level - 1) * 4 / 129f + 1) * ((Mathf.Pow((float)character.star - 1, 2) / 4f) + 1));
        dmg = (int)(dmg * ((character.level - 1) * 14 / 99f + 1) * ((Mathf.Pow((float)character.star - 1, 2) / 4f) + 1));
        character.defense = (int)(character.defense * ((character.level - 1) * 3 / 79f + 1) * ((Mathf.Pow((float)character.star - 1, 2) / 6f) + 1));
        character.speed = (int)(character.speed * ((character.level - 1) * 1 / 199f + 1));
        character.Potency += (int)(character.level * .8f);
        character.Tenacity += (int)(character.level * .7f);
        if (!character.Armor || character.level <= 50)
        {
            character.armor = 0;
        }
        character.stunImmunity = true;
        StartCoroutine(start());
        StartCoroutine(establishCooldowns());
    }
    IEnumerator start()
    {
        yield return new WaitForSeconds(.1f);
        //character.blockSound = GlobalSounds.Anvil1;
        //character.parrySound = GlobalSounds.Dink1;
        if (GlobalVariables.enemiesSelected.Length > 0)
        {
            if (GlobalVariables.enemiesSelected[0].GetComponent<Character>().tags.Contains("Nightmare"))
            {
                GlobalVariables.enemiesSelected[0].GetComponent<Character>().CriticalChance /= 2;
                GlobalVariables.enemiesSelected[0].GetComponent<Character>().Potency -= 50;
            }
        }
        foreach (GameObject boy in GlobalVariables.hostileArray)
        {
            if (boy.GetComponent<Character>().tags.Contains("Nightmare"))
            {
                boy.GetComponent<Character>().CriticalChance /= 2;
                boy.GetComponent<Character>().Potency -= 50;
            }
        }

        if (character.isLeader)
        {
            GlobalVariables.Leaders[0] = "PlagueDoctor";
            bool enemies = true;
            if (!GlobalVariables.enemiesSelected[0].GetComponent<Character>().tags.Contains("Wolfpack") && !GlobalVariables.enemiesSelected[0].GetComponent<Character>().tags.Contains("Nightmare"))
            {
                enemies = false;
            }
            foreach (GameObject boy in GlobalVariables.hostileArray)
            {
                if (!boy.GetComponent<Character>().tags.Contains("Nightmare") && !boy.GetComponent<Character>().tags.Contains("Wolfpack"))
                    enemies = false;
            }
            if (enemies)
            {
                GlobalVariables.enemiesSelected[0].GetComponent<Character>().PhysResist -= 50;
                GlobalVariables.enemiesSelected[0].GetComponent<Character>().MagResist -= 50;
            }
            foreach (GameObject boy in GlobalVariables.hostileArray)
            {
                if (enemies)
                {
                    boy.GetComponent<Character>().PhysResist -= 50;
                    boy.GetComponent<Character>().MagResist -= 50;
                }
            }
            foreach (GameObject boy in GlobalVariables.friendlyArray)
            {
                if (boy.GetComponent<Character>().tags.Contains("Resistance") && enemies)
                {
                    boy.GetComponent<Character>().Tenacity = (int)(boy.GetComponent<Character>().Tenacity * 1.5f);
                }
            }
        }
    }
   
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Basic Ability / / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void Basic(GameObject enemy2)
    {
        enemy = enemy2;
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
        //look at enemy
        StartCoroutine(MoveToEnemy(.65f, "Basic", 6));
        StartCoroutine(LookAtEnemy());
    }
    IEnumerator basic()
    {
        gameObject.tag = "Ally";
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
            GlobalVariables.addToWhomstJustParried(enemy.GetComponent<Character>()); //tells globalvariables that the enemy just dodged
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
            enemy.GetComponent<Character>().SendMessage("PlagueDoctorBasic", character);
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
        if (enemy.GetComponent<Character>().buffsAndDebuffs.DebuffList.Contains(enemy.GetComponent<Character>().buffsAndDebuffs.PotencyDown))
        {
            if (character.boutons[0].currentCooldown != 0)
                character.boutons[0].currentCooldown -= 1;
            if (character.boutons[1].currentCooldown != 0)
                character.boutons[1].currentCooldown -= 1;
        }
        enemy.GetComponent<Character>().SendMessage("abilityUpdate");
    }
    
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Special Ability 1 / / / / / / / / / / / / / / / / / / / / / / / / / / / / /

    public void Special1(GameObject enemy2)
    {
        enemy = enemy2;
        gameObject.tag = "Ally";
        basicButton.GetComponent<BasicButton>().moveDownAndBack(character.boutons.Count, Buttons);
        character.turnMeter = 0;
        GlobalVariables.addToWhomstJustUsedSpecial(character);
        StartCoroutine(special1());
    }
    IEnumerator special1()
    {
        gameObject.tag = "Ally";
        GetComponent<Animator>().CrossFade("Special1", .025f);
        yield return new WaitForSeconds(1.27f);
        int burn = 0;
        if (enemy.GetComponent<Character>().tags.Contains("Nightmare"))
            burn++;
        foreach (GameObject boy in GlobalVariables.hostileArray)
        {
            if (boy.GetComponent<Character>().tags.Contains("Nightmare"))
            {
                burn++;
                if (boy.GetComponent<Character>().isLeader)
                    burn++;
            }
        }
        foreach (Effect debuff in character.buffsAndDebuffs.DebuffList)
        {
            if (debuff.numbered)
                burn += debuff.getAmount();
            else
                burn++;
        }
        foreach (Effect debuff in character.buffsAndDebuffs.BurnList)
        {
            if (debuff.numbered && !debuff.getName().Equals("heal"))
                burn += debuff.getAmount();
            else
                burn++;
        }
        character.buffsAndDebuffs.cleanseDebuffs();
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
            enemy.GetComponent<Character>().SendMessage("PlagueDoctorSpecial1", character);
            enemy.GetComponent<Character>().buffsAndDebuffs.Burning.burnAdd(burn);
            if (!enemy.GetComponent<Character>().buffsAndDebuffs.BurnList.Contains(enemy.GetComponent<Character>().buffsAndDebuffs.Burning) && burn > 0)
                enemy.GetComponent<Character>().buffsAndDebuffs.SendMessage("burningActivate");
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
                enemy.GetComponent<Character>().SendMessage("PlagueDoctorSpecial1", character);
                enemy.GetComponent<Character>().buffsAndDebuffs.Burning.burnAdd(burn);
                if (!enemy.GetComponent<Character>().buffsAndDebuffs.BurnList.Contains(enemy.GetComponent<Character>().buffsAndDebuffs.Burning) && burn > 0)
                    enemy.GetComponent<Character>().buffsAndDebuffs.SendMessage("burningActivate");
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
    public void Special2(GameObject enemy2)
    {
        enemy = enemy2;
        gameObject.tag = "Ally";
        basicButton.GetComponent<BasicButton>().moveDownAndBack(character.boutons.Count, Buttons);
        character.turnMeter = 0;
        GlobalVariables.addToWhomstJustUsedSpecial(character);
        StartCoroutine(Special2());
    }
    IEnumerator Special2()
    {
        gameObject.tag = "Ally";
        GetComponent<Animator>().CrossFade("Special2", .025f);
        int count = 0;
        foreach (Effect debuff in character.buffsAndDebuffs.DebuffList)
        {
            if (debuff.numbered)
                count += debuff.getAmount();
            else
                count++;
        }
        foreach (Effect debuff in character.buffsAndDebuffs.BurnList)
        {
            if (debuff.numbered && !debuff.getName().Equals("heal"))
                count += debuff.getAmount();
            else
                count++;
        }
        character.buffsAndDebuffs.cleanseDebuffs();
        foreach (GameObject ally in GlobalVariables.friendlyArray)
        {
            foreach (Effect debuff in ally.GetComponent<Character>().buffsAndDebuffs.DebuffList)
            {
                if (debuff.numbered)
                    count += debuff.getAmount();
                else
                    count++;
            }
            foreach (Effect debuff in ally.GetComponent<Character>().buffsAndDebuffs.BurnList)
            {
                if (debuff.numbered && !debuff.getName().Equals("heal"))
                    count += debuff.getAmount();
                else
                    count++;
            }
            ally.GetComponent<Character>().buffsAndDebuffs.cleanseDebuffs();
            ally.GetComponent<Character>().buffsAndDebuffs.ImageArray();
        }
        foreach (GameObject enemy in GlobalVariables.hostileArray)
        {
            if (Random.Range(0, 100) < 50) {
                foreach (Effect buff in enemy.GetComponent<Character>().buffsAndDebuffs.BuffList)
                {
                    if (buff.numbered)
                        count += buff.getAmount();
                    else
                        count++;
                }
                foreach (Effect buff in enemy.GetComponent<Character>().buffsAndDebuffs.BurnList)
                {
                    if (buff.numbered && buff.getName().Equals("heal"))
                        count += buff.getAmount();
                    else
                        count++;
                }
                enemy.GetComponent<Character>().buffsAndDebuffs.cleanseBuffs();
                enemy.GetComponent<Character>().buffsAndDebuffs.ImageArray();
            }
        }
        yield return new WaitForSeconds(1.27f);
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
            enemy.GetComponent<Character>().SendMessage("PlagueDoctorSpecial2a", character);
            for (int i = 0; i < count; i++) 
                enemy.GetComponent<Character>().SendMessage("PlagueDoctorSpecial2b", character);
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
                enemy.GetComponent<Character>().SendMessage("PlagueDoctorSpecial2a", character);
                for (int i = 0; i < count; i++)
                    enemy.GetComponent<Character>().SendMessage("PlagueDoctorSpecial2b", character);
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
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Move To Enemy / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
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
            if (speed < 360)
                speed += 40;
            yield return new WaitForSeconds(.03f);
        }
        switch (ability)
        {
            case "Basic":
                GetComponent<Animator>().CrossFade("Base Layer.Basic", .3f);
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
        if (!character.attackingOutOfTurn) { GetComponent<Animator>().CrossFade("Base Layer." + character.currentIdle, .5f); } else { GetComponent<Animator>().Play("Base Layer." + character.currentIdle); }
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
    
}
