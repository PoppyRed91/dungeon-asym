using System;
using System.Collections.Generic;
using SocketIOClient;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;
    public string _ipAddress;
    public Uri _uri;
    private SocketIOUnity _socket;
    private readonly SocketIOOptions _socketIOOptions = new()
    {
        Query = new Dictionary<string, string> {
            {"token", "UNITY"}
        },
        Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
    };

    private void Awake()
    {
        if (Instance == null) Instance = this;
        DontDestroyOnLoad(gameObject);
        _uri = new Uri($"https://{_ipAddress}");
        _socket = new SocketIOUnity(_uri, _socketIOOptions);
        _socket.OnConnected += OnConnected;
        _socket.OnDisconnected += OnDisconnected;
        _socket.OnError += OnErrorHandle;
    }

    private void OnErrorHandle(object sender, string e)
    {
        Debug.Log("Error " + e);
    }

    private void OnDisconnected(object sender, string e)
    {
        Debug.Log($"Disconnected from {sender}");
    }

    private void OnConnected(object sender, EventArgs e)
    {
        Debug.Log($"Connected to {sender}");
    }

    public void SetUri(string uri)
    {

    }

    public void Connect()
    {

        _socket.ConnectAsync();
    }

    public void Disconnect()
    {
        _socket.DisconnectAsync();
    }


    private void OnDisconnected()
    {
        Debug.Log("Disconnected!");
    }

    private void OnApplicationQuit()
    {
        if (_socket != null) Disconnect();
    }
}
