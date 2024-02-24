using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{
    [Header("SO's")]
    [SerializeField] private NetworkRunnerCallbacksSO networkCallbacks;
    [Space] 
    [Header("References")]
    [SerializeField] private NetworkRunner runner;
    [SerializeField] private NetworkPlayer networkPlayerReference;
    [SerializeField] private NetworkObject onlinePuppetRigPrefab;
    [Space] 
    [Header("Debug")]
    [SerializeField] private bool connectToARoomOnStart;
    
    public static NetworkManager Instance { get; private set; }

    public NetworkRunner GetRunner => runner;
    
    public Dictionary<PlayerRef, NetworkPlayer> NetworkPlayers { get; set; } = new();

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
        
    }

    private void Start()
    {
        if(connectToARoomOnStart)
            ConnectGame();
    }

    public async void ConnectGame()
    {
        var args = new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = "Test",
            Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex),
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        };

        var connectionResult = await runner.StartGame(args);
        
        if(connectionResult.Ok)
            Debug.Log("StartGame Successful");
        else
            Debug.LogError($"Connection Error:{connectionResult.ErrorMessage}");
    }

    //important to remember that this doesnt get all players on the game, just the players that entered
    //AFTER this player opened the game
    public void AddPlayer(PlayerRef pRef, NetworkPlayer nPlay)
    {
        NetworkPlayers[pRef] = nPlay;
    }

    public NetworkPlayer GetPlayer(PlayerRef player = default)
    {
        if (player == default)
            player = runner.LocalPlayer;

        NetworkPlayers.TryGetValue(player, out NetworkPlayer networkPlayer);
        
        return networkPlayer;
    }
    
    
    public void RemovePlayer(PlayerRef player)
    {
        if (NetworkPlayers.ContainsKey(player))
            NetworkPlayers.Remove(player);
        else
            Debug.LogError($"The player {player} was not found!");
    }

    private void SpawnPlayer(NetworkRunner runner, PlayerRef player)
    {
        //the networkPlayerReference is not the localplayer, its important to remember that, it just acts serves to indicate that the player spanwed on the server
        //the local player is instantiated on another script, the "LocalPlayerInstantiator
        
        if (player == runner.LocalPlayer)
        {
            runner.Spawn(networkPlayerReference, Vector3.zero, Quaternion.identity, player);
            runner.Spawn(onlinePuppetRigPrefab, Vector3.zero, quaternion.identity, player);
        }
    }
    

#region NetworkRunnerCallbacks
    
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        networkCallbacks.OnObjectExitAOI(runner, obj, player);
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        networkCallbacks.OnObjectEnterAOI(runner, obj, player);
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        networkCallbacks.OnPlayerJoined(runner, player);
        
        SpawnPlayer(runner, player);
        
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        networkCallbacks.OnPlayerLeft(runner, player);
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        networkCallbacks.OnInput(runner, input);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        networkCallbacks.OnInputMissing(runner, player, input);
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        networkCallbacks.OnShutdown(runner, shutdownReason);
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        networkCallbacks.OnConnectedToServer(runner);
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        networkCallbacks.OnDisconnectedFromServer(runner, reason);
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        networkCallbacks.OnConnectRequest(runner, request, token);
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        networkCallbacks.OnConnectFailed(runner, remoteAddress, reason);
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        networkCallbacks.OnUserSimulationMessage(runner, message);
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        networkCallbacks.OnSessionListUpdated(runner, sessionList);
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        networkCallbacks.OnCustomAuthenticationResponse(runner, data);
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        networkCallbacks.OnHostMigration(runner, hostMigrationToken);
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        networkCallbacks.OnReliableDataReceived(runner, player, key, data);
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        networkCallbacks.OnReliableDataProgress(runner, player, key, progress);
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        networkCallbacks.OnSceneLoadDone(runner);
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        networkCallbacks.OnSceneLoadStart(runner);
    }
    
#endregion
     
     
     
}
