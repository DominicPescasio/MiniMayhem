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

        CheatConsole.OnOpen.AddListener(() => Debug.Log("Opened"));
        CheatConsole.OnClose.AddListener(() => Debug.Log("Closed"));

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
    [Cheat]
    public async void CreateLobby()
    {
        try
        {
            string lobbyName = "Ny Lobby";
            int maxPLayers = 4;
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = true,
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPLayers);

            mainLobby = lobby;

            Debug.Log("Created Lobby!" + lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Id + " " + lobby.LobbyCode);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }



    [Cheat]
    public static async void listLobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Count = 25,
                Filters = new List<QueryFilter>
                {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
            },
                Order = new List<QueryOrder>
                {
new QueryOrder(false, QueryOrder.FieldOptions.Created)
                }
            };
            
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
    [Cheat]
    private async void JoinLobby(string lobbyCode)
    {
        try
        {
           await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode);

            Debug.Log("joined Lobby with code " + lobbyCode);
        }catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }





   
    }

