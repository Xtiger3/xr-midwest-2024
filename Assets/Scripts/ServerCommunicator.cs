using System.Collections;
using UnityEngine;
using SocketIOClient;

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
            EIO = 4,  // Engine.IO protocol version 4
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });

        // Handle the connection event
        socket.On("connect", (response) =>
        {
            Debug.Log("Connected to the server");
        });

        // Listen for custom events from the server
        socket.On("assigned", (response) =>
        {
            role = response.GetValue<string>(0);
            currentHP = response.GetValue<int>(1);

            Debug.Log("Assigned role: " + role);
            Debug.Log("Assigned HP: " + currentHP);
        });

        socket.On("updateHP", (response) =>
        {
            currentHP = response.GetValue<int>();
            Debug.Log("HP updated: " + currentHP);
        });

        socket.On("gameStart", (response) =>
        {
            ongoingGame = true;
            Debug.Log("Game started");
        });

        // Time heartbeat
        socket.On("timeUpdate", (response) =>
        {
            timeLeft = response.GetValue<int>();
            Debug.Log("Time left: " + timeLeft);
        });

        // Listen for error events
        socket.On("error", (response) =>
        {
            string error = response.GetValue<string>();
            Debug.LogError("Error: " + error);
        });

        // Handle disconnection
        socket.On("disconnect", (response) =>
        {
            Debug.Log("Disconnected from the server");
        });

        // Listen for game finished event
        socket.On("gameFinished", (response) =>
        {
            result = response.GetValue<string>();
            ongoingGame = false;
            Debug.Log("Game finished: " + result);
        });

        // Establish the connection to the server
        socket.Connect();
    }

    // Send the "correctGesture" event to the server
    public void SendCorrectGesture()
    {
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
