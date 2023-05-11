using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DevelopersHub.RealtimeNetworking.Client;
using System;

public class hub_ui : MonoBehaviour
{
    public static hub_ui _instance = null; public static hub_ui instance { get { return _instance; } }
    
    [SerializeField] public Text levelText;
    [SerializeField] public Text xpText;
    [SerializeField] private GameObject xpBar;
    [SerializeField] public Text usernameText;
    [SerializeField] public InputField usernameUpdateField;
    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {

    }
    public void updateUI(int xp, int level)
    {
        float previousXP = 0;
        float n = 15;
        if (level > 1)
        {
            for (int i = 1; i < level - 1; i++)
            {
                n += 5 * (int)Math.Round((Math.Pow(i, 2.2f) + 15) / 5.0);
                previousXP = n;
            }
            n += 5 * (int)Math.Round((Math.Pow(level-1, 2.2f) + 15) / 5.0);
        }
        if (level == 2)
        {
            previousXP = 15;
        }
        while (xp >= n && level < 100)
        {
            addLevel();
            return;
        }
        xpBar.GetComponent<RectTransform>().sizeDelta = new Vector2(((xp-previousXP) / (n - previousXP)) * 487.5f, 50);
        xpText.text = (int)((((double)xp - previousXP) / (n-previousXP)) * 100) + "%";
    }

    public void ResetAccount()
    {
        Packet packet = new Packet();
        packet.Write((int)5);
        packet.Write(SystemInfo.deviceUniqueIdentifier);
        Sender.TCP_Send(packet);
    }
    public void UpdatePFP(int index)
    {

    }
    public void UpdateUsername()
    {
        Packet packet = new Packet();
        packet.Write((int)4);
        packet.Write(SystemInfo.deviceUniqueIdentifier);
        packet.Write(usernameUpdateField.text);
        Sender.TCP_Send(packet);
    }
    public void addXP(int amount)
    {
        Packet packet = new Packet();
        packet.Write((int)3);
        packet.Write(SystemInfo.deviceUniqueIdentifier);
        packet.Write("xp");
        packet.Write(amount);
        Sender.TCP_Send(packet);
    }
    public void addLevel()
    {
        Packet packet = new Packet();
        packet.Write((int)3);
        packet.Write(SystemInfo.deviceUniqueIdentifier);
        packet.Write("level");
        packet.Write(1);
        Sender.TCP_Send(packet);
    }
}
