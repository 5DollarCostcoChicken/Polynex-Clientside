using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class ParentCharacter : MonoBehaviour
{
    public Character character;

    public GameObject enemy;
    public string[] damageNumbah = new string[2];
    public int dmg;

    public GameObject blankButton;
    public GameObject basicButton;
    public GameObject Buttons;
    public List<GameObject> buttons = new List<GameObject>();

    public GameObject selectableIndicator;

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
        for (int i = 0; i < character.abilities.AbilityCooldowns.Count; i++)
        {
            if (character.abilities.CurrentAbilityCooldowns[i] >= 3700)
            {
                character.abilities.CurrentAbilityCooldowns[i] = character.abilities.CurrentAbilityCooldowns[i] % 3700;
                BabyButton specialB = new BabyButton(character.abilities.AbilityIcons[i + 1], character.abilities.AbilityCooldowns[i], character.abilities.CurrentAbilityCooldowns[i], i + 1, character.nameOfChar, true);
                character.boutons.Add(specialB);
            }
            else
            {
                BabyButton specialB = new BabyButton(character.abilities.AbilityIcons[i + 1], character.abilities.AbilityCooldowns[i], character.abilities.CurrentAbilityCooldowns[i], i + 1, character.nameOfChar);
                character.boutons.Add(specialB);
            }
        }
    }

    public void OnMouseDown()
    {
        if (GlobalVariables.allySelectable)
        {
            GlobalVariables.allySelectable = false;
            GlobalVariables.addToWhomstJustGotSelectedAlly(this.GetComponent<Character>());
        }
    }
    //This doesnt change all that much unless their stats change for a few turns without being tied to a status effect, such as Templar Champion and Cursed Guardian
    #region The Update Method
    public virtual void Update()
    {
        if (!character.dead)
        {
            selectableIndicator.GetComponent<SpriteRenderer>().enabled = GlobalVariables.allySelectable;
            //gaining turn meter and taking turn
            if (!GlobalVariables.TakingTurn && !GlobalVariables.countering)
            {
                if (GlobalVariables.playerArray.Length == 0)
                    character.turnMeter += ((float)character.speed * Time.deltaTime);
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
                            for (int i = 1; i <= character.boutons.Count; i++)
                            {
                                GameObject instance = Instantiate(blankButton, Buttons.transform); 
                                if (character.abilityBlock)
                                    instance.GetComponent<Button>().abilityBlock = true;
                                instance.transform.position = Buttons.transform.position + .60f * i * Buttons.transform.right;
                                instance.GetComponent<Button>().setShit(character.boutons[i - 1]);
                                buttons.Add(instance);
                            }
                            for (int i = 0; i < character.abilities.PassiveIcons.Count; i++)
                            {
                                GameObject instance = Instantiate(StatusPopup.PassiveIcon, Buttons.transform);
                                instance.transform.position = Buttons.transform.position + (.60f * (character.abilities.AbilityIcons.Count - 1) + .07f) * Buttons.transform.right - .40f * i * Buttons.transform.right + .475f * Buttons.transform.up;
                                instance.transform.localScale = new Vector3(instance.transform.localScale.x * 10.66666666f, instance.transform.localScale.y * 2f, 1);
                                instance.GetComponent<DisplayPassiveIcons>().displayImage.sprite = character.abilities.PassiveIcons[i];
                                instance.GetComponent<DisplayPassiveIcons>().displayText = character.abilities.PassiveTexts[i];
                                buttons.Add(instance);
                            }

                            basicButton.GetComponent<BasicButton>().texture = character.abilities.AbilityIcons[0];
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
                if (character.nameOfChar == "Shaubeny")
                {
                    this.GetComponent<Shaubeny>().Basic(null);
                }
                else
                    this.SendMessage("Basic", enemy);
            }
        }
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
                    if (GlobalVariables.AllyTaunters > 0)
                    {
                        List<GameObject> array = new List<GameObject>();
                        foreach (GameObject guy in GlobalVariables.friendlyArray)
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
                    gameObject.tag = "PlayerSelected";
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
                enemy.tag = "EnemySelected";
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
                character.attackingOutOfTurn = true; enemy = GlobalVariables.enemiesSelected[0];

                float counter2 = character.counter;
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
                character.counter = counter2;
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
    #region Cooldown Change
    public void cooldownChange(int number)
    {
        for (int i = 0; i < character.boutons.Count; i++)
        {
            character.boutons[i].currentCooldown += number;
            if (character.boutons[i].currentCooldown < 0)
                character.boutons[i].currentCooldown = 0;
            if (character.boutons[i].currentCooldown > character.boutons[i].cooldown)
                character.boutons[i].currentCooldown = character.boutons[i].cooldown;
        }
    }
    #endregion
}
