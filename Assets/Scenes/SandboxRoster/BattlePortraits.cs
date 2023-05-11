using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePortraits : MonoBehaviour
{
    #region Unless you happen to be named Kiran Freund, don't edit
    //elements
    public Sprite FireBase;
    public Sprite LightningBase;
    public Sprite IceBase;
    public Sprite EarthBase;
    public Sprite LifeBase;
    public Sprite NightmareBase;
    public Sprite MagicBase;
    public Sprite MetalBase;
    public Sprite StarBase;
    public static Sprite Fire;
    public static Sprite Lightning;
    public static Sprite Ice;
    public static Sprite Earth;
    public static Sprite Life;
    public static Sprite Nightmare;
    public static Sprite Magic;
    public static Sprite Metal;
    public static Sprite Star;

    //factions
    public Sprite CrusadersBase;
    public static Sprite Crusaders;
    public List<string> behemothList = new List<string>();
    public GameObject portraitMatrix;
    public BattlePortrait initialPortrait;
    public List<string> modifiableArray = new List<string>();
    public GameObject[] cubes = new GameObject[5]; //will NEVER change from 5
    #endregion
    public GameObject[] charModels = new GameObject[ProfileInfo.characters.Length]; //whatever profileInfo.characters.length is
    public Sprite[] charSprites = new Sprite[ProfileInfo.characters.Length]; //whatever profileInfo.characters.length is                  these are the only lines you edit

    void Start()
    {
        behemothList.Add("037"); //Every time there's a behemoth unit, you gotta add their associated number in the array here
        #region Same here
        Fire = FireBase;
        Lightning = LightningBase;
        Ice = IceBase;
        Earth = EarthBase;
        Life = LifeBase;
        Nightmare = NightmareBase;
        Magic = MagicBase;
        Metal = MetalBase;
        Star = StarBase;
        //balls
        Crusaders = CrusadersBase;
        StartCoroutine(instantiate());
        foreach (Sprite spwite in charSprites)
        {
            charSpwites.Add(spwite);
        }
    }
    public static List<Sprite> charSpwites = new List<Sprite>();
    public void portraitArrayInstantiation()
    {
        BattlePortrait[] children = portraitMatrix.GetComponentsInChildren<BattlePortrait>();
        foreach (BattlePortrait child in children)
            Destroy(child.gameObject);
        for (int i = 0; i < (modifiableArray.Count / 2); i++)
        {
            for (int j = 0; j < 2; j++)
            {
                BattlePortrait instance = Instantiate(initialPortrait, portraitMatrix.transform);
                instance.characterString = modifiableArray[i * 2 + j];
                instance.SendMessage("instantiate");
                instance.transform.localScale = new Vector3(17.4f, 17.4f, 17.4f);
                instance.transform.position = portraitMatrix.transform.position + (portraitMatrix.transform.up * -.18f * portraitMatrix.transform.localScale.x) + (portraitMatrix.transform.right * .11f * portraitMatrix.transform.localScale.x);
                instance.transform.position += portraitMatrix.transform.right * 2.4f * j * portraitMatrix.transform.localScale.x;
                instance.transform.position += portraitMatrix.transform.up * -3f * i * portraitMatrix.transform.localScale.x;
            }
        }
        for (int j = 0; j < (modifiableArray.Count % 2); j++)
        {
            BattlePortrait instance = Instantiate(initialPortrait, portraitMatrix.transform);
            instance.characterString = modifiableArray[(modifiableArray.Count / 2) * 2 + j];
            instance.SendMessage("instantiate");
            instance.transform.localScale = new Vector3(17.4f, 17.4f, 17.4f);
            instance.transform.position = portraitMatrix.transform.position + (portraitMatrix.transform.up * -.18f * portraitMatrix.transform.localScale.x) + (portraitMatrix.transform.right * .11f * portraitMatrix.transform.localScale.x);
            instance.transform.position += portraitMatrix.transform.right * 2.4f * j * portraitMatrix.transform.localScale.x;
            instance.transform.position += portraitMatrix.transform.up * -3f * (modifiableArray.Count / 2) * portraitMatrix.transform.localScale.x;
        }
    }

    IEnumerator instantiate()
    {
        yield return new WaitForSeconds(.001f);
        foreach (string i in ProfileInfo.characters)
        {
            modifiableArray.Add(i);
        }
        portraitArrayInstantiation();
    }

    public void OnPortraitClick(string character)
    {
        int count = 0;
        for (int i = 0; i < cubes.Length; i++)
        {
            if (cubes[i].GetComponent<BattleDisplay>().filled)
            {
                count++;
            }
        }
        if ((!behemothList.Contains(character.Substring(0, 3)) && cubes.Length - count > 0) || (behemothList.Contains(character.Substring(0, 3)) && cubes.Length - count > 1)) {
            modifiableArray.Remove(character);
            portraitArrayInstantiation();
            for (int i = 0; i < cubes.Length; i++)
            {
                if (!cubes[i].GetComponent<BattleDisplay>().filled)
                {
                    cubes[i].GetComponent<BattleDisplay>().instantiate(character);
                    cubes[i].GetComponent<BattleDisplay>().filled = true;
                    if (behemothList.Contains(character.Substring(0, 3)))
                    {
                        cubes[i+1].GetComponent<BattleDisplay>().filled = true;
                        cubes[i + 1].GetComponent<BattleDisplay>().characterString = "behemoth";
                        i++;
                    }
                    return;
                }
            }
        }
    }
    public void OnBehemothClick()
    {
        for (int i = 0; i < cubes.Length; i++)
        {
            if (cubes[i].GetComponent<BattleDisplay>().filled && cubes[i].GetComponent<BattleDisplay>().characterString.Equals("behemoth"))
            {
                cubes[i].GetComponent<BattleDisplay>().OnMouseUp();
                return;
            }
        }
    }
    #endregion
}
