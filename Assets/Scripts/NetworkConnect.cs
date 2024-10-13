using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class NetworkConnect : MonoBehaviour
{

    public void Create()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void Join()
    {
        NetworkManager.Singleton.StartClient();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Create();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Join();
        }
    }

    private void Start()
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData("192.168.0.1", (ushort)7777, "0.0.0.0");
    }
}
