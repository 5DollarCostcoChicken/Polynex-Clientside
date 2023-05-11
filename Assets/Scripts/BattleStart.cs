using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleStart : MonoBehaviour
{
    public GameObject friendlyArray;
    public GameObject enemyArray;
    public static BattleInstantiation battle;
    public static string[] players = new string[5];
    public static string[] enemies = new string[5];
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
    public void OnButtonPressed()
    {
        BattleDisplay[] friendlies = friendlyArray.GetComponentsInChildren<BattleDisplay>();
        BattleDisplay[] foelies = enemyArray.GetComponentsInChildren<BattleDisplay>();
        for (int i = 0; i < friendlies.Length; i++)
        {
            players[i] = friendlies[i].characterString;
        }
        for (int i = 0; i < foelies.Length; i++)
        {
            enemies[i] = foelies[i].characterString;
        }
        SceneManager.LoadScene(1);
    }
}
