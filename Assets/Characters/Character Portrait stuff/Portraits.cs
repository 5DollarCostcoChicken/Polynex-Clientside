using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevelopersHub.RealtimeNetworking.Client;
using System;

public class Portraits : MonoBehaviour
{
    public static Portraits _instance = null; public static Portraits instance { get { return _instance; } }

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

    //number of balls in yo jaw
    public GameObject portraitMatrix;
    public Portrait initialPortrait;


    //the array of characters
    public List<Data.Character> characters = new List<Data.Character>();
    // Start is called before the first frame update
    private void Awake()
    {
        _instance = this;
        Packet packet = new Packet();
        packet.Write((int)2);
        packet.Write(SystemInfo.deviceUniqueIdentifier);
        Sender.TCP_Send(packet);
    }

    void Start()
    {
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
    }

    public void standardInstantiation()
    {
        //sorting the array (standard sort, nothing extraneous like faction preference)
        characters.Sort((x, y) =>
        {
            if (x.activated != y.activated)
            {
                return x.activated ? -1 : 1;
            }
            if (x.power != y.power)
            {
                return y.power.CompareTo(x.power);
            }
            return string.Compare(x.cName, y.cName, StringComparison.Ordinal);
        });
        //instantiating the array of portraits
        for (int i = 0; i < characters.Count; i++)
        {
            Portrait instance = Instantiate(initialPortrait, portraitMatrix.transform);
            instance.gameObject.SetActive(true);
            instance.setVars(characters[i].char_index, characters[i].cName, characters[i].stars, characters[i].level, 5);
        }
    }
}
