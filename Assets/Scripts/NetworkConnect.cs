using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

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

    }
}
