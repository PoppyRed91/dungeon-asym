using System;
using System.Collections.Generic;
using SocketIOClient;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;
    public static event Action OnConnect;
    public static event Action OnDisconnect;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public SocketIOUnity Socket;

    public async void Connect(string ip, int port, string name)
    {
        Socket = new SocketIOUnity(new Uri($"http://{ip}:{port}"), new SocketIOOptions
        {
            Query = new Dictionary<string, string>{
                    {"token", "UNITY" }
                },
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });

        Socket.unityThreadScope = SocketIOUnity.UnityThreadScope.Update;
        Debug.Log("Connecting to " + ip + ":" + port);

        Socket.OnConnected += async (sender, e) =>
        {
            Debug.Log("Connected to server!");
            await Socket.EmitAsync("userdata", name);
            await Socket.EmitAsync("msg", "Hello from unity!");

            Socket.On("msg", (data) => Debug.Log(data.ToString()));
        };

        await Socket.ConnectAsync();

        OnConnect?.Invoke();
    }

    public async void Disconnect()
    {
        await Socket.DisconnectAsync();

        OnDisconnect?.Invoke();
    }

    private void OnApplicationQuit()
    {
        if (Socket != null) Disconnect();
    }
}
