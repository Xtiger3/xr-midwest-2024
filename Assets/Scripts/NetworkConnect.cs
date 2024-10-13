using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkConnect : MonoBehaviour
{
    public string serverUrl;
    public ServerCommunicator server;
    public GameObject startPanel;
    //public GameObject waitPanel;

    public void Create()
    {
        NetworkManager.Singleton.StartHost();
        server.Connect(serverUrl);
        startPanel.SetActive(false);
    }

    public void Join()
    {
        NetworkManager.Singleton.StartClient();
        server.Connect(serverUrl);
        startPanel.SetActive(false);
    }

}
