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
    private SocketIOUnity _socket;



    public async void Connect(string ip, int port, string name)
    {
        _socket = new SocketIOUnity(new Uri($"http://{ip}:{port}"), new SocketIOOptions
        {
            Query = new Dictionary<string, string>{
                    {"token", "UNITY" }
                },
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });

        _socket.unityThreadScope = SocketIOUnity.UnityThreadScope.Update;
        Debug.Log("Connecting to " + ip + ":" + port);

        _socket.OnConnected += async (sender, e) =>
        {
            Debug.Log("Connected to server!");
            await _socket.EmitAsync("userdata", name);
            await _socket.EmitAsync("msg", "Hello from unity!");
        };

        await _socket.ConnectAsync();

        OnConnect?.Invoke();
    }

    public async void Disconnect()
    {
        await _socket.DisconnectAsync();

        OnDisconnect?.Invoke();
    }

    private void OnApplicationQuit()
    {
        if (_socket != null) Disconnect();
    }


}
