using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CorruptCultist : ParentCharacter
{
    
    public GameObject hand;
    public GameObject arrow;
    int corruptionCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        //GlobalVariables.OnGotSelectedAlly += beans;
        GlobalVariables.OnDodge += MethodOnDodge;
        GlobalVariables.OnEndTurn += MethodOnCorruption;
        GlobalVariables.OnGotHit += MethodOnDie;
        GlobalVariables.OnDied += Summoned;

        character.nameOfChar = "CorruptCultist";
        character.Gender = "female";
        character.tags.Add("Cultist");
        character.tags.Add("Support");
        character.tags.Add("Attacker");
        character.tags.Add("Nightmare");
        character.tags.Add("Summoned");
        character.mainElement = "Nightmare";


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
        bool armuk = false;
        for (int i = 0; i < GlobalVariables.friendlyArray.Length; i++)
        {
            if (GlobalVariables.friendlyArray[i].GetComponent<Character>().nameOfChar.Equals("Armuk"))
            {
                armuk = true;
            }
        }
        if (armuk)
        {
            character.maxHealth = (int)(character.maxHealth * 1.2f);
            character.health = character.maxHealth;
            character.defense = (int)(character.defense * 1.2f);
            character.Tenacity = (int)(character.Tenacity * 1.2f);
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
        StartCoroutine(basic());
    }
    IEnumerator basic()
    {
        List<Character> CultistList = new List<Character>();
        for (int i = 0; i < GlobalVariables.friendlyArray.Length; i++)
        {
            if (GlobalVariables.friendlyArray[i].GetComponent<Character>().canAttackOutOfTurn && GlobalVariables.friendlyArray[i].GetComponent<Character>().tags.Contains("Cultist") && GlobalVariables.friendlyArray[i].GetComponent<Character>() != this)
                CultistList.Add(GlobalVariables.friendlyArray[i].GetComponent<Character>());
        }
        Character target = CultistList[Random.Range(0, CultistList.Count - 1)];
        if (Random.Range(0, 100) < 50 && target != null && !character.attackingOutOfTurn)
            target.SendMessage("AttackOutOfTurn", 1f);
        gameObject.tag = "Ally";
        GetComponent<Animator>().CrossFade("Basic", .3f);
        yield return new WaitForSeconds(1.01f);
        StartCoroutine(projectile(enemy, "Basic"));
        character.buffsAndDebuffs.ImageArray();
        yield return new WaitForSeconds(.71f);
        GetComponent<Animator>().CrossFade("Base Layer." + character.currentIdle, .25f);
        GlobalVariables.TakingTurn = false;
        character.buffsAndDebuffs.SendMessage("EndOfTurn");
        character.turnUpdate = true;
    }
    
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Special Ability 1 / / / / / / / / / / / / / / / / / / / / / / / / / / / / /

    public void Special1()
    {
        enemy = GlobalVariables.enemiesSelected[0];
        gameObject.tag = "Ally";
        basicButton.GetComponent<BasicButton>().moveDownAndBack(character.boutons.Count, Buttons);
        character.turnMeter = 0;
        GlobalVariables.addToWhomstJustUsedSpecial(character);
        StartCoroutine(special1(enemy));
    }
    IEnumerator special1(GameObject enemy2)
    {
        enemy = enemy2;
        gameObject.tag = "Ally";
        GetComponent<Animator>().Play("Special1");
        yield return new WaitForSeconds(.8f);
        if (character.buffsAndDebuffs.Corruption.getAmount() < 5)
        {
            character.buffsAndDebuffs.Corruption.burnAdd(2);
            if (!character.buffsAndDebuffs.DebuffList.Contains(character.buffsAndDebuffs.Corruption))
                character.buffsAndDebuffs.SendMessage("corruptionActivate");
            else
                character.buffsAndDebuffs.SendMessage("corruptionAdd");
            GlobalVariables.addToWhomstJustInflictedDebuff(character, "corruption");
        }
        StartCoroutine(projectile(enemy, "Special1"));
        character.buffsAndDebuffs.ImageArray();
        yield return new WaitForSeconds(.725f);
        GetComponent<Animator>().CrossFade("Base Layer." + character.currentIdle, .25f);
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
            enemy.GetComponent<Character>().SendMessage("CorruptCultistBasic", character);
            if (Random.Range(0, 100) < 25 + 5 * character.buffsAndDebuffs.Corruption.getAmount())
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
    }
   
    public void MethodOnCorruption()
    {
        int newcount = 0;
        for (int i = 0; i < GlobalVariables.friendlyArray.Length; i++)
        {
            newcount += GlobalVariables.friendlyArray[i].GetComponent<Character>().buffsAndDebuffs.Corruption.getAmount();
        }
        for (int i = 0; i < GlobalVariables.hostileArray.Length; i++)
        {
            newcount += GlobalVariables.hostileArray[i].GetComponent<Character>().buffsAndDebuffs.Corruption.getAmount();
        }
        character.speed *= (1 + .02f * (newcount - corruptionCount));
        character.dodge += 2 * (newcount - corruptionCount);
        corruptionCount = newcount;
    }
    public void MethodOnDodge()
    {
        for (int i = 0; i < GlobalVariables.friendlyArray.Length; i++)
        {
            if (GlobalVariables.friendlyArray[i].GetComponent<Character>().tags.Contains("Risen") || GlobalVariables.friendlyArray[i].GetComponent<Character>().tags.Contains("Cultist"))
                GlobalVariables.friendlyArray[i].GetComponent<Character>().speed *= 1.02f;
        }
    }
    public void Summoned()
    {
        bool allies = false;
        for (int i = 0; i < GlobalVariables.friendlyArray.Length; i++)
        {
            if (!GlobalVariables.friendlyArray[i].GetComponent<Character>().dead || GlobalVariables.friendlyArray[i].GetComponent<Character>() != character && !GlobalVariables.friendlyArray[i].GetComponent<Character>().tags.Contains("Summoned"))
                allies = true;
        }
        if (!allies && !character.dead)
            character.SendMessage("ded");
        if (GlobalVariables.whomstJustDied[0] = character)
            GlobalVariables.SummonSlot = false;
    }
    private void MethodOnDie()
    {
        if (GlobalVariables.whomstJustGotHit[0] == this.character && character.health <= 0)
        {
            GlobalVariables.addToWhomstJustDied(this.character);
            int newcount = 0;
            for (int i = 0; i < GlobalVariables.friendlyArray.Length; i++)
            {
                newcount += GlobalVariables.friendlyArray[i].GetComponent<Character>().buffsAndDebuffs.Corruption.getAmount();
                GlobalVariables.friendlyArray[i].GetComponent<Character>().buffsAndDebuffs.SendMessage("corruptionDeactivate");
                for (int j = 0; j < GlobalVariables.friendlyArray[i].GetComponent<Character>().buffsAndDebuffs.Corruption.getAmount(); j++)
                    GlobalVariables.friendlyArray[i].GetComponent<Character>().buffsAndDebuffs.SendMessage("corruptionSubtract");
                GlobalVariables.friendlyArray[i].GetComponent<Character>().buffsAndDebuffs.BlueBuffList.Remove(character.buffsAndDebuffs.EnergyAbsorption);
            }
            for (int i = 0; i < GlobalVariables.hostileArray.Length; i++)
            {
                newcount += GlobalVariables.hostileArray[i].GetComponent<Character>().buffsAndDebuffs.Corruption.getAmount();
                GlobalVariables.hostileArray[i].GetComponent<Character>().buffsAndDebuffs.SendMessage("corruptionDeactivate");
                for (int j = 0; j < GlobalVariables.hostileArray[i].GetComponent<Character>().buffsAndDebuffs.Corruption.getAmount(); j++)
                    GlobalVariables.hostileArray[i].GetComponent<Character>().buffsAndDebuffs.SendMessage("corruptionSubtract");
                GlobalVariables.hostileArray[i].GetComponent<Character>().buffsAndDebuffs.BlueBuffList.Remove(character.buffsAndDebuffs.EnergyAbsorption);
            }
            character.protection = 0;
            character.health = (int)(character.maxHealth * .03f * newcount);
        }
        if (GlobalVariables.whomstJustGotHit[0].name.Contains("Enemy") && GlobalVariables.whomstJustGotHit[0].GetComponent<Character>().buffsAndDebuffs.DebuffList.Contains(GlobalVariables.whomstJustGotHit[0].GetComponent<Character>().buffsAndDebuffs.Corruption) && Random.Range(0, 100) < (25 + 5 * character.buffsAndDebuffs.Corruption.getAmount()) && character.canAttackOutOfTurn)
        {
            enemy = GlobalVariables.enemiesSelected[0];
            StartCoroutine(AttackOutOfTurn(1f));
        }
    }

}
