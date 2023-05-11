using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TempPikeEnemy : ParentEnemy
{
    int TempPikeBasic5050 = 1;
    //bool alreadyCountering = false;
    public ParticleSystem swordSparks;
    public TrailRenderer swordTrail;
    bool canRevive = true;
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
        GlobalVariables.OnGotHit += MethodOnDie;

        var swordSparksEmission = swordSparks.emission;
        swordSparksEmission.enabled = false;
        swordTrail.emitting = false;

        character.nameOfChar = "TempPike";
        character.Gender = "male";
        character.tags.Add("Tank");
        character.tags.Add("Attacker");
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
        character.Potency += (int)(character.level * .15f);
        character.Tenacity += (int)(character.level * .75f);
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
   
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Basic Ability / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void Basic(GameObject enemy2)
    {
        enemy = enemy2;
        gameObject.tag = "Enemy";
        enemy.tag = "PlayerSelected";
        TempPikeBasic5050 = 1;
        if (Random.Range(0, 100) < 50)
            TempPikeBasic5050++;
        if (!character.attackingOutOfTurn)
        {
            character.turnMeter = 0;
            GlobalVariables.addToWhomstJustUsedBasic(character);
        }
        //look at enemy
        StartCoroutine(MoveToEnemy(1f, "Basic", 7));
        StartCoroutine(LookAtEnemy());
    }
    IEnumerator basic()
    {
                gameObject.tag = "Enemy";
                yield return new WaitForSeconds(.1f);
                swordTrail.emitting = true;
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
                    var swordSparksEmission = swordSparks.emission;
                    swordSparksEmission.enabled = true;
            //elemental Damage
            character.elementDamage(character.mainElement, enemy.GetComponent<Character>().tags);
            enemy.GetComponent<Character>().SendMessage("TempPikeBasic", character);
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
                yield return new WaitForSeconds(.5f);
            
        
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
        yield return new WaitForSeconds(.57f);
        //character.audioSource3.clip = GlobalSounds.MaleGrunt2;
        //character.audioSource3.Play();
        yield return new WaitForSeconds(.57f);
        if (!character.buffsAndDebuffs.DebuffList.Contains(character.buffsAndDebuffs.BuffImmunity))
        {
            character.buffsAndDebuffs.DefenseUp.add(3);
            if (!character.buffsAndDebuffs.BuffList.Contains(character.buffsAndDebuffs.DefenseUp))
                character.buffsAndDebuffs.SendMessage("defenseUpActivate");
        }
        character.buffsAndDebuffs.Guard.add(3);
        if (!character.buffsAndDebuffs.BlueBuffList.Contains(character.buffsAndDebuffs.Guard))
            character.buffsAndDebuffs.SendMessage("guardActivate");
        character.buffsAndDebuffs.ImageArray();
        yield return new WaitForSeconds(.4f);
        GetComponent<Animator>().CrossFade("Guard", .1f);
        if (!character.attackingOutOfTurn)
            cooldownChange(-1);
        enemy.GetComponent<Character>().SendMessage("abilityUpdate");
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
        for (int j = 0; j < TempPikeBasic5050; j++)
        {
            GetComponent<Animator>().CrossFade("Base Layer.Basic " + j, .3f);
            StartCoroutine(basic());
            yield return new WaitForSeconds(wait);
            speed = 0;
        }
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
    
    private void MethodOnDie()
    {
        if (GlobalVariables.whomstJustGotHit[0] == this.character && canRevive && character.health <= 0)
        {
            canRevive = false;
            GlobalVariables.addToWhomstJustDied(this.character);
            character.protection = 0;
            character.health = (int)(character.maxHealth * .1f);
            character.turnMeter = 100;
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
