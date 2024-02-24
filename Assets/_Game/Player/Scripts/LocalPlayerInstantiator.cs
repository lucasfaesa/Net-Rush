using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

public class LocalPlayerInstantiator : NetworkBehaviour
{
    [SerializeField] private GameObject OnlineLocalPlayerPrefab;
    [SerializeField] private PlayerStatsSO playerStats;
    [Header("Player Settings")]
    [SerializeField] private Vector3 playerLeftPos = new Vector3(0,0,5.49f);
    [SerializeField] private Vector3 playerRightPos = new Vector3(0,0, -5.49f);
    
    // Start is called before the first frame update
    public override void Spawned()
    {
        GameObject player = Instantiate(OnlineLocalPlayerPrefab);
        
        if (Runner.ActivePlayers.Count() <= 1)
        {
            playerStats.PlayerSide = PlayerStatsSO.PlayerSideEnum.LeftSide;
            player.transform.SetPositionAndRotation(playerLeftPos, Quaternion.Euler(0,-180,0));
        }
        else
        {
            playerStats.PlayerSide = PlayerStatsSO.PlayerSideEnum.RightSide;
            player.transform.SetPositionAndRotation(playerRightPos, Quaternion.Euler(Vector3.zero));
        }
        
        player.gameObject.SetActive(true);
    }
    
}
