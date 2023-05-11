    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using DevelopersHub.RealtimeNetworking.Client;
    public class Player : MonoBehaviour
    {
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    public enum RequestsID
        {
            AUTH = 1, SYNC = 2
        }

        private void Start()
        {
            RealtimeNetworking.OnLongReceived += ReceivedLong;
            RealtimeNetworking.OnPacketReceived += ReceivedPacket;
            ConnectToServer();
        }
        private void ReceivedLong(int id, long value)
        {
            switch (id)
            {
                case 1: //response to authentication
                    Sender.TCP_Send((int)RequestsID.SYNC, SystemInfo.deviceUniqueIdentifier);
                    break;
            }
        }
        private void ReceivedPacket(Packet packet)
        {
        int id = packet.ReadInt();    
        switch (id)
            {
                case 2: //response to sync
                string playerClass = packet.ReadString();
                Data.Player player = Data.Deserialize<Data.Player>(playerClass);
                if (hub_ui.instance != null)
                {
                    hub_ui.instance.levelText.text = player.level.ToString();
                    hub_ui.instance.usernameText.text = player.username;
                    hub_ui.instance.UpdatePFP(player.pfp);
                    hub_ui.instance.updateUI(player.xp, player.level);
                }
                if (Portraits.instance != null)
                {
                    Portraits.instance.characters = player.characters;
                    Portraits.instance.standardInstantiation();
                }
                break;
            }
        }
        private void ConnectionResponse(bool successful)
        {
            if (successful)
            {
                RealtimeNetworking.OnDisconnectedFromServer += DisconnectedFromServer;
                string device = SystemInfo.deviceUniqueIdentifier;
                Sender.TCP_Send((int)RequestsID.AUTH, device);
            }
            else
            {
                //TODO make a button that pops up to try and connect to server again
            }
            RealtimeNetworking.OnConnectingToServerResult -= ConnectionResponse;
        }
        public void ConnectToServer()
        {
            RealtimeNetworking.OnConnectingToServerResult += ConnectionResponse;
            RealtimeNetworking.Connect();
        }
        private void DisconnectedFromServer()
        {
            RealtimeNetworking.OnDisconnectedFromServer -= DisconnectedFromServer;
            //TODO make a button that pops up to try and connect to server again
        }
    }
