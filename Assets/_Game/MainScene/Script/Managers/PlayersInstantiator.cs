using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersInstantiator : MonoBehaviour
{
    [SerializeField] private PlayerController playerLeftPrefab;
    [SerializeField] private PlayerController playerRightPrefab;
    [Header("Player Settings")]
    [SerializeField] private Vector3 playerLeftPos = new Vector3(0,0,5.49f);
    [SerializeField] private Vector3 playerRightPos = new Vector3(0,0, -5.49f);
    
    public PlayerController LeftPlayer { get; private set; }
    public PlayerController RightPlayer { get; private set; }
    
    public void InstantiatePlayers(ref PlayerController playerLeft, ref PlayerController playerRight)
    {
        LeftPlayer = Instantiate(playerLeftPrefab, playerLeftPos, Quaternion.identity);
        RightPlayer = Instantiate(playerRightPrefab, playerRightPos, Quaternion.identity);

        playerLeft = LeftPlayer;
        playerRight = RightPlayer;
        
        playerLeft.gameObject.name = "PlayerLeft";
        playerRight.gameObject.name = "PlayerRight";
        
        playerLeft.transform.position = playerLeftPos;
        playerLeft.transform.rotation = Quaternion.Euler(0,-180,0);
        
        playerRight.transform.position = playerRightPos;
        
        playerLeft.gameObject.SetActive(true);
        playerRight.gameObject.SetActive(true);
    }
}
