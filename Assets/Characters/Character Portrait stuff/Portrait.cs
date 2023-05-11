using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Portrait : MonoBehaviour
{
    #region Don't edit
    string charName;
    int charID;
    int stars;
    int levels;
    int ascensionLevels;
    public GameObject levelText;
    public GameObject ascensionText;
    public GameObject firstStar;
    public GameObject element;
    public GameObject faction;
    public GameObject nameText;
    /* character string 000.0.0.000.000.00000.0.0.0.0.0.0.0
     * Digits 1-3 : Character ID
     * Digit 4 : Binary Activated or not
     * Digit 5 : Stars
     * Digits 6-8 : Number of Shards (total out of 500)
     * Digits 9-11 : Level
     * Digits 12-16 : XP (Just for that level ig)
     * Digit 17 : Number of Ascension Levels Ascension
     * Digit 18 : Binary Ascension 1
     * Digit 19 : Binary Ascension 2
     * Digit 20 : Binary Ascension 3
     * Digit 21 : Binary Ascension 4
     * Digit 22 : Binary Ascension 5
     */
    void OnMouseUp()
    {
        Debug.Log(charID);
    }
    public void setVars(int cID, string cName, int star, int level, int aLevels)
    {
        charID = cID;
        charName = cName;
        stars = star;
        levels = level;
        ascensionLevels = aLevels;
        StartCoroutine(instantiate());
    }
    IEnumerator instantiate()
    {
        yield return new WaitForSeconds(.0011f);
        if (this.name.Contains("(Clone)"))
        {
            levelText.GetComponent<Text>().text = "" + levels;
            ascensionText.GetComponent<Text>().text = "" + ascensionLevels;
            for (int i = 1; i < stars; i++)
            {
                GameObject instance = Instantiate(firstStar, this.transform);
                instance.transform.position = firstStar.transform.position + (firstStar.transform.right * .46f * i * firstStar.transform.localScale.x);
            }
            if (stars == 0)
                firstStar.GetComponent<Image>().enabled = false;
#endregion
            //                              For the love of God I fucking hope you have the intuition to know what to do here.
            //                           This isn't identical to BattlePortrait's switch case cause Portraits != BattlePortraits
            switch (charID)
            {
                case 0:
                    nameText.GetComponent<Text>().text = "Templar Paladi...";
                    element.GetComponent<Image>().sprite = Portraits.Metal;
                    //faction.GetComponent<Image>().sprite = Portraits.Crusaders;
                    break;
                case 1:
                    nameText.GetComponent<Text>().text = "Templar Protec...";
                    element.GetComponent<Image>().sprite = Portraits.Metal;
                    //faction.GetComponent<Image>().sprite = Portraits.Crusaders;
                    break;
                case 2:                    
                    nameText.GetComponent<Text>().text = "Templar Pikeman";
                    element.GetComponent<Image>().sprite = Portraits.Metal;
                    //faction.GetComponent<Image>().sprite = Portraits.Crusaders;
                    break;
                case 3:
                    nameText.GetComponent<Text>().text = "Gladiator";
                    element.GetComponent<Image>().sprite = Portraits.Metal;
                    //faction.GetComponent<Image>().sprite = Portraits.Resistance;
                    break;
                case 4:
                    nameText.GetComponent<Text>().text = "Frontline Grunt";
                    element.GetComponent<Image>().sprite = Portraits.Metal;
                    //faction.GetComponent<Image>().sprite = Portraits.Crusaders;
                    break;
                case 5:
                    nameText.GetComponent<Text>().text = "Templar Priest";
                    element.GetComponent<Image>().sprite = Portraits.Metal;
                    //faction.GetComponent<Image>().sprite = Portraits.Crusaders;
                    break;
                case 6:
                    nameText.GetComponent<Text>().text = "Templar Champi...";
                    element.GetComponent<Image>().sprite = Portraits.Metal;
                    //faction.GetComponent<Image>().sprite = Portraits.Crusaders;
                    break;
                case 7:
                    nameText.GetComponent<Text>().text = "Templar Comman...";
                    element.GetComponent<Image>().sprite = Portraits.Metal;
                    //faction.GetComponent<Image>().sprite = Portraits.Crusaders;
                    break;
                case 8:
                    nameText.GetComponent<Text>().text = "Bloodletter";
                    element.GetComponent<Image>().sprite = Portraits.Metal;
                    //faction.GetComponent<Image>().sprite = Portraits.Wolfpack;
                    break;
                case 9:
                    nameText.GetComponent<Text>().text = "Wolfpack Archer";
                    element.GetComponent<Image>().sprite = Portraits.Metal;
                    //faction.GetComponent<Image>().sprite = Portraits.Wolfpack;
                    break;
                case 10:
                    nameText.GetComponent<Text>().text = "Executioner";
                    element.GetComponent<Image>().sprite = Portraits.Metal;
                    //faction.GetComponent<Image>().sprite = Portraits.Wolfpack;
                    break;
                case 11:
                    nameText.GetComponent<Text>().text = "Cursed Guardian";
                    element.GetComponent<Image>().sprite = Portraits.Metal;
                    //faction.GetComponent<Image>().sprite = Portraits.Wolfpack;
                    break;
                case 12:
                    nameText.GetComponent<Text>().text = "Fanatic";
                    element.GetComponent<Image>().sprite = Portraits.Nightmare;
                    //faction.GetComponent<Image>().sprite = Portraits.Risen;
                    break;
                case 13:
                    nameText.GetComponent<Text>().text = "Lord Shaubeny";
                    element.GetComponent<Image>().sprite = Portraits.Metal;
                    //faction.GetComponent<Image>().sprite = Portraits.Crusaders;
                    break;
                case 14:
                    nameText.GetComponent<Text>().text = "Wolfmother";
                    element.GetComponent<Image>().sprite = Portraits.Metal;
                    //faction.GetComponent<Image>().sprite = Portraits.Wolfpack;
                    break;
                case 15:
                    nameText.GetComponent<Text>().text = "Fanatic's Crea...";
                    element.GetComponent<Image>().sprite = Portraits.Nightmare;
                    //faction.GetComponent<Image>().sprite = Portraits.Risen;
                    break;
                case 16:
                    nameText.GetComponent<Text>().text = "Cultist Summon...";
                    element.GetComponent<Image>().sprite = Portraits.Nightmare;
                    //faction.GetComponent<Image>().sprite = Portraits.Cultist;
                    break;
                case 17:
                    nameText.GetComponent<Text>().text = "Corrupt Cultist";
                    element.GetComponent<Image>().sprite = Portraits.Nightmare;
                    //faction.GetComponent<Image>().sprite = Portraits.Cultist;
                    break;
                case 18:
                    nameText.GetComponent<Text>().text = "Fanatic's Warc...";
                    element.GetComponent<Image>().sprite = Portraits.Nightmare;
                    //faction.GetComponent<Image>().sprite = Portraits.Risen;
                    break;
                case 19:
                    nameText.GetComponent<Text>().text = "Cultist Caster";
                    element.GetComponent<Image>().sprite = Portraits.Nightmare;
                    //faction.GetComponent<Image>().sprite = Portraits.Cultist;
                    break;
                case 20:
                    nameText.GetComponent<Text>().text = "Foppish Knight";
                    element.GetComponent<Image>().sprite = Portraits.Metal;
                    //faction.GetComponent<Image>().sprite = Portraits.Resistance;
                    break;
                case 21:
                    nameText.GetComponent<Text>().text = "Jester Knight";
                    element.GetComponent<Image>().sprite = Portraits.Earth;
                    //faction.GetComponent<Image>().sprite = Portraits.Resistance;
                    break;
                case 22:
                    nameText.GetComponent<Text>().text = "Plague Doctor";
                    element.GetComponent<Image>().sprite = Portraits.Fire;
                    //faction.GetComponent<Image>().sprite = Portraits.Resistance;
                    break;
                case 23:
                    nameText.GetComponent<Text>().text = "Amadeus The Co...";
                    element.GetComponent<Image>().sprite = Portraits.Metal;
                    //faction.GetComponent<Image>().sprite = Portraits.Resistance;
                    break;
                case 24:
                    nameText.GetComponent<Text>().text = "Nomad";
                    element.GetComponent<Image>().sprite = Portraits.Metal;
                    //faction.GetComponent<Image>().sprite = Portraits.Resistance;
                    break;
                case 25:
                    nameText.GetComponent<Text>().text = "Dark Templar";
                    element.GetComponent<Image>().sprite = Portraits.Metal;
                    //faction.GetComponent<Image>().sprite = Portraits.BlackLegion;
                    break;
                case 26:
                    nameText.GetComponent<Text>().text = "Dark Praetorian";
                    element.GetComponent<Image>().sprite = Portraits.Metal;
                    //faction.GetComponent<Image>().sprite = Portraits.BlackLegion;
                    break;
                case 27:
                    nameText.GetComponent<Text>().text = "Dark Spartan";
                    element.GetComponent<Image>().sprite = Portraits.Metal;
                    //faction.GetComponent<Image>().sprite = Portraits.BlackLegion;
                    break;
                case 28:
                    nameText.GetComponent<Text>().text = "Dark Sentinel";
                    element.GetComponent<Image>().sprite = Portraits.Metal;
                    //faction.GetComponent<Image>().sprite = Portraits.BlackLegion;
                    break;
                case 29:
                    nameText.GetComponent<Text>().text = "The Red Hand";
                    element.GetComponent<Image>().sprite = Portraits.Metal;
                    //faction.GetComponent<Image>().sprite = Portraits.BlackLegion;
                    break;
                case 32:
                    nameText.GetComponent<Text>().text = "Goblin Peon";
                    element.GetComponent<Image>().sprite = Portraits.Earth;
                    //faction.GetComponent<Image>().sprite = Portraits.Goblin;
                    break;
                case 34:
                    nameText.GetComponent<Text>().text = "Giant Spider";
                    element.GetComponent<Image>().sprite = Portraits.Nightmare;
                    //faction.GetComponent<Image>().sprite = Portraits.Monster;
                    break;
                case 35:
                    nameText.GetComponent<Text>().text = "Spider Matriar...";
                    element.GetComponent<Image>().sprite = Portraits.Nightmare;
                    //faction.GetComponent<Image>().sprite = Portraits.Monster;
                    break;
                case 36:
                    nameText.GetComponent<Text>().text = "Valgus Varus";
                    element.GetComponent<Image>().sprite = Portraits.Nightmare;
                    //faction.GetComponent<Image>().sprite = Portraits.Monster;
                    break;
                case 37:
                    nameText.GetComponent<Text>().text = "Ogre";
                    element.GetComponent<Image>().sprite = Portraits.Earth;
                    //faction.GetComponent<Image>().sprite = Portraits.Monster;
                    break;
                case 38:
                    nameText.GetComponent<Text>().text = "Virion";
                    element.GetComponent<Image>().sprite = Portraits.Lightning;
                    //faction.GetComponent<Image>().sprite = Portraits.idk lmao;
                    break;
                case 39:
                    nameText.GetComponent<Text>().text = "Berserkir";
                    element.GetComponent<Image>().sprite = Portraits.Ice;
                    //faction.GetComponent<Image>().sprite = Portraits.Northmen lmao;
                    break;
                case 40:
                    nameText.GetComponent<Text>().text = "Caesar";
                    element.GetComponent<Image>().sprite = Portraits.Metal;
                    //faction.GetComponent<Image>().sprite = Portraits.Wolfpack lmao;
                    break;
            }
        }
    }
}
