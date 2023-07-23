
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine.SceneManagement;

using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using System.Runtime.CompilerServices;

public class Lobbie : MonoBehaviour
{
    private Lobby HostLobby;
    private float HeartbeatTimer =10f;
    [SerializeField] TMP_InputField inputField;

    // Start is called before the first frame update
    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        //await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void CreateLobby()
    {
        try
        {
            string lobbiename = "MyLobbie";
            int maxPlayer = 7;
            CreateLobbyOptions createLobbieOptions = new CreateLobbyOptions
            {
                IsPrivate = true,
            };
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbiename, maxPlayer, createLobbieOptions);
            HostLobby= lobby;
            Debug.Log("Created lobby " + lobby.Name + " " + lobby.MaxPlayers+ " "+ lobby.Id+" "+lobby.LobbyCode);
            SceneManager.LoadScene("game");
        }catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
        
    }
    private void Update()
    {
        HandleLobbyHeartBeat();
    }
    private async void HandleLobbyHeartBeat()
    {
        if (HostLobby != null)
        {
            //Debug.Log(HeartbeatTimer);
            HeartbeatTimer -= Time.deltaTime;
            if(HeartbeatTimer < 0f) {
                float HeartBeatTimerMax = 15f;
                HeartbeatTimer= HeartBeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(HostLobby.Id);
                Debug.Log("send this");
            }
        }
    }
    public async void ListLobby()
    {
        try
        {
            QueryResponse queryResponce = await Lobbies.Instance.QueryLobbiesAsync();
            Debug.Log("Lobbies found " + queryResponce.Results.Count);
            foreach (Lobby lobby in queryResponce.Results)
            {
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers);
            }

        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    public async void JoinLobbie()
    {
        
        try
        {
            //QueryResponse queryResponce = await Lobbies.Instance.QueryLobbiesAsync();

            string lobbycode = inputField.text;
            await Lobbies.Instance.JoinLobbyByCodeAsync(lobbycode);
            Debug.Log( "Join lobby with code " + lobbycode);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
}
