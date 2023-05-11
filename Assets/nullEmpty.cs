using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nullEmpty : ParentCharacter
{
    // Start is called before the first frame update
    void Start()
    {
        character.nameOfChar = "Name_of_Character_";
        character.Gender = "male_female_other";
        //tags
        string[] tags = {"Class", "Faction", "Element"};
        character.mainElement = "Element";

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
        setTags(tags);
        StartCoroutine(establishCooldowns());
    }
    IEnumerator start()
    {
        yield return new WaitForSeconds(.01f);
        // this start() method is used for certain things that have to wait for all characters' stats to finalize before activating, such as leader abilities
    }

    // / / / / / / / / / / / / / / / / / / / / / / / / / / Abilities / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void Basic(GameObject enemy2)
    {

    }
    IEnumerator basic()
    {
        yield return new WaitForSeconds(1f);
    }
    public void Special1(GameObject enemy2)
    {

    }
    IEnumerator special1()
    {
        yield return new WaitForSeconds(1f);
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
    
}
