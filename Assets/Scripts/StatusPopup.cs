using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusPopup : MonoBehaviour
{
    [Header("Status Popup")]
    public static Text statusText;
    public static Image statusImage;
    public static GameObject me;
    public GameObject m;
    public Text statusT;
    public Image statusI;

    [Header("Enemy Ability Popup")]
    public static Text enemyText;
    public static GameObject eneme;
    public GameObject em;
    public Text enemyT;

    [Header("Player Ability Popup")]
    public static Text playerText;
    public static GameObject playerme;
    public GameObject pm;
    public Text playerT;

    [Header("Other")]
    public static StatusPopup Instance;
    public StatusPopup instance;
    public static GameObject PassiveIcon;
    public GameObject PI;
    // Start is called before the first frame update
    void Start()
    {
        statusImage = statusI;
        statusText = statusT;
        me = m;

        enemyText = enemyT;
        eneme = em;

        playerText = playerT;
        playerme = pm;

        Instance = instance;
        PassiveIcon = PI;
    }
    public void OnStatusClick()
    {
        Instance.StartCoroutine(statusRollUp());
    }
    public void OnEnemyClick()
    {
        Instance.StartCoroutine(enemyRollUp());
    }
    public void OnAbilityClick()
    {
        Instance.StartCoroutine(playerRollUp());
    }
    public static void StatusPop()
    {
        Instance.StartCoroutine(statusRoll());
        Instance.StartCoroutine(enemyRollUp());
        Instance.StartCoroutine(playerRollUp());
    }
    public static void AbilityPop()
    {
        Instance.StartCoroutine(statusRollUp());
        Instance.StartCoroutine(enemyRollUp());
        Instance.StartCoroutine(playerRoll());
    }
    public static void EnemyPop()
    {
        Instance.StartCoroutine(statusRollUp());
        Instance.StartCoroutine(enemyRoll());
        Instance.StartCoroutine(playerRollUp());
    }
    static IEnumerator statusRoll()
    {
        me.SetActive(true);
        me.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 130, 0);
        int count = 0;
        while (me.GetComponent<RectTransform>().anchoredPosition.y > 0)
        {
            me.GetComponent<RectTransform>().localPosition += Vector3.up * (1f + count) * -1;
            count++;
            yield return new WaitForSeconds(.01f);
        }
        if (me.GetComponent<RectTransform>().anchoredPosition.y < 0)
        {
            me.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
        }
    }
    static IEnumerator statusRollUp()
    {
        me.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
        int count = 0;
        while (me.GetComponent<RectTransform>().anchoredPosition.y < 130)
        {
            me.GetComponent<RectTransform>().localPosition += Vector3.up * (1f + count);
            count++;
            yield return new WaitForSeconds(.01f);
        }
        me.SetActive(false);
    }
    static IEnumerator enemyRoll()
    {
        eneme.SetActive(true);
        eneme.GetComponent<RectTransform>().anchoredPosition = new Vector3(478, 136.4f, 0);
        int count = 0;
        while (eneme.GetComponent<RectTransform>().anchoredPosition.x > 1)
        {
            eneme.GetComponent<RectTransform>().localPosition += Vector3.right * (1f + count) * -3;
            count++;
            yield return new WaitForSeconds(.01f);
        }
        if (eneme.GetComponent<RectTransform>().anchoredPosition.x < 1)
        {
            eneme.GetComponent<RectTransform>().anchoredPosition = new Vector3(1, 136.4f, 0);
        }
    }
    static IEnumerator enemyRollUp()
    {
        eneme.GetComponent<RectTransform>().anchoredPosition = new Vector3(1, 136.40f, 0);
        int count = 0;
        while (eneme.GetComponent<RectTransform>().anchoredPosition.x < 478)
        {
            eneme.GetComponent<RectTransform>().localPosition += Vector3.right * (1f + count) * 3;
            count++;
            yield return new WaitForSeconds(.01f);
        }
        eneme.SetActive(false);
    }
    static IEnumerator playerRoll()
    {
        playerme.SetActive(true);
        playerme.GetComponent<RectTransform>().anchoredPosition = new Vector3(478, 68.7f, 0);
        int count = 0;
        while (playerme.GetComponent<RectTransform>().anchoredPosition.x > 1)
        {
            playerme.GetComponent<RectTransform>().localPosition += Vector3.right * (1f + count) * -3;
            count++;
            yield return new WaitForSeconds(.01f);
        }
        if (playerme.GetComponent<RectTransform>().anchoredPosition.x < 1)
        {
            playerme.GetComponent<RectTransform>().anchoredPosition = new Vector3(1, 68.7f, 0);
        }
    }
    static IEnumerator playerRollUp()
    {
        playerme.GetComponent<RectTransform>().anchoredPosition = new Vector3(1, 68.7f, 0);
        int count = 0;
        while (playerme.GetComponent<RectTransform>().anchoredPosition.x < 478)
        {
            playerme.GetComponent<RectTransform>().localPosition += Vector3.right * (1f + count) * 3;
            count++;
            yield return new WaitForSeconds(.01f);
        }
        playerme.SetActive(false);
    }
}
