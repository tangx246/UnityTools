using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class to display helper buttons and status labels on the GUI, as well as buttons to start host/client/server.
/// </summary>
public class BootstrapManager : MonoBehaviour
{
    [Scene]
    public string gameScenePath;

    [Scene]
    public string titleScenePath;

    private void Start()
    {
        var networkManager = NetworkManager.Singleton;
        networkManager.OnServerStarted += OnServerStarted;
        networkManager.OnClientDisconnectCallback += OnClientDisconnected;
    }

    private void OnClientDisconnected(ulong clientId)
    {
        if (!NetworkManager.Singleton.IsServer)
        {
            SceneManager.LoadScene(titleScenePath, LoadSceneMode.Single);
        }
    }

    private void OnServerStarted()
    {
        NetworkManager.Singleton.SceneManager.LoadScene(gameScenePath, LoadSceneMode.Single);
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));

        var networkManager = NetworkManager.Singleton;

        if (!networkManager.IsClient && !networkManager.IsServer)
        {
            var transport = ((UnityTransport)networkManager.NetworkConfig.NetworkTransport);
            ref var connData = ref transport.ConnectionData;
            var ip = GUILayout.TextField(connData.Address);
            var port = GUILayout.TextField(connData.Port.ToString());
            transport.SetConnectionData(ip, ushort.Parse(port));

            if (GUILayout.Button("Host"))
            {
                transport.SetConnectionData("0.0.0.0", ushort.Parse(port));
                networkManager.StartHost();
            }

            if (GUILayout.Button("Client"))
            {
                networkManager.StartClient();
            }

            if (GUILayout.Button("Server"))
            {
                networkManager.StartServer();
            }
        }

        GUILayout.EndArea();
    }
}
