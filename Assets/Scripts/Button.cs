using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    GameObject currentUnit;
    public GameObject text;
    public Sprite texture;
    public int cooldown;
    public int currentCooldown;
    public int abilityNumber;
    public string charName;
    public bool abilityBlock;
    public bool allySelector = false;
    public void Update()
    {
        text.GetComponent<Text>().text = "" + currentCooldown;
        GetComponent<Image>().sprite = texture;
        if (currentCooldown < 0)
            currentCooldown = 0;
        if (currentCooldown == 0)
            text.GetComponent<Text>().CrossFadeAlpha(0, 0, false);
        else
            text.GetComponent<Text>().CrossFadeAlpha(1, 0, false);
    }
    public void setShit(BabyButton b)
    {
        texture = b.texture;
        cooldown = b.cooldown;
        currentCooldown = b.currentCooldown;
        abilityNumber = b.abilityNumber;
        charName = b.charName;
        allySelector = b.spetcial;
        if (abilityBlock)
            texture = GlobalTextures.AbilityBlockButton;
        GetComponent<Image>().sprite = texture;

    }
    float time = 0;
    bool canPop = false;
    public void OnMouseDrag()
    {
        if (canPop)
        {
            if (time < .5f)
            {
                time += Time.deltaTime;
            }
            else
            {
                time = 0;
                currentUnit = GlobalVariables.playerArray[0];
                StatusPopup.playerText.text = currentUnit.GetComponent<Character>().abilities.AbilityTexts[abilityNumber].Replace("\\n", "\n"); ;
                StatusPopup.AbilityPop();
                canPop = false;
            }
        }
    }
    public void OnMouseDown()
    {
        canPop = true;
    }

    public void pressed()
    {
        currentUnit = GlobalVariables.playerArray[0];
        if (abilityNumber == 100)
        {
            currentUnit.GetComponent<Character>().SendMessage(charName);
            for (int i = 0; i < currentUnit.GetComponent<Character>().boutons.Count; i++)
            {
                currentUnit.GetComponent<Character>().boutons[i].currentCooldown -= 1;
            }
            StartCoroutine(Wait());
            currentUnit.GetComponent<Character>().boutons.Remove(currentUnit.GetComponent<Character>().boutons[currentUnit.GetComponent<Character>().boutons.Count - 1]);
        }
        else if (!allySelector)
        {
            if (currentCooldown == 0 && !currentUnit.GetComponent<Character>().abilityBlock)
            {
                currentCooldown = cooldown;
                currentUnit.GetComponent<Character>().SendMessage("Special" + abilityNumber, GlobalVariables.enemiesSelected[0]);
                currentUnit.GetComponent<Character>().boutons[abilityNumber - 1].currentCooldown = currentUnit.GetComponent<Character>().boutons[abilityNumber - 1].cooldown;
                for (int i = 0; i < currentUnit.GetComponent<Character>().boutons.Count; i++)
                {
                    currentUnit.GetComponent<Character>().boutons[i].currentCooldown -= 1;
                }
                StartCoroutine(Wait());
            }
        }
        else
        {
            StartCoroutine(ifAllySelector());
        }
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(.2f);
    }

    IEnumerator ifAllySelector()
    {
        GlobalVariables.allySelectable = true;
        while (GlobalVariables.allySelectable && GlobalVariables.playerArray.Length > 0)
        {
            yield return new WaitForSeconds(.00f);
        }
        if (currentCooldown == 0 && !currentUnit.GetComponent<Character>().abilityBlock && GlobalVariables.playerArray.Length > 0)
        {
            GlobalVariables.allySelectable = false;
            currentCooldown = cooldown;
            currentUnit.GetComponent<Character>().SendMessage("Special" + abilityNumber, GlobalVariables.enemiesSelected[0]);
            currentUnit.GetComponent<Character>().boutons[abilityNumber - 1].currentCooldown = currentUnit.GetComponent<Character>().boutons[abilityNumber - 1].cooldown;
            for (int i = 0; i < currentUnit.GetComponent<Character>().boutons.Count; i++)
            {
                currentUnit.GetComponent<Character>().boutons[i].currentCooldown -= 1;
            }
            StartCoroutine(Wait());
        }
        GlobalVariables.allySelectable = false;
    }
}

