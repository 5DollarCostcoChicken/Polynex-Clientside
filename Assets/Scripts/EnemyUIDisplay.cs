using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUIDisplay : MonoBehaviour
{
    [Header("General")]
    public Image portrait;
    public GameObject everything;
    public Text charName;
    public Image element;
    public string previous;
    public GameObject health;
    public GameObject healthBG;
    public GameObject armor;
    public GameObject armorBG;

    [Header("Elements")]
    public Sprite Metal;
    public Sprite Magic;
    public Sprite Fire;
    public Sprite Ice;
    public Sprite Earth;
    public Sprite Lightning;
    public Sprite Life;
    public Sprite Nightmare;

    Gradient gradient;
    GradientColorKey[] colorKey;
    GradientAlphaKey[] alphaKey;

    [Header("Abilties")]
    public GameObject activeAbilities;
    public GameObject passiveAbilities;
    public GameObject activePrefab;
    public GameObject passivePrefab;

    private void Start()
    {
        gradient = new Gradient();

        // Populate the color keys at the relative time 0 and 1 (0 and 100%)
        colorKey = new GradientColorKey[3];
        colorKey[0].color = Color.red;
        colorKey[0].time = 0.2f;
        colorKey[1].color = Color.yellow;
        colorKey[1].time = .5f;
        colorKey[2].color = Color.green;
        colorKey[2].time = 1.0f;

        // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
        alphaKey = new GradientAlphaKey[1];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;

        gradient.SetKeys(colorKey, alphaKey);
    }
    // Update is called once per frame
    void Update()
    {
        if (GlobalVariables.enemiesSelected.Length > 0)
        {
            if (GlobalVariables.enemiesSelected[0] != null) {
                everything.SetActive(true);
                #region healthbars
                health.GetComponent<Image>().color = gradient.Evaluate((float)GlobalVariables.enemiesSelected[0].GetComponent<Character>().health / GlobalVariables.enemiesSelected[0].GetComponent<Character>().maxHealth);
                if (GlobalVariables.enemiesSelected[0].GetComponent<Character>().Armor)
                {
                    health.GetComponent<RectTransform>().sizeDelta = new Vector2(442 * (float)GlobalVariables.enemiesSelected[0].GetComponent<Character>().health / GlobalVariables.enemiesSelected[0].GetComponent<Character>().maxHealth, 34);
                    healthBG.GetComponent<RectTransform>().sizeDelta = new Vector2(442, 34);
                    armor.GetComponent<RectTransform>().sizeDelta = new Vector2(442 * (float)GlobalVariables.enemiesSelected[0].GetComponent<Character>().protection / GlobalVariables.enemiesSelected[0].GetComponent<Character>().maxProtection, 34);

                    armor.SetActive(true);
                    armorBG.SetActive(true);
                }
                else
                {
                    health.GetComponent<RectTransform>().sizeDelta = new Vector2(442 * (float)GlobalVariables.enemiesSelected[0].GetComponent<Character>().health / GlobalVariables.enemiesSelected[0].GetComponent<Character>().maxHealth, 50);
                    healthBG.GetComponent<RectTransform>().sizeDelta = new Vector2(442, 50);

                    armor.SetActive(false);
                    armorBG.SetActive(false);
                }
                #endregion
                if (!GlobalVariables.enemiesSelected[0].name.Equals(previous))
                {
                    for (int i = 0; i < GlobalPortraits.characterAbilityIcons.Count; i++)
                    {
                        if (GlobalPortraits.characterAbilityIcons[i] != null)
                        {
                            if (GlobalVariables.enemiesSelected[0].name.Contains(GlobalPortraits.characterAbilityIcons[i].characterName))
                            {
                                portrait.sprite = GlobalPortraits.characterAbilityIcons[i].characterPortrait;
                                charName.text = GlobalPortraits.characterAbilityIcons[i].displayName;
                                previous = GlobalVariables.enemiesSelected[0].name;

                                switch (GlobalPortraits.characterAbilityIcons[i].mainElement)
                                {
                                    case "Metal":
                                        element.sprite = Metal;
                                        break;
                                    case "Magic":
                                        element.sprite = Magic;
                                        break;
                                    case "Fire":
                                        element.sprite = Fire;
                                        break;
                                    case "Ice":
                                        element.sprite = Ice;
                                        break;
                                    case "Earth":
                                        element.sprite = Earth;
                                        break;
                                    case "Lightning":
                                        element.sprite = Lightning;
                                        break;
                                    case "Life":
                                        element.sprite = Life;
                                        break;
                                    case "Nightmare":
                                        element.sprite = Nightmare;
                                        break;
                                }
                                #region ability icons
                                //active abilities
                                while (activeAbilities.transform.childCount > 0)
                                {
                                    DestroyImmediate(activeAbilities.transform.GetChild(0).gameObject);
                                }
                                int j;
                                for (j = 0; j < GlobalPortraits.characterAbilityIcons[i].AbilityIcons.Count; j++)
                                {
                                    GameObject instance = Instantiate(activePrefab, activeAbilities.transform);
                                    instance.GetComponent<DisplayAbilityIcon>().displayImage.sprite = GlobalPortraits.characterAbilityIcons[i].AbilityIcons[j];
                                    instance.GetComponent<DisplayAbilityIcon>().displayText = "" + GlobalPortraits.characterAbilityIcons[i].AbilityTexts[j];
                                    //text number setting
                                    if (j > 0) {
                                        if (GlobalPortraits.characterAbilityIcons[i].CurrentAbilityCooldowns[j - 1] > 0)
                                        {
                                            instance.GetComponent<DisplayAbilityIcon>().displayNumber.enabled = true;
                                            instance.GetComponent<DisplayAbilityIcon>().displayNumber.text = "" + GlobalPortraits.characterAbilityIcons[i].CurrentAbilityCooldowns[j - 1];
                                            instance.GetComponent<DisplayAbilityIcon>().displayImage.color = Color.gray;
                                        }
                                        else
                                            instance.GetComponent<DisplayAbilityIcon>().displayNumber.enabled = false;
                                    }
                                    else
                                        instance.GetComponent<DisplayAbilityIcon>().displayNumber.enabled = false;
                                    instance.GetComponent<RectTransform>().localPosition = new Vector3(30 + 17 * j - 17 * (GlobalPortraits.characterAbilityIcons[i].AbilityIcons.Count - 1), -18.67f, 0);
                                }

                                //passive abilities
                                while (passiveAbilities.transform.childCount > 0)
                                {
                                    DestroyImmediate(passiveAbilities.transform.GetChild(0).gameObject);
                                }
                                if (GlobalPortraits.characterAbilityIcons[i].PassiveIcons.Count == 0)
                                {
                                    passiveAbilities.SetActive(false);
                                }
                                else
                                {
                                    for (int k = 0; k < GlobalPortraits.characterAbilityIcons[i].PassiveIcons.Count; k++)
                                    {
                                        GameObject instance = Instantiate(passivePrefab, passiveAbilities.transform);
                                        instance.GetComponent<DisplayAbilityIcon>().displayImage.sprite = GlobalPortraits.characterAbilityIcons[i].PassiveIcons[k];
                                        instance.GetComponent<RectTransform>().localPosition = new Vector3(45 - 17 * j - 13.5f * (k + 1), -17.3f, 0);
                                        instance.GetComponent<DisplayAbilityIcon>().displayText = "" + GlobalPortraits.characterAbilityIcons[i].PassiveTexts[k];
                                    }
                                    passiveAbilities.SetActive(true);
                                }
                                #endregion
                            }
                        }
                    }
                }
            }
        }
        else
        {
            everything.SetActive(false);
        }
    }
}
