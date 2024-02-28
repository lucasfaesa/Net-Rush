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
    
    public void InstantiatePlayers(ref PlayerController playerLeft, ref PlayerController playerRight)
    {
        playerLeft = Instantiate(playerLeftPrefab, playerLeftPos, Quaternion.identity);
        playerRight = Instantiate(playerRightPrefab, playerRightPos, Quaternion.identity);

        playerLeft.gameObject.name = "PlayerLeft";
        playerRight.gameObject.name = "PlayerRight";
        
        playerLeft.transform.position = playerLeftPos;
        playerLeft.transform.rotation = Quaternion.Euler(0,-180,0);
        
        playerRight.transform.position = playerRightPos;
        
        playerLeft.gameObject.SetActive(true);
        playerRight.gameObject.SetActive(true);
    }
}
