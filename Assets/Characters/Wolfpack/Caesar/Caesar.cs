using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Caesar : ParentCharacter
{
    
    public bool funni = false;
    // Start is called before the first frame update
    void Start()
    {
        GlobalVariables.OnStartTurn += OnCaesarTurn;
        GlobalVariables.OnUsedSpecial += OnSpecialTurn;
        character.nameOfChar = "Caesar";
        character.Gender = "male";
        //tags
        character.tags.Add("Attacker");
        character.tags.Add("Support");
        character.tags.Add("Wolfpack");
        character.tags.Add("Metal");
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
    #region The Update Method
    public override void Update()
    {
        if (!character.dead)
        {
            selectableIndicator.GetComponent<SpriteRenderer>().enabled = GlobalVariables.allySelectable;
            //gaining turn meter and taking turn
            if (!GlobalVariables.TakingTurn && !GlobalVariables.countering)
            {
                if (GlobalVariables.playerArray.Length == 0)
                {
                    character.turnMeter += ((float)character.speed * Time.deltaTime);
                }
                if (character.turnMeter >= 100 && GlobalVariables.playerArray.Length == 0)
                {
                    gameObject.tag = "AllyTurn";
                    GlobalVariables.TakingTurn = true;
                    character.critical = false;
                    if (!character.dead)
                    {
                        character.buffsAndDebuffs.SendMessage("StartOfTurn");
                        character.turnUpdate = false;
                        if (!character.Stun && !character.dead)
                        {
                            if (character.channeling)
                            {
                                funni = true;
                                enemy = GlobalVariables.enemiesSelected[0];
                                character.turnUpdate = true;
                                this.SendMessage("SpecialPunch", enemy);
                            }
                            else
                            {
                                for (int i = 1; i <= character.boutons.Count; i++)
                                {
                                    GameObject instance = Instantiate(blankButton, Buttons.transform);
                                    if (character.abilityBlock)
                                        instance.GetComponent<Button>().abilityBlock = true;
                                    instance.transform.position = Buttons.transform.position + .60f * i * Buttons.transform.right;
                                    instance.GetComponent<Button>().setShit(character.boutons[i - 1]);
                                    buttons.Add(instance);
                                }

                                basicButton.GetComponent<BasicButton>().texture = character.abilities.AbilityIcons[0];
                            }
                        }
                        else
                        {
                            character.turnMeter = 0;
                            GlobalVariables.TakingTurn = false;
                            gameObject.tag = "Ally";
                            character.buffsAndDebuffs.SendMessage("EndOfTurn");
                        }
                    }
                }
                else
                    gameObject.tag = "Ally";
            }

            //calling abilities
            if (BasicButton.activatedBinary == 1 && gameObject.CompareTag("AllyTurn"))
            {
                enemy = GlobalVariables.enemiesSelected[0];
                character.turnUpdate = true;
                this.SendMessage("Basic", enemy);
            }
        }
    }
    #endregion

    // / / / / / / / / / / / / / / / / / / / / / / / / / / Abilities / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void Basic(GameObject enemy2)
    {
        enemy = enemy2;
        gameObject.tag = "Ally";
        if (!character.attackingOutOfTurn)
        {
            character.turnMeter -= 100;
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
        if (!funni)
            StartCoroutine(MoveToEnemy(1.25f, "Basic", 8));
        else
            StartCoroutine(MoveToEnemy(1.4f, "BasicFunni", 14));
        StartCoroutine(LookAtEnemy());
    }
    IEnumerator basic()
    {
        gameObject.tag = "Ally";
        if (!funni)
            yield return new WaitForSeconds(.8f);
        else
        {
            yield return new WaitForSeconds(1f);
            funni = false;
        }
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
            enemy.GetComponent<Character>().SendMessage("CaesarBasic", this.GetComponent<Character>());
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
    public void Special1()
    {
        gameObject.tag = "Ally";
    enemy = GlobalVariables.enemiesSelected[0];
        basicButton.GetComponent<BasicButton>().moveDownAndBack(character.boutons.Count, Buttons);
    character.turnMeter = 0;
        GlobalVariables.addToWhomstJustUsedSpecial(this.GetComponent<Character>());
    StartCoroutine(special1());
    }
IEnumerator special1()
    {
    gameObject.tag = "Ally";
    GetComponentInChildren<Animator>().CrossFade("Special1", .025f);
    yield return new WaitForSeconds(.57f);
    //Do method
    if (!character.turnMeterUpImmunity)
        {
            character.turnMeter += 50;
        }
          character.buffsAndDebuffs.Channeling.add(2);
                if (!character.buffsAndDebuffs.BlueBuffList.Contains(character.buffsAndDebuffs.Channeling))
                    character.buffsAndDebuffs.SendMessage("channelingActivate");
        character.buffsAndDebuffs.ImageArray();
    yield return new WaitForSeconds(1f);
    GetComponentInChildren<Animator>().CrossFade("Base Layer." + character.currentIdle, .25f);
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
    public void SpecialPunch(GameObject enemy2)
    {
        enemy = enemy2;
        gameObject.tag = "Ally";
        basicButton.GetComponent<BasicButton>().moveDownAndBack(character.boutons.Count, Buttons);
        character.buffsAndDebuffs.OffenseUp.add(2);
        if (!character.buffsAndDebuffs.BuffList.Contains(character.buffsAndDebuffs.OffenseUp))
            character.buffsAndDebuffs.SendMessage("offenseUpActivate");
        character.abilityUpdate();
        character.turnMeter = 0;
        GlobalVariables.addToWhomstJustUsedSpecial(this.GetComponent<Character>());
    StartCoroutine(MoveToEnemy(1.25f, "SpecialPunch", 7));
        StartCoroutine(LookAtEnemy());
}
IEnumerator specialPunch()
    {
    gameObject.tag = "Ally";
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
            enemy.GetComponent<Character>().SendMessage("CaesarSpecialPunch", this.GetComponent<Character>());
            character.turnMeter = 97;
            funni = true;
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
public void Special2()
    {
        gameObject.tag = "Ally";
    enemy = GlobalVariables.enemiesSelected[0];
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
        //Do method
        foreach (GameObject boy in GlobalVariables.friendlyArray)
        {
            if (boy.GetComponent<Character>().turnMeterUpImmunity)
            {
                boy.GetComponent<Character>().turnMeter += 85;
                if (boy.GetComponent<Character>().turnMeter > 100)
                    boy.GetComponent<Character>().turnMeter = 100;
            }
            if (!boy.GetComponent<Character>().healingImmunity)
                boy.GetComponent<Character>().health = boy.GetComponent<Character>().maxHealth;
            if (!boy.GetComponent<Character>().buffsAndDebuffs.DebuffList.Contains(boy.GetComponent<Character>().buffsAndDebuffs.BuffImmunity))
            {
                boy.GetComponent<Character>().buffsAndDebuffs.OffenseUp.add(2);
                if (!boy.GetComponent<Character>().buffsAndDebuffs.BuffList.Contains(boy.GetComponent<Character>().buffsAndDebuffs.OffenseUp))
                    boy.GetComponent<Character>().buffsAndDebuffs.SendMessage("offenseUpActivate");
                boy.GetComponent<Character>().buffsAndDebuffs.SpeedUp.add(2);
                if (!boy.GetComponent<Character>().buffsAndDebuffs.BuffList.Contains(boy.GetComponent<Character>().buffsAndDebuffs.SpeedUp))
                    boy.GetComponent<Character>().buffsAndDebuffs.SendMessage("speedUpActivate");
                boy.GetComponent<Character>().buffsAndDebuffs.DefenseUp.add(2);
                if (!boy.GetComponent<Character>().buffsAndDebuffs.BuffList.Contains(boy.GetComponent<Character>().buffsAndDebuffs.DefenseUp))
                    boy.GetComponent<Character>().buffsAndDebuffs.SendMessage("defenseUpActivate");
                boy.GetComponent<Character>().buffsAndDebuffs.ResilienceUp.add(2);
                if (!boy.GetComponent<Character>().buffsAndDebuffs.BuffList.Contains(boy.GetComponent<Character>().buffsAndDebuffs.ResilienceUp))
                    boy.GetComponent<Character>().buffsAndDebuffs.SendMessage("resilienceUpActivate");
                boy.GetComponent<Character>().buffsAndDebuffs.CritChanceUp.add(2);
                if (!boy.GetComponent<Character>().buffsAndDebuffs.BuffList.Contains(boy.GetComponent<Character>().buffsAndDebuffs.CritChanceUp))
                    boy.GetComponent<Character>().buffsAndDebuffs.SendMessage("critChanceUpActivate");
                boy.GetComponent<Character>().buffsAndDebuffs.Focus.add(2);
                if (!boy.GetComponent<Character>().buffsAndDebuffs.BuffList.Contains(boy.GetComponent<Character>().buffsAndDebuffs.Focus))
                    boy.GetComponent<Character>().buffsAndDebuffs.SendMessage("focusActivate");
            }
            boy.GetComponent<Character>().abilityUpdate();
            boy.GetComponent<Character>().SendMessage("AttackOutOfTurn", 1f);
        }
        character.buffsAndDebuffs.ImageArray();
    yield return new WaitForSeconds(.8f);
    GetComponentInChildren<Animator>().CrossFade("Base Layer." + character.currentIdle, .25f);
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
    public override IEnumerator AttackOutOfTurn(float DamageMultiplier)
    {
        character.actionQueue++;
        if (!character.attackingOutOfTurn)
        {
            yield return new WaitForSeconds(.5f);
            if (character.canAttackOutOfTurn && !character.dead)
            {
                GlobalVariables.numCounters++; dmg = (int)(dmg * DamageMultiplier);
                character.attackingOutOfTurn = true; enemy = GlobalVariables.enemiesSelected[0];

                float counter2 = enemy.GetComponent<Character>().counter;
                while (character.actionQueue > 0 && enemy.GetComponent<Character>().health > 0)
                {
                    GlobalVariables.assisters++;
                    GlobalVariables.addToWhomstJustAttackedOutOfTurn(this.GetComponent<Character>());
                    if (!character.channeling)
                    {
                        SendMessage("Basic", enemy);
                    }
                    else
                    {
                        SendMessage("SpecialPunch", enemy);
                        character.buffsAndDebuffs.Channeling.setAmount(0);
                        character.abilityUpdate();
                    }
                    yield return new WaitForSeconds(1.5f);
                    GlobalVariables.assisters--;
                    yield return new WaitForSeconds(.6f);
                    character.actionQueue--;
                }
                enemy.GetComponent<Character>().counter = counter2;
                GlobalVariables.numCounters--; dmg = (int)(dmg / DamageMultiplier);
                character.attackingOutOfTurn = false;
            }
            else character.actionQueue--;

        }
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
            case "SpecialPunch":
                GetComponentInChildren<Animator>().CrossFade("Base Layer.SpecialPunch", .3f);
                StartCoroutine(specialPunch());
                break;
            case "BasicFunni":
                GetComponentInChildren<Animator>().CrossFade("Base Layer.BasicSpecial", .3f);
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

    public void OnCaesarTurn()
    {
        if (GlobalVariables.whomstJustStartedTurn[0] == character)
        {
            if (character.health < character.maxHealth && !character.healingImmunity)
            {
                GlobalVariables.DamageNumberGreen.GetComponent<TextMeshProUGUI>().text = "" + (int)(character.maxHealth * .20f);
                character.health += (int)(character.maxHealth * .20f);
                if (character.health > character.maxHealth)
                    character.health = character.maxHealth;
                damageNumbers("Green", "me");
                if (character.health < character.maxHealth / 4)
                    character.healthArray[0].color = Color.red;
                else if (character.health < character.maxHealth / 2)
                    character.healthArray[0].color = Color.yellow;
                else
                    character.healthArray[0].color = Color.green;
            }
            List<Character> WolfpackList = new List<Character>();
            if (GlobalVariables.friendlyArray.Length > 0)
            {
                for (int i = 0; i < GlobalVariables.friendlyArray.Length; i++)
                {
                    if (GlobalVariables.friendlyArray[i].GetComponent<Character>().tags.Contains("Wolfpack"))
                    {
                        WolfpackList.Add(GlobalVariables.friendlyArray[i].GetComponent<Character>());
                    }
                }
            }
            else
                WolfpackList.Add(null);
            Character target = null;
            if (WolfpackList.Count > 0)
                target = WolfpackList[Random.Range(0, WolfpackList.Count - 1)];
            if (target != null)
            {
                target.buffsAndDebuffs.OffenseUp.burnAdd(2);
                if (!target.buffsAndDebuffs.BuffList.Contains(target.GetComponent<Character>().buffsAndDebuffs.OffenseUp))
                    target.buffsAndDebuffs.SendMessage("offenseUpActivate");
                target.buffsAndDebuffs.ImageArray();
            }
        }
    }
    public void OnSpecialTurn()
    {
        if (GlobalVariables.whomstJustUsedSpecial[0].buffsAndDebuffs.BuffList.Contains(GlobalVariables.whomstJustUsedSpecial[0].buffsAndDebuffs.OffenseUp) && GlobalVariables.whomstJustUsedSpecial[0].tags.Contains("Wolfpack") && character.boutons[1].currentCooldown > 0 && !GlobalVariables.whomstJustUsedSpecial[0].name.Contains("Enemy"))
        {
            character.boutons[1].currentCooldown--;
        }
    }
}
