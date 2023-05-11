using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TempChampEnemy : ParentEnemy
{
    
    public GameObject hand;
    public GameObject arrow;

    public ParticleSystem swordSparks;
    public TrailRenderer swordTrail;

    public bool statsDoubled;
    int statsDoubledCount;
    // Start is called before the first frame update
    void Start()
    {
        GlobalVariables.OnStartTurn += OnStartTurn;
        GlobalVariables.OnGotCrit += OnGotCrit;

        statsDoubled = false;
        statsDoubledCount = 0;
        /*
        if (character.isLeader)
        {
            character.buffsAndDebuffs.Stealth.add(1);
            if (!character.buffsAndDebuffs.BuffList.Contains(character.buffsAndDebuffs.Stealth))
                character.buffsAndDebuffs.SendMessage("stealthActivate");
            character.abilityUpdate();
        }    
        */
        var swordSparksEmission = swordSparks.emission;
        swordSparksEmission.enabled = false;
        swordTrail.emitting = false;

        character.nameOfChar = "TempChamp";
        character.Gender = "male";
        character.tags.Add("Attacker");
        character.tags.Add("KingdomCrasher");
        character.tags.Add("Crusader");
        character.tags.Add("Metal");
        character.mainElement = "Metal";
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
        character.physArmor = true; //if you have true armor, just enable both
        character.physArmorAmount = .7f;
        character.magArmorAmount = 1;
        character.health = (int)(character.health * ((character.level - 1) * 8 / 79f + 1) * ((Mathf.Pow((float)character.star - 1, 2) / 4f) + 1));
        character.maxHealth = character.health;
        character.protection = (int)(character.protection * ((character.level - 1) * 8 / 79f + 1) * ((Mathf.Pow((float)character.star - 1, 2) / 4f) + 1));
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
        yield return new WaitForSeconds(.01f);
        //character.blockSound = GlobalSounds.Anvil1;
        //character.parrySound = GlobalSounds.Dink1;
    }
    
    // Update is called once per frame
    public override void Update()
    {
        if (!character.dead)
        {
            if (GlobalVariables.enemiesSelected.Length == 0 && (!character.untargetable || GlobalVariables.hostileArray.Length == 1) && (character.Taunt || (GlobalVariables.EnemyTaunters == 0 && (!character.Stealth || GlobalVariables.allStealthed))))
                gameObject.tag = "EnemySelected";
            if (GlobalVariables.enemiesSelected.Length != 0)
            {
                if (GlobalVariables.enemiesSelected[0] != this.gameObject)
                    gameObject.tag = "Enemy";
            }
            //if they're the only one taunting
            if (GlobalVariables.playerArray.Length != 0)
            {
                if (GlobalVariables.EnemyTaunters == 1 && character.Taunt && !GlobalVariables.playerArray[0].GetComponent<Character>().TauntIgnore)
                {
                    if (GlobalVariables.enemiesSelected.Length > 0)
                    {
                        for (int i = 0; i < GlobalVariables.enemiesSelected.Length; i++)
                        {
                            GlobalVariables.enemiesSelected[i].tag = "Enemy";
                        }
                        GlobalVariables.enemiesSelected[0] = this.gameObject;
                    }
                    gameObject.tag = "EnemySelected";
                }
            }
            //turn meter
            if (!GlobalVariables.TakingTurn && !GlobalVariables.countering)
            {
                if (!gameObject.CompareTag("EnemySelected"))
                {
                    if (GlobalVariables.enemyArray.Length == 0)
                        character.turnMeter += ((float)character.speed * Time.deltaTime);
                    if (character.turnMeter >= 100 && GlobalVariables.enemyArray.Length == 0)
                    {
                        gameObject.tag = "EnemyTurn";
                        GlobalVariables.TakingTurn = true;
                        character.critical = false;
                        if (!character.dead)
                        {
                            character.buffsAndDebuffs.SendMessage("StartOfTurn");
                            if (statsDoubled)
                            {
                                if (statsDoubledCount == 0)
                                {
                                    character.maxHealth /= 2;
                                    if (character.health > character.maxHealth)
                                        character.health = character.maxHealth;
                                    character.offense /= 2;
                                    character.defense /= 2;
                                    statsDoubled = false;
                                }
                                else
                                    statsDoubledCount--;
                            }
                            character.turnUpdate = false;
                            if (character.Stun)
                            {
                                character.turnMeter = 0;
                                GlobalVariables.TakingTurn = false;
                                gameObject.tag = "Enemy";
                                character.buffsAndDebuffs.SendMessage("EndOfTurn");

                            }
                        }
                    }
                    else
                        gameObject.tag = "Enemy";
                }
                else
                {
                    if (GlobalVariables.enemyArray.Length == 0)
                        character.turnMeter += ((float)character.speed * Time.deltaTime);
                    if (character.turnMeter >= 100 && GlobalVariables.enemyArray.Length == 0)
                    {
                        gameObject.tag = "EnemyTurn";
                        GlobalVariables.TakingTurn = true;
                        if (!character.dead)
                        {
                            character.buffsAndDebuffs.SendMessage("StartOfTurn");
                            character.turnUpdate = false;
                            if (!character.Stun && !character.dead)
                            {
                                StartCoroutine(EnemySelectedWait());
                            }
                            else
                            {
                                character.turnMeter = 0;
                                GlobalVariables.TakingTurn = false;
                                gameObject.tag = "EnemySelected";
                                character.buffsAndDebuffs.SendMessage("EndOfTurn");
                            }
                        }
                    }
                    else
                        gameObject.tag = "EnemySelected";
                }
            }


            //taking turn
            if (gameObject.CompareTag("EnemyTurn"))
            {
                List<GameObject> array = new List<GameObject>();
                if (GlobalVariables.playersSelected.Length > 0)
                    GlobalVariables.playersSelected[0].tag = "Ally";
                GlobalVariables.friendlyArray = GameObject.FindGameObjectsWithTag("Ally");
                foreach (GameObject guy in GlobalVariables.friendlyArray)
                {
                    if (GlobalVariables.AllyTaunters > 0)
                    {
                        if ((guy.GetComponent<Character>().Taunt || (!guy.GetComponent<Character>().Stealth && character.TauntIgnore) || (guy.GetComponent<Character>().Stealth && character.TauntIgnore && character.StealthIgnore)) && guy.GetComponent<Character>().health > 0 && !guy.GetComponent<Character>().untargetable)
                        {
                            array.Add(guy);
                        }
                    }
                    else
                    {
                        if (((guy.GetComponent<Character>().Stealth && character.StealthIgnore) || !guy.GetComponent<Character>().Stealth || GlobalVariables.AllyStealthers == GlobalVariables.friendlyArray.Length) && guy.GetComponent<Character>().health > 0 && !guy.GetComponent<Character>().untargetable)
                        {
                            array.Add(guy);
                        }
                    }
                }
                if (character.health > 0)
                    AbilityAI(array);

            }
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
            GlobalVariables.addToWhomstJustUsedBasic(character);
        }
        //look at enemy
        StartCoroutine(MoveToEnemy(1.2f, "Basic", 6));
        StartCoroutine(LookAtEnemy());
    }
    IEnumerator basic()
    {
        var swordSparksEmission = swordSparks.emission;
        swordSparksEmission.enabled = false;
        gameObject.tag = "Ally";
        yield return new WaitForSeconds(.1f);
        swordTrail.emitting = true;
        yield return new WaitForSeconds(.5f);
        GlobalSounds.playSlash(character.audioSource1);
        swordSparksEmission.enabled = true;
        yield return new WaitForSeconds(.01f);
        swordSparksEmission.enabled = false;
        yield return new WaitForSeconds(.5f);

        // if enemy dodges
        swordTrail.emitting = false;
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
            GlobalVariables.addToWhomstJustParried(enemy.GetComponent<Character>());
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
            // if critical hit or not
            if (!enemy.GetComponent<Character>().critImmunity && ((Random.Range(0, 100) < 20) || character.Focus))
            {
                character.DamageAmount = (int)((dmg + Random.Range(0, 26)) * 2.5f * 4);
                 
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
            swordSparksEmission.enabled = true;
            //elemental Damage
            character.elementDamage(character.mainElement, enemy.GetComponent<Character>().tags);
            enemy.GetComponent<Character>().SendMessage("TempChampBasic", character);
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
            swordSparksEmission.enabled = false;
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
        GetComponent<Animator>().Play("Special1a");
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(projectile(enemy));
        GetComponent<Animator>().Play("Special1b");
        character.buffsAndDebuffs.ImageArray();
        yield return new WaitForSeconds(1.125f);
        GetComponent<Animator>().CrossFade("Base Layer." + character.currentIdle, .25f);
        GlobalVariables.TakingTurn = false;
        character.buffsAndDebuffs.SendMessage("EndOfTurn");
        character.turnUpdate = true;
    }
    IEnumerator projectile(GameObject enemy2)
    {
        enemy = enemy2;
        GameObject instance = Instantiate(arrow, hand.transform);
        instance.transform.position = hand.transform.position;
        instance.transform.parent = null;
        instance.transform.rotation = Quaternion.LookRotation(new Vector3(enemy.transform.position.x - transform.position.x, 0, enemy.transform.position.z - transform.position.z), Vector3.up);
        instance.transform.localScale *= 1.4f;
        int speed = 360;
        Vector3 enemyPos = new Vector3(enemy.GetComponent<Character>().initial.x, instance.transform.position.y, enemy.GetComponent<Character>().initial.z);
        Vector3 lTargetDir = enemyPos - instance.transform.position;
        lTargetDir.y = 0.0f;
        while (Vector3.Distance(enemyPos, instance.transform.position) > 2)
        {
            instance.transform.position = Vector3.MoveTowards(instance.transform.position, enemyPos, speed * Time.deltaTime);
            yield return new WaitForSeconds(.03f);
        }
        Destroy(instance);


        foreach (GameObject ally in GlobalVariables.hostileArray)
        {
            if (ally.GetComponent<Character>().tags.Contains("KingdomCrasher"))
            {
                if (!character.turnMeterUpImmunity)
                    character.turnMeter += 20;
            }
        }


        // if enemy dodges
        swordTrail.emitting = false;
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
            enemy.GetComponent<Character>().SendMessage("TempChampBasic", character);
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
        if (!character.attackingOutOfTurn)
            cooldownChange(-1);
    }
    public void Special2()
    {
        enemy = GlobalVariables.enemiesSelected[0];
        gameObject.tag = "Enemy";
        character.turnMeter = 0;
        GlobalVariables.addToWhomstJustUsedSpecial(character);
        StartCoroutine(special2(enemy));
    }
    IEnumerator special2(GameObject enemy2)
    {
        enemy = enemy2;
        gameObject.tag = "Enemy";
        GetComponent<Animator>().Play("Special2a");
        yield return new WaitForSeconds(1.6f);
        character.maxHealth *= 2;
        if (!character.healingImmunity)
        {
            GlobalVariables.DamageNumberGreen.GetComponent<TextMeshProUGUI>().text = "" + (int)(character.maxHealth - character.health);
            character.health = character.maxHealth;
            damageNumbers("Green", "me");
            character.healthArray[0].color = Color.green;
        }
        character.offense *= 2;
        character.defense *= 2;
        statsDoubled = true;
        statsDoubledCount = 3;
        GetComponent<Animator>().Play("Special2b");
        character.buffsAndDebuffs.ImageArray();
        yield return new WaitForSeconds(1.125f);
        GetComponent<Animator>().CrossFade("Base Layer." + character.currentIdle, .25f);
        GlobalVariables.TakingTurn = false;
        character.buffsAndDebuffs.SendMessage("EndOfTurn");
        character.turnUpdate = true;
    }

    
    // / / / / / / / / / / / / / / / / / / / / / / / / / / You Assisting / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public override IEnumerator AttackOutOfTurn(float DamageMultiplier)
    {
        character.actionQueue++;
        if (!character.attackingOutOfTurn)
        {
            yield return new WaitForSeconds(.5f);
            if (character.canAttackOutOfTurn && !character.dead)
            {
                GlobalVariables.numCounters++; dmg = (int)(dmg * DamageMultiplier); if (GlobalVariables.Leaders[0].Equals("Shaubeny")) character.HealthSteal += 70;
                character.attackingOutOfTurn = true; if (GlobalVariables.playersSelected[0] != null) enemy = GlobalVariables.playersSelected[0]; else yield break;
              
                float counter = enemy.GetComponent<Character>().counter; enemy.GetComponent<Character>().counter = 0;
                while (character.actionQueue > 0 && enemy.GetComponent<Character>().health > 0)
                {
                    GlobalVariables.assisters++;
                    GlobalVariables.addToWhomstJustAttackedOutOfTurn(character);
                    SendMessage("Basic", enemy);
                    yield return new WaitForSeconds(1.5f);
                    GlobalVariables.assisters--;
                    yield return new WaitForSeconds(.6f);
                    character.actionQueue--;
                }
                enemy.GetComponent<Character>().counter = counter;
                GlobalVariables.numCounters--; dmg = (int)(dmg / DamageMultiplier); if (GlobalVariables.Leaders[0].Equals("Shaubeny")) character.HealthSteal -= 70;
                character.attackingOutOfTurn = false; } else character.actionQueue--;
            
        }
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
    

    private void OnStartTurn()
    {
        if (GlobalVariables.whomstJustStartedTurn[0].tags.Contains("KingdomCrasher") && GlobalVariables.whomstJustStartedTurn[0].name.Contains("Enemy") && Random.Range(0, 100) < 50)
        {
            if (!GlobalVariables.whomstJustStartedTurn[0].buffsAndDebuffs.DebuffList.Contains(GlobalVariables.whomstJustStartedTurn[0].buffsAndDebuffs.BuffImmunity))
            {
                GlobalVariables.whomstJustStartedTurn[0].buffsAndDebuffs.Retribution.add(3);
                if (!GlobalVariables.whomstJustStartedTurn[0].buffsAndDebuffs.BuffList.Contains(GlobalVariables.whomstJustStartedTurn[0].buffsAndDebuffs.Retribution))
                    GlobalVariables.whomstJustStartedTurn[0].buffsAndDebuffs.SendMessage("retributionActivate");
            }
        }
    }

    private void OnGotCrit()
    {
        if (GlobalVariables.whomstJustGotCrit[0] == character)
        {
            if (!character.attackingOutOfTurn)
                cooldownChange(-1);
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
        else if (character.abilities.CurrentAbilityCooldowns[1] == 0 && !character.abilityBlock)
        {
            Special2();
            character.abilities.CurrentAbilityCooldowns[1] = character.abilities.AbilityCooldowns[1];
        }
        else if (Random.Range(0, 100) < 60 && !character.abilityBlock)
        {
            Special1(currentEnemy);
        }
        else
        {
            Basic(currentEnemy);
        }
    }
}
