using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SummonerEnemy : ParentEnemy
{
    readonly string Ability;
    readonly bool physicalAttack;
    readonly int DamageAmount;
    public int orderNumber;

    public GameObject hand;
    public GameObject arrow;
    public GameObject CorruptCultist;
    public GameObject summonSpawn;
    // Start is called before the first frame update
    void Start()
    {
        GlobalVariables.OnGainedDebuff += MethodOnCorruption;

        character.nameOfChar = "Summoner";
        character.Gender = "female";
        character.tags.Add("Cultist");
        character.tags.Add("Support");
        character.tags.Add("Attacker");
        character.tags.Add("Nightmare");
        character.tags.Add("Magic");
        character.tags.Add("Summoner");
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
        StartCoroutine(basic());
    }
    IEnumerator basic()
    {
        gameObject.tag = "Enemy";
        GetComponent<Animator>().CrossFade("Basic", .3f);
        List<Character> CultistList = new List<Character>();
        if (GlobalVariables.hostileArray.Length > 0)
        {
            for (int i = 0; i < GlobalVariables.hostileArray.Length; i++)
            {
                if (GlobalVariables.hostileArray[i].GetComponent<Character>().canAttackOutOfTurn && GlobalVariables.hostileArray[i].GetComponent<Character>().tags.Contains("Cultist") && GlobalVariables.hostileArray[i].GetComponent<Character>() != this)
                    CultistList.Add(GlobalVariables.hostileArray[i].GetComponent<Character>());
            }
        }
        else
            CultistList.Add(null);
        Character target = CultistList[Random.Range(0, CultistList.Count - 1)];
        if (target != null && !character.attackingOutOfTurn)
        {
            target.SendMessage("AttackOutOfTurn", 1.5f);
            if (enemy.GetComponent<Character>().buffsAndDebuffs.DebuffList.Contains(enemy.GetComponent<Character>().buffsAndDebuffs.Corruption))
                target.buffsAndDebuffs.cleanseDebuffs();
        }
        else
        {
            yield return new WaitForSeconds(1.01f);
            StartCoroutine(projectile(enemy, "Basic"));
        }
        character.buffsAndDebuffs.ImageArray();
        yield return new WaitForSeconds(.71f);
        GetComponent<Animator>().CrossFade("Base Layer." + character.currentIdle, .25f);
        GlobalVariables.TakingTurn = false;
        character.buffsAndDebuffs.SendMessage("EndOfTurn");
        character.turnUpdate = true;
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / /  Special Abilities / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void Special2(GameObject enemy2)
    {
        enemy = enemy2;
        gameObject.tag = "Enemy";
        character.turnMeter = 0;
        GlobalVariables.addToWhomstJustUsedSpecial(character);
        StartCoroutine(special2());
    }
    IEnumerator special2()
    {
        gameObject.tag = "Enemy";
        GetComponent<Animator>().CrossFade("Special2", .025f);
        yield return new WaitForSeconds(.57f);
        bool armuk = false;
        //Character corruptAssBussyBitch = null;
        for (int i = 0; i < GlobalVariables.hostileArray.Length; i++)
        {
            if (GlobalVariables.hostileArray[i].GetComponent<Character>().nameOfChar.Equals("CorruptCultist"))
            {
                armuk = true;
                //corruptAssBussyBitch = GlobalVariables.hostileArray[i].GetComponent<Character>();
            }
        }
        if (armuk)
        {
            float equalize = 0;
            int count = 0;
            foreach (GameObject boy in GlobalVariables.hostileArray)
            {
                if ((boy.GetComponent<Character>().tags.Contains("Cultist") || boy.GetComponent<Character>().tags.Contains("Risen")) && !boy.GetComponent<Character>().dead)
                {
                    count++;
                    equalize += (boy.GetComponent<Character>().health / boy.GetComponent<Character>().maxHealth);
                }
            }
            foreach (GameObject boy in GlobalVariables.hostileArray)
            {
                if ((boy.GetComponent<Character>().tags.Contains("Cultist") || boy.GetComponent<Character>().tags.Contains("Risen")) && !boy.GetComponent<Character>().dead)
                {
                    boy.GetComponent<Character>().health = boy.GetComponent<Character>().maxHealth * (equalize / count);
                    boy.GetComponent<Character>().abilityUpdate();
                }
            }
        }
        else if(!GlobalVariables.SummonSlot)
        {
            GlobalVariables.SummonSlot = true;
            GameObject instance = Instantiate(CorruptCultist, summonSpawn.transform);
            instance.GetComponent<Character>().star = character.star;
            instance.GetComponent<Character>().level = character.level;
            instance.transform.SetPositionAndRotation(summonSpawn.transform.position, summonSpawn.transform.rotation);
            instance.transform.localScale = new Vector3(452.919f, 452.919f, 452.919f);
            instance.GetComponent<BuffsDebuffs>().globalTextures = GetComponent<BuffsDebuffs>().globalTextures;
            instance.GetComponent<BuffsDebuffs>().healthAndArmor.GetComponent<RotateToCam>().Camera = GetComponent<BuffsDebuffs>().healthAndArmor.GetComponent<RotateToCam>().Camera;
        }
        yield return new WaitForSeconds(.8f);
        if (!character.attackingOutOfTurn)
            cooldownChange(-1);
        enemy.GetComponent<Character>().SendMessage("abilityUpdate");
        GlobalVariables.TakingTurn = false;
        character.buffsAndDebuffs.SendMessage("EndOfTurn");
        character.turnUpdate = true;
    }
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
        GetComponent<Animator>().Play("Special1");
        yield return new WaitForSeconds(.71f);
        bool armuk = false;
        Character corruptAssBussyBitch = null;
        for (int i = 0; i < GlobalVariables.hostileArray.Length; i++)
        {
            if (GlobalVariables.hostileArray[i].GetComponent<Character>().nameOfChar.Equals("CorruptCultist"))
            {
                armuk = true;
                corruptAssBussyBitch = GlobalVariables.hostileArray[i].GetComponent<Character>();
            }
        }
        if (armuk)
            corruptAssBussyBitch.SendMessage("AttackOutOfTurn", 1f);
        StartCoroutine(projectile(enemy, "Special1"));
        character.buffsAndDebuffs.ImageArray();
        yield return new WaitForSeconds(.71f);
        GetComponent<Animator>().CrossFade("Base Layer." + character.currentIdle, .25f);
        GlobalVariables.TakingTurn = false;
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
            damageNumbers("White", "player");
            GlobalVariables.addToWhomstJustBlocked(enemy.GetComponent<Character>());
        }
        else
        {
            // if critical hit or not
            if (!enemy.GetComponent<Character>().critImmunity && ((Random.Range(0, 100) < (character.CriticalChance - enemy.GetComponent<Character>().CriticalAvoidance)) || character.Focus || character.Stealth))
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
            enemy.GetComponent<Character>().SendMessage("Summoner" + ability, character);
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
    
    public void MethodOnCorruption()
    {
        if (character.abilities.CurrentAbilityCooldowns[1] != 0 && GlobalVariables.debuffGained[0].Equals("corruption") && !GlobalVariables.whomstJustGainedDebuff[0].name.Contains("Enemy"))
            character.abilities.CurrentAbilityCooldowns[1] -= 1;            
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
