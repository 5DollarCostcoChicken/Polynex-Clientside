using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class ParentEnemy : MonoBehaviour
{
    public Character character;

    public GameObject enemy;
    public string[] damageNumbah = new string[2];
    public int dmg;

    public void setTags(string[] tagies)
    {
        foreach (string tag in tagies)
        {
            character.tags.Add(tag);
        }
    }
    public IEnumerator establishCooldowns()
    {
        yield return new WaitForSeconds(.1f);
        for (int i = 0; i < GlobalPortraits.characterAbilityIcons.Count; i++)
        {
            if (GlobalPortraits.characterAbilityIcons[i] != null)
            {
                if (this.name.Contains(GlobalPortraits.characterAbilityIcons[i].characterName))
                {
                    character.abilities = GlobalPortraits.characterAbilityIcons[i];
                }
            }
        }
        for (int i = 0; i < character.abilities.CurrentAbilityCooldowns.Count; i++)
        {
            if (character.abilities.CurrentAbilityCooldowns[i] >= 3700)
                character.abilities.CurrentAbilityCooldowns[i] = character.abilities.CurrentAbilityCooldowns[i] % 3700;
        }
    }
    #region Cooldown Change
    public void cooldownChange(int number)
    {
        for (int i = 0; i < character.abilities.CurrentAbilityCooldowns.Count; i++)
        {
            character.abilities.CurrentAbilityCooldowns[i] += number;
            if (character.abilities.CurrentAbilityCooldowns[i] < 0)
                character.abilities.CurrentAbilityCooldowns[i] = 0;
            if (character.abilities.CurrentAbilityCooldowns[i] > character.abilities.AbilityCooldowns[i])
                character.abilities.CurrentAbilityCooldowns[i] = character.abilities.AbilityCooldowns[i];
        }
    }
    #endregion
    #region OnMouseDown
    void OnMouseDown()
    {
        if (!character.untargetable && GlobalVariables.playerArray.Length > 0)
        {
            if (GlobalVariables.EnemyTaunters > 0)
            {
                if (character.Taunt || (!character.Stealth && GlobalVariables.playerArray[0].GetComponent<Character>().TauntIgnore) || (character.Stealth && GlobalVariables.playerArray[0].GetComponent<Character>().TauntIgnore && GlobalVariables.playerArray[0].GetComponent<Character>().StealthIgnore))
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
            else
            {
                if ((character.Stealth && GlobalVariables.playerArray[0].GetComponent<Character>().StealthIgnore) || !character.Stealth || GlobalVariables.allStealthed)
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
        }
    }
    #endregion

    //This doesnt change all that much unless their stats change for a few turns without being tied to a status effect, such as Templar Champion and Cursed Guardian
    #region The Update Method
    public virtual void Update()
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
                            character.turnUpdate = false;
                            if (character.Stun)
                            {
                                character.turnMeter = 0;
                                GlobalVariables.TakingTurn = false;
                                cooldownChange(-1);
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
    public IEnumerator EnemySelectedWait()
    {
        yield return new WaitForSeconds(1);
        gameObject.tag = "EnemySelected";
    }
    public void damageNumbers(string color, string name)
    {
        damageNumbah[0] = color;
        damageNumbah[1] = name;
        SendMessage("damageNombers", damageNumbah);
    }
    #endregion

    //These change occasionally
    #region Attacking out of Turn
    // / / / / / / / / / / / / / / / / / / / / / / / / / / ENEMY Counter / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public virtual IEnumerator Counter(float DamageMultiplier)
    {
        enemy.GetComponent<Character>().actionQueue++;
        if (!enemy.GetComponent<Character>().dead)
        {
            if (enemy.GetComponent<Character>().canAttackOutOfTurn)
            {
                GlobalVariables.numCounters++; dmg = (int)(dmg * DamageMultiplier);
                yield return new WaitForSeconds(1.25f);
                enemy.GetComponent<Character>().attackingOutOfTurn = true;

                float counter2 = character.counter;
                while (enemy.GetComponent<Character>().actionQueue > 0)
                {
                    if (GlobalVariables.EnemyTaunters > 0)
                    {
                        List<GameObject> array = new List<GameObject>();
                        foreach (GameObject guy in GlobalVariables.hostileArray)
                        {
                            if ((guy.GetComponent<Character>().Taunt || (!guy.GetComponent<Character>().Stealth && character.TauntIgnore) || (guy.GetComponent<Character>().Stealth && character.TauntIgnore && character.StealthIgnore)) && guy.GetComponent<Character>().health > 0)
                            {
                                array.Add(guy);
                            }
                        }
                        int index = Random.Range(0, array.Count - 1);
                        enemy.SendMessage("Basic", array[index]);
                    }
                    else
                    {
                        enemy.SendMessage("Basic", gameObject);
                    }
                    GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Counter Attack";
                    damageNumbah[0] = "White";
                    damageNumbah[1] = "me";
                    enemy.GetComponent<Character>().SendMessage("damageNombers", damageNumbah);
                    gameObject.tag = "EnemySelected";
                    yield return new WaitForSeconds(.8f);
                    while (Vector3.Distance(enemy.GetComponent<Character>().initial, enemy.transform.position) > .0001f)
                    {
                        yield return new WaitForSeconds(.1f);
                    }
                    yield return new WaitForSeconds(.3f);
                    enemy.GetComponent<Character>().actionQueue--;
                }
                character.counter = counter2;
                enemy.GetComponent<Character>().attackingOutOfTurn = false;
                GlobalVariables.numCounters--; dmg = (int)(dmg / DamageMultiplier);
                enemy.tag = "PlayerSelected";
            }
            else
                enemy.GetComponent<Character>().actionQueue--;
        }
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / You Assisting / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public virtual IEnumerator AttackOutOfTurn(float DamageMultiplier)
    {
        character.actionQueue++;
        if (!character.attackingOutOfTurn)
        {
            yield return new WaitForSeconds(.5f);
            if (character.canAttackOutOfTurn && !character.dead)
            {
                GlobalVariables.numCounters++; dmg = (int)(dmg * DamageMultiplier);
                character.attackingOutOfTurn = true; if (GlobalVariables.playersSelected[0] != null) enemy = GlobalVariables.playersSelected[0]; else yield break;

                float counter2 = enemy.GetComponent<Character>().counter;
                while (character.actionQueue > 0 && enemy.GetComponent<Character>().health > 0)
                {
                    GlobalVariables.assisters++;
                    GlobalVariables.addToWhomstJustAttackedOutOfTurn(this.GetComponent<Character>());
                    SendMessage("Basic", enemy);
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
    #endregion

    #region Look at Enemy
    public IEnumerator LookAtEnemy()
    {
        transform.LookAt(enemy.transform);
        yield return new WaitForSeconds(.6f);
        Vector3 lTargetDir;
        lTargetDir.y = 0.0f;
        for (int i = 0; i < 50; i++)
        {
            lTargetDir = enemy.transform.position - transform.position;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lTargetDir), Time.time * .005f);
            Vector3 v = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(0, v.y, 0);
            yield return new WaitForSeconds(.001f);
        }
    }
    #endregion
    public abstract void AbilityAI(List<GameObject> enemies);
}
