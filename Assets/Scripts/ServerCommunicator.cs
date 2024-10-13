using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class ServerCommunicator : MonoBehaviour
{
    private SocketIOUnity socket;  // Declare the Socket.IO socket

    // Public variables for storing user state
    public int currentHP;
    public bool ongoingGame;
    public int timeLeft;
    public string role;  // 'player1' or 'player2'
    public string result;  // Game result (victory, loss, etc.)

    // Connect to the Socket.IO server
    public void Connect(string serverUrl)
    {
        var uri = new System.Uri(serverUrl);
        socket = new SocketIOUnity(uri, new SocketIOOptions
        {
            Query = new Dictionary<string, string>
                {
                    {"token", "UNITY" }
                }
            ,
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });

        socket.JsonSerializer = new NewtonsoftJsonSerializer();

        socket.Connect();

        socket.OnAnyInUnityThread((name, response) =>
        {

            Debug.Log($"Received event '{name}' with data: {response.ToString()}");

            if (name == "connect")
            {
                Debug.Log("Connected to the server1");
            }

            if(name == "assigned")
            {
                var assignment = response.GetValue<PlayerAssignment>();

                // Set the role and currentHP based on the deserialized data
                role = assignment.Player;
                currentHP = assignment.HP;

                Debug.Log("Assigned role: " + role);
                Debug.Log("Assigned HP: " + currentHP);
            }

            if(name == "updateHP")
            {
                var hpUpdates = response.GetValue<HPUpdateData>();
                currentHP = hpUpdates.HP;
                GameManagerMultiplayer.Instance.health = currentHP;
                Debug.Log("HP updated: " + currentHP);
            }

            if(name == "gameStart")
            {
                ongoingGame = true;
                Debug.Log("Game started");
                GameManagerMultiplayer.Instance.StartGame();

            }

            if(name == "timeUpdate")
            {
                var timeUpdates = response.GetValue<TimeUpdateData>();
                timeLeft = timeUpdates.TimeLeft;
                GameManagerMultiplayer.Instance.UpdateTimer(timeLeft);
                Debug.Log("Time left: " + timeLeft);
            }

            if(name == "error")
            {
                string error = response.GetValue<string>();
                Debug.LogError("Error: " + error);
            }

            if(name == "disconnect")
            {
                Debug.Log("Disconnected from the server");
            }

            if(name == "gameFinished")
            {
                var gameResults = response.GetValue<GameFinishedData>();

                result = gameResults.Result;
                ongoingGame = false;
                GameManagerMultiplayer.Instance.ShowScore(result);
                Debug.Log("Game finished: " + result);
             }
        });

    }

    // Send the "correctGesture" event to the server
    public void SendCorrectGesture()
    {
        Debug.Log("SendGesture");
        Debug.Log(socket != null);
        Debug.Log(socket.Connected);
        if (socket != null && socket.Connected)
        {
            socket.Emit("correctGesture");
            Debug.Log("Sent correct gesture to the server");
        }
        else
        {
            Debug.LogError("Not connected to the server");
        }
    }

    // Simulate sending the "restartGame" event
    public void RestartGame()
    {
        if (socket != null && socket.Connected)
        {
            socket.Emit("restartGame");
            ongoingGame = true;
            Debug.Log("Restarting game...");
        }
        else
        {
            Debug.LogError("Not connected to the server");
        }
    }

    // Optionally, implement a disconnect function to manually disconnect
    public void Disconnect()
    {
        if (socket != null)
        {
            socket.Disconnect();
            Debug.Log("Disconnected from the server.");
        }
    }

    // Make sure to clean up when the application is closed
    void OnApplicationQuit()
    {
        Disconnect();
    }
}


class PlayerAssignment
{
    public string Player { get; set; }
    public int HP { get; set; }
}

class TimeUpdateData
{
    public int TimeLeft { get; set; }
}

class GameFinishedData
{
    public string Result { get; set; } 
}

class HPUpdateData
{
    public int HP { get; set; }
}