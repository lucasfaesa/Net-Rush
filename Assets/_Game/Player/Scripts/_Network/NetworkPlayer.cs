using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField] private NetworkPlayerCallbacksSO networkPlayerCallbacks;
    [Space]
    [SerializeField] private NetworkObject onlinePuppetPlayerPrefab;

    private PuppetPlayerRig instantiatedPuppet;
    
    public override void Spawned()
    {
        base.Spawned();
        
        NetworkManager.Instance.AddPlayer(Runner.LocalPlayer, this);
            
        networkPlayerCallbacks.OnPlayerSpawn(Runner, Runner.LocalPlayer);
    }
}
