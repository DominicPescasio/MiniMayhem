using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class TestLobby : MonoBehaviour
{


    private Lobby mainLobby;
    private float heartbeatTimer;
    // Start
    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in" + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void Update()
    {
        HandleLobbyHeartbeat();
    }
    private async void HandleLobbyHeartbeat()
    {
        if (mainLobby != null) {
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer < 0f)
            {

                float heartbeatTimerMax = 15;
                    heartbeatTimer = heartbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(mainLobby.Id);
            }
                }
    }

    public async void CreateLobby()
    {
        try
        {
            string lobbyName = "Ny Lobby";
            int maxPLayers = 4;
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPLayers);

            mainLobby = lobby;

            Debug.Log("Created Lobby!" + lobby.Name + " " + lobby.MaxPlayers);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }




    public async void listLobbies()
    {
        try
        {
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            Debug.Log("Lobbies Found: " + queryResponse.Results.Count);
            foreach (Lobby lobby in queryResponse.Results)
            {
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers);
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }






   
    }

