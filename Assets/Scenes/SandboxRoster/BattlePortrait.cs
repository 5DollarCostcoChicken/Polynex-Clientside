using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePortrait : MonoBehaviour
{
    #region Don't Edit
    public string characterString = "";
    string charName;
    int stars;
    int levels;
    int ascensionLevels;
    readonly int power;
    public GameObject levelText;
    public GameObject ascensionText;
    public GameObject firstStar;
    public GameObject element;
    public GameObject faction;
    public GameObject nameText;
    public BattlePortraits portraits;
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
        charName += "";
        if ((portraits.modifiableArray.Count - 1) >= (ProfileInfo.characters.Length - 5))
            portraits.OnPortraitClick(characterString);
    }

    IEnumerator instantiate()
    {
        yield return new WaitForSeconds(.00f);
        if (this.name.Contains("(Clone)"))
        {
            stars = (int.Parse(characterString.Substring(6, 1)));
            levels = (int.Parse(characterString.Substring(12, 3)));
            ascensionLevels = (int.Parse(characterString.Substring(22, 1)));
            levelText.GetComponent<TextMesh>().text = "" + levels;
            ascensionText.GetComponent<TextMesh>().text = "" + ascensionLevels;
            for (int i = 1; i < stars; i++)
            {
                GameObject instance = Instantiate(firstStar, this.transform);
                instance.transform.position = firstStar.transform.position + (firstStar.transform.right * .46f * i * firstStar.transform.localScale.x);
            }
            if (stars == 0)
                firstStar.GetComponent<SpriteRenderer>().enabled = false;
            this.GetComponent<SpriteRenderer>().sprite = BattlePortraits.charSpwites[int.Parse(characterString.Substring(0, 3))];
            #endregion
            //                   For the love of God I fucking hope you have the intuition to know what to do here.
            switch (int.Parse(characterString.Substring(0, 3)))
            {
                case 0:
                    charName = "TemplarPaladin";
                    nameText.GetComponent<TextMesh>().text = "Templar Paladi...";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Metal;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Crusaders;
                    break;
                case 1:
                    charName = "TemplarProtector";
                    nameText.GetComponent<TextMesh>().text = "Templar Protec...";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Metal;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Crusaders;
                    break;
                case 2:
                    charName = "TemplarPikeman";
                    nameText.GetComponent<TextMesh>().text = "Templar Pikeman";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Metal;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Crusaders;
                    break;
                case 3:
                    charName = "Gladiator";
                    nameText.GetComponent<TextMesh>().text = "Gladiator";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Metal;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Resistance;
                    break;
                case 4:
                    charName = "FrontlineGrunt";
                    nameText.GetComponent<TextMesh>().text = "Frontline Grunt";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Metal;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Crusaders;
                    break;
                case 5:
                    charName = "TemplarPriest";
                    nameText.GetComponent<TextMesh>().text = "Templar Priest";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Life;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Crusaders;
                    break;
                case 6:
                    charName = "TemplarChampion";
                    nameText.GetComponent<TextMesh>().text = "Templar Champi...";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Metal;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Crusaders;
                    break;
                case 7:
                    charName = "TemplarCommander";
                    nameText.GetComponent<TextMesh>().text = "Templar Comman...";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Metal;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Crusaders;
                    break;
                case 8:
                    charName = "Bloodletter";
                    nameText.GetComponent<TextMesh>().text = "Bloodletter";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Metal;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Wolfpack;
                    break;
                case 9:
                    charName = "WolfpackArcher";
                    nameText.GetComponent<TextMesh>().text = "Wolfpack Archer";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Metal;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Wolfpack;
                    break;
                case 10:
                    charName = "Executioner";
                    nameText.GetComponent<TextMesh>().text = "Executioner";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Metal;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Wolfpack;
                    break;
                case 11:
                    charName = "CursedGuardian";
                    nameText.GetComponent<TextMesh>().text = "Cursed Guardian";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Metal;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Wolfpack;
                    break;
                case 12:
                    charName = "Fanatic";
                    nameText.GetComponent<TextMesh>().text = "Fanatic";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Nightmare;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Risen;
                    break;
                case 13:
                    charName = "Shaubeny";
                    nameText.GetComponent<TextMesh>().text = "Lord Shaubeny";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Metal;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Crusaders;
                    break;
                case 14:
                    charName = "Wolfmother";
                    nameText.GetComponent<TextMesh>().text = "Wolfmother";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Metal;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Wolfpack;
                    break;
                case 15:
                    charName = "Fanatic's Creation";
                    nameText.GetComponent<TextMesh>().text = "Fanatic's Crea...";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Nightmare;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Risen;
                    break;
                case 16:
                    charName = "Cultist Summoner";
                    nameText.GetComponent<TextMesh>().text = "Cultist Summon...";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Nightmare;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Cultist;
                    break;
                case 17:
                    charName = "Corrupt Cultist";
                    nameText.GetComponent<TextMesh>().text = "Corrupt Cultist";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Nightmare;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Cultist;
                    break;
                case 18:
                    charName = "Fanatic's Warchief";
                    nameText.GetComponent<TextMesh>().text = "Fanatic's Warc...";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Nightmare;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Risen;
                    break;
                case 19:
                    charName = "Cultist Caster";
                    nameText.GetComponent<TextMesh>().text = "Cultist Caster";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Nightmare;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Cultist;
                    break;
                case 20:
                    charName = "Foppish Knight";
                    nameText.GetComponent<TextMesh>().text = "Foppish Knight";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Metal;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Resistance;
                    break;
                case 21:
                    charName = "Jester Knight";
                    nameText.GetComponent<TextMesh>().text = "Jester Knight";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Earth;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Resistance;
                    break;
                case 22:
                    charName = "Plague Doctor";
                    nameText.GetComponent<TextMesh>().text = "Plague Doctor";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Fire;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Resistance;
                    break;
                case 23:
                    charName = "Amadeus The Conductor";
                    nameText.GetComponent<TextMesh>().text = "Amadeus The Co...";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Metal;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Resistance;
                    break;
                case 24:
                    charName = "Nomad";
                    nameText.GetComponent<TextMesh>().text = "Nomad";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Metal;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Resistance;
                    break;
                case 25:
                    charName = "Dark Templar";
                    nameText.GetComponent<TextMesh>().text = "Dark Templar";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Metal;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.BlackLegion;
                    break;
                case 26:
                    charName = "Dark Praetorian";
                    nameText.GetComponent<TextMesh>().text = "Dark Praetorian";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Metal;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.BlackLegion;
                    break;
                case 27:
                    charName = "Dark Spartan";
                    nameText.GetComponent<TextMesh>().text = "Dark Spartan";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Metal;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.BlackLegion;
                    break;
                case 28:
                    charName = "Dark Sentinel";
                    nameText.GetComponent<TextMesh>().text = "Dark Sentinel";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Metal;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.BlackLegion;
                    break;
                case 29:
                    charName = "The Red Hand";
                    nameText.GetComponent<TextMesh>().text = "The Red Hand";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Metal;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.BlackLegion;
                    break;
                case 30:
                    charName = "Phantom";
                    nameText.GetComponent<TextMesh>().text = "Phantom";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Magic;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.BlackLegion;
                    break;
                case 31:
                    charName = "Superion";
                    nameText.GetComponent<TextMesh>().text = "Superion";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Magic;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.BlackLegion;
                    break;
                case 32:
                    charName = "Goblin Peon";
                    nameText.GetComponent<TextMesh>().text = "Goblin Peon";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Earth;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Goblin;
                    break;
                case 33:
                    charName = "Goblin Brute";
                    nameText.GetComponent<TextMesh>().text = "Goblin Brute";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Earth;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Goblin;
                    break;
                case 34:
                    charName = "Giant Spider";
                    nameText.GetComponent<TextMesh>().text = "Giant Spider";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Nightmare;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Monster;
                    break;
                case 35:
                    charName = "Spider Matriarch";
                    nameText.GetComponent<TextMesh>().text = "Spider Matriar...";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Nightmare;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Monster;
                    break;
                case 36:
                    charName = "Valgus Varus";
                    nameText.GetComponent<TextMesh>().text = "Valgus Varus";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Nightmare;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Monster;
                    break;
                case 37:
                    charName = "Ogre";
                    nameText.GetComponent<TextMesh>().text = "Ogre";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Earth;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Monster;
                    break;
                case 38:
                    charName = "Virion";
                    nameText.GetComponent<TextMesh>().text = "Virion";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Lightning;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.idk lmao;
                    break;
                case 39:
                    charName = "Berserkir";
                    nameText.GetComponent<TextMesh>().text = "Berserkir";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Ice;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.idk lmao;
                    break;
                case 40:
                    charName = "Caesar";
                    nameText.GetComponent<TextMesh>().text = "Caesar";
                    element.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Metal;
                    //faction.GetComponent<SpriteRenderer>().sprite = BattlePortraits.Wolfpack lmao;
                    break;
            }
        }
    }
}
