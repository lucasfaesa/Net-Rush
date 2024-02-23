using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField] private NetworkPlayerCallbacksSO networkPlayerCallbacks;
    
    public override void Spawned()
    {
        base.Spawned();
        
        NetworkManager.Instance.AddPlayer(Runner.LocalPlayer, this);
        
        networkPlayerCallbacks.OnPlayerSpawn(Runner, Runner.LocalPlayer);
        
        Debug.Log("<color=red>PLAYER SPAWNED</color>");
        
        Debug.Log($"Runner {Runner}", Runner);
        Debug.Log($"Runner {Runner.LocalPlayer}");
    }
}
