using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SuperionEnemy : ParentEnemy
{

    void Start()
    {
        character.nameOfChar = "Superion";
        character.Gender = "male";
        //tags
        character.tags.Add("Leader");
        character.tags.Add("Attacker");
        character.tags.Add("Support");
        character.tags.Add("BlackLegion");
        character.tags.Add("Metal");
        character.tags.Add("Magic");
        character.mainElement = "Magic";

        character.stunImmunity = true;
        character.turnMeterDownImmunity = true;
        character.abilityBlockImmunity = true;

        character.Armor = true;
        character.physArmor = true; //if you have true armor, just enable both
        character.magArmor = true;
        character.physArmorAmount = .7f; // these values are almost always either .7f or 1, unless their kit is specified not to
        character.magArmorAmount = .7f;

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
        StartCoroutine(establishCooldowns());

        if (character.isLeader)
        {
            GlobalVariables.Leaders[0] = "Superion";
        }
    }
    IEnumerator start()
    {
        if (character.isLeader)
        {
            GlobalVariables.EnemyLeaders[0] = "Superion";
        }
        yield return new WaitForSeconds(.1f);
        // this start() method is used for certain things that have to wait for all characters' stats to finalize before activating, such as leader abilities
        if (character.isLeader)
        {
            foreach (GameObject boy in GlobalVariables.hostileArray)
            {
                boy.GetComponent<Character>().speed += 30;
                boy.GetComponent<Character>().HealthSteal += 40;
                boy.GetComponent<Character>().maxHealth *= 1.5f;
                boy.GetComponent<Character>().health = boy.GetComponent<Character>().maxHealth;
                boy.GetComponent<Character>().maxHealth += boy.GetComponent<Character>().maxProtection;
                boy.GetComponent<Character>().health = boy.GetComponent<Character>().maxHealth;
                boy.GetComponent<Character>().protection = 0;
                boy.GetComponent<Character>().maxProtection = 0;
                boy.GetComponent<Character>().Armor = false;
            }
            character.speed += 30;
            character.HealthSteal += 40;
            character.maxHealth *= 1.5f;
            character.health = character.maxHealth;
            character.maxHealth += character.maxProtection;
            character.health = character.maxHealth;
            character.protection = 0;
            character.maxProtection = 0;
            character.Armor = false;
        }
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
            GlobalVariables.addToWhomstJustUsedBasic(this.GetComponent<Character>());
        }
        //look at enemy
        StartCoroutine(MoveToEnemy(1.25f, "Basic", 4));
        StartCoroutine(LookAtEnemy());
    }
    IEnumerator basic()
    {
        gameObject.tag = "Enemy";
        yield return new WaitForSeconds(.1f);
        yield return new WaitForSeconds(.5f);
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
            int count = 0;
            foreach (Effect effect in enemy.GetComponent<Character>().buffsAndDebuffs.DebuffList)
            {
                if (effect.numbered)
                    count += effect.getAmount();
                else
                    count++;
            }
            foreach (Effect effect in enemy.GetComponent<Character>().buffsAndDebuffs.BurnList)
            {
                if (!effect.getName().Equals("heal"))
                    count += effect.getAmount();
            }
            enemy.GetComponent<Character>().buffsAndDebuffs.cleanseBuffs();
            enemy.GetComponent<Character>().buffsAndDebuffs.cleanseDebuffs();
            enemy.GetComponent<Character>().buffsAndDebuffs.ImageArray();
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
            enemy.GetComponent<Character>().SendMessage("SimplePhysical", this.GetComponent<Character>());
            character.buffsAndDebuffs.DarkEnergy.burnAdd(count);
            if (!character.buffsAndDebuffs.BlueBuffList.Contains(character.buffsAndDebuffs.DarkEnergy) && count > 0)
                character.buffsAndDebuffs.SendMessage("darkEnergyActivate");
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

    GetComponentInChildren<Animator>().Play("Special1");
    yield return new WaitForSeconds(.71f);
        StartCoroutine(projectile(enemy, "Special1"));
    character.buffsAndDebuffs.ImageArray();
    yield return new WaitForSeconds(.71f);
    GetComponentInChildren<Animator>().CrossFade("Base Layer." + character.currentIdle, .25f);
    GlobalVariables.TakingTurn = false;
    character.buffsAndDebuffs.SendMessage("EndOfTurn");
    character.turnUpdate = true;
}
    public GameObject hand;
    public GameObject arrow;
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
        // if enemy dodges
        bool enemyHealth = false;
        if (enemy.GetComponent<Character>().health == enemy.GetComponent<Character>().maxHealth)
        {
            enemyHealth = true;
        }
        if ((Random.Range(0, 100) < enemy.GetComponent<Character>().dodge - character.accuracy + 10) || enemy.GetComponent<Character>().Agility || character.Blind || enemy.GetComponent<Character>().TrueSight)
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
        damageNumbers("White", "player");
        GlobalVariables.addToWhomstJustBlocked(enemy.GetComponent<Character>());
    }
    else
    {
        // if character.criticalhit or not
        if (!enemy.GetComponent<Character>().critImmunity && ((Random.Range(0, 100) < (character.CriticalChance - enemy.GetComponent<Character>().CriticalAvoidance)) || character.Focus))
        {
            character.DamageAmount = (int)((dmg + Random.Range(0, 26)) * 2.75f * character.CritMultiplier);

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
            character.DamageAmount = (int)((dmg + Random.Range(0, 26)) * 2.75f);

        }
            //elemental Damage
            character.elementDamage(character.mainElement, enemy.GetComponent<Character>().tags);
            enemy.GetComponent<Character>().SendMessage("Superion" + ability, this.GetComponent<Character>());
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
    enemy.GetComponent<Character>().SendMessage("abilityUpdate");
        if (enemyHealth)
        {
            foreach (GameObject enemy3 in GlobalVariables.friendlyArray)
            {
                enemy.tag = "Enemy";
                enemy3.tag = "EnemySelected";
                enemy = enemy3;
                yield return new WaitForSeconds(.01f);

                if ((Random.Range(0, 100) < enemy.GetComponent<Character>().dodge - character.accuracy + 10) || enemy.GetComponent<Character>().Agility || character.Blind || enemy.GetComponent<Character>().TrueSight)
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
                    damageNumbers("White", "player");
                    GlobalVariables.addToWhomstJustBlocked(enemy.GetComponent<Character>()); //tells globalvariables that the enemy just dodged
                }
                else
                {
                    // if character.criticalhit or not
                    if (!enemy.GetComponent<Character>().critImmunity && ((Random.Range(0, 100) < (character.CriticalChance - enemy.GetComponent<Character>().CriticalAvoidance)) || character.Focus))
                    {
                        character.DamageAmount = (int)((dmg + Random.Range(0, 26)) * 2.75f * character.CritMultiplier);

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
                        character.DamageAmount = (int)((dmg + Random.Range(0, 26)) * 2.75f);

                    }
                    //elemental Damage
                    character.elementDamage(character.mainElement, enemy.GetComponent<Character>().tags);
                    enemy.GetComponent<Character>().SendMessage("SuperionSpecial1", this.GetComponent<Character>());
                    // countering
                    if (Random.Range(0, 100) < enemy.GetComponent<Character>().counter && !character.attackingOutOfTurn && character.buffsAndDebuffs.BuffList.Contains(character.buffsAndDebuffs.Stealth))
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
                enemy.GetComponent<Character>().SendMessage("abilityUpdate");
            }
        }
        GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.DarkEnergy.burnAdd(2);
        if (!GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.BlueBuffList.Contains(character.buffsAndDebuffs.DarkEnergy))
            GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.SendMessage("darkEnergyActivate");
        foreach (GameObject ally in GlobalVariables.hostileArray)
        {
            ally.GetComponent<Character>().buffsAndDebuffs.DarkEnergy.burnAdd(2);
            if (!ally.GetComponent<Character>().buffsAndDebuffs.BlueBuffList.Contains(character.buffsAndDebuffs.DarkEnergy))
                ally.GetComponent<Character>().buffsAndDebuffs.SendMessage("darkEnergyActivate");
            ally.GetComponent<Character>().buffsAndDebuffs.ImageArray();
        }
        if (!character.attackingOutOfTurn)
            cooldownChange(-1);
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / /  Special Abilities / / / / / / / / / / / / / / / / / / / / / / / / / / /
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
    yield return new WaitForSeconds(.8f);
        if ((Random.Range(0, 100) < enemy.GetComponent<Character>().dodge - character.accuracy + 10) || enemy.GetComponent<Character>().Agility || character.Blind || enemy.GetComponent<Character>().TrueSight)
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
            enemy.GetComponent<Character>().SendMessage("SuperionSpecial2", this.GetComponent<Character>());
            // countering
            if (Random.Range(0, 100) < enemy.GetComponent<Character>().counter && !character.attackingOutOfTurn && character.buffsAndDebuffs.BuffList.Contains(character.buffsAndDebuffs.Stealth))
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
        enemy.GetComponent<Character>().SendMessage("abilityUpdate");
        foreach (GameObject enemy2 in GlobalVariables.friendlyArray)
        {
            enemy.tag = "Enemy";
            enemy2.tag = "EnemySelected";
            enemy = enemy2;
            yield return new WaitForSeconds(.01f);

            if ((Random.Range(0, 100) < enemy.GetComponent<Character>().dodge - character.accuracy + 10) || enemy.GetComponent<Character>().Agility || character.Blind || enemy.GetComponent<Character>().TrueSight)
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
                enemy.GetComponent<Character>().SendMessage("SuperionSpecial2", this.GetComponent<Character>());
                // countering
                if (Random.Range(0, 100) < enemy.GetComponent<Character>().counter && !character.attackingOutOfTurn && character.buffsAndDebuffs.BuffList.Contains(character.buffsAndDebuffs.Stealth))
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
            enemy.GetComponent<Character>().SendMessage("abilityUpdate");
        }
        character.buffsAndDebuffs.ImageArray();
    yield return new WaitForSeconds(.8f);
    GetComponentInChildren<Animator>().CrossFade("Base Layer." + character.currentIdle, .25f);
        if (!character.attackingOutOfTurn)
            cooldownChange(-1);
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
        else if(character.abilities.CurrentAbilityCooldowns[1] == 0 && !character.abilityBlock)
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
