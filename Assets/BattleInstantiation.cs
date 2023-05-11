using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class BattleInstantiation : MonoBehaviour
{
    #region Dont fucking touch this you dickweeds
    public GameObject[] players = new GameObject[ProfileInfo.characters.Length]; //whatever the other lists' amounts are; its the num characters
    public GameObject[] enemies = new GameObject[ProfileInfo.characters.Length];
    public GameObject[] playerSpawn = new GameObject[6]; //keep these 2 static at 6
    public GameObject[] enemySpawn = new GameObject[6];

    public GameObject blankButton;
    public GameObject basicButton;
    public GameObject buttons;
    public GameObject cameraBeans;
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Instantiate());
    }
    private void Update()
    {
        if (!GlobalVariables.TakingTurn)
            buttons.transform.localPosition = new Vector3(383.7f, -200, 0);
    }
    #endregion
    public void instantiate()
    {
        StartCoroutine(Instantiate());
    }
    public IEnumerator Instantiate()
    {
        #region you dont have to worry about this
        GlobalVariables.TakingTurn = false;
        GlobalVariables.ClearAllEvents();
        buttons.transform.localPosition = new Vector3(383.7f,-200,0);
        for (int i = 0; i < playerSpawn.Length - 1; i++)
        {
            foreach (Transform child in playerSpawn[i].transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            if (BattleStart.players[i] != null && !BattleStart.players[i].Equals("behemoth"))
            {
                if (BattleStart.players[i].Length != 0)
                {

                    GameObject instance = Instantiate(players[int.Parse(BattleStart.players[i].Substring(0, 3))], playerSpawn[i].transform);
                    if (i == 0)
                        instance.GetComponent<Character>().isLeader = true;
                    instance.transform.SetPositionAndRotation(playerSpawn[i].transform.position, playerSpawn[i].transform.rotation);
                    instance.transform.localScale = new Vector3(452.919f, 452.919f, 452.919f);
                    instance.GetComponent<BuffsDebuffs>().globalTextures = this.GetComponent<GlobalTextures>();
                    instance.GetComponent<BuffsDebuffs>().healthAndArmor.GetComponent<RotateToCam>().Camera = cameraBeans;
                    instance.GetComponent<ParentCharacter>().blankButton = blankButton;
                    instance.GetComponent<ParentCharacter>().basicButton = basicButton;
                    instance.GetComponent<ParentCharacter>().Buttons = buttons;
                    #endregion
                    switch (int.Parse(BattleStart.players[i].Substring(0, 3)))
                    {
                        case 16:
                            instance.GetComponent<Summoner>().summonSpawn = playerSpawn[5];
                            break;
                        case 38:
                            instance.GetComponent<Virion>().blankButton = blankButton;
                            instance.GetComponent<Virion>().basicButton = basicButton;
                            instance.GetComponent<Virion>().Buttons = buttons;
                            int passive1 = 1;
                            for (int v = 0; v < BattleStart.players.Count(); v++)
                            {
                                if (BattleStart.players[v] != null && BattleStart.players[v].Length > 0)
                                {
                                    passive1++;
                                }
                            }
                            instance.GetComponent<Virion>().Passive1 = (playerSpawn.Length - passive1);
                            break;
                    }
                    #region Mind ya business
                }
            }
        }
        for (int i = 0; i < enemySpawn.Length - 1; i++)
        {
            foreach (Transform child in enemySpawn[i].transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            if (BattleStart.enemies[i] != null && !BattleStart.enemies[i].Equals("behemoth"))
            {
                if (BattleStart.enemies[i].Length != 0)
                {
                    GameObject instance = Instantiate(enemies[int.Parse(BattleStart.enemies[i].Substring(0, 3))], enemySpawn[i].transform);
                    if (i == 0)
                        instance.GetComponent<Character>().isLeader = true;
                    instance.transform.SetPositionAndRotation(enemySpawn[i].transform.position, enemySpawn[i].transform.rotation);
                    instance.transform.localScale = new Vector3(452.919f, 452.919f, 452.919f);
                    instance.GetComponent<BuffsDebuffs>().globalTextures = this.GetComponent<GlobalTextures>();
                    instance.GetComponent<BuffsDebuffs>().healthAndArmor.GetComponent<RotateToCam>().Camera = cameraBeans;
                    switch (int.Parse(BattleStart.enemies[i].Substring(0, 3)))
                    {
                        case 16:
                            instance.GetComponent<SummonerEnemy>().summonSpawn = enemySpawn[5];
                            break;
                        case 38:
                            int passive1 = 1;
                            for (int v = 0; v < BattleStart.enemies.Count(); v++)
                            {
                                if (BattleStart.enemies[v] != null && BattleStart.enemies[v].Length > 0)
                                {
                                    passive1++;
                                }
                            }
                            instance.GetComponent<VirionEnemy>().Passive1 = (enemySpawn.Length - passive1);
                            break;
                    }
                }
            }
        }
        yield return new WaitForSeconds(.000001f);
    }
    #endregion
}



