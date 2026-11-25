/*
    GameNetworkController.cs

    This script handles the synchronization between the players and the game during online

*/
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using ExitGames.Client.Photon;

public class GameNetworkController : MonoBehaviourPunCallbacks, IOnEventCallback
{
    [SerializeField] private GameController gameController;

    private const byte PlayerReadyCode = 1;
    private const byte GameReadyCode = 2;

    private readonly HashSet<int> readyPlayers = new();
    private bool gameReady = false;

    private void Start()
    {
        if (!PhotonNetwork.InRoom)
        {
            Debug.LogWarning("Not connected to a Photon room.");
            return;
        }

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            gameController.RegisterNetworkPlayer(player.ActorNumber); // Registrar jugadores

        }

        gameController.SetLocalPlayer(PhotonNetwork.LocalPlayer.ActorNumber);

        if (PhotonNetwork.IsMasterClient)
        {
            RegisterReadyPlayer(PhotonNetwork.LocalPlayer.ActorNumber);
        }
        else
        {
            PhotonNetwork.RaiseEvent(
                PlayerReadyCode,
                null,
                new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.MasterClient
                },
                SendOptions.SendReliable
            );
        }

        Debug.Log(
            $"Local Actor: {PhotonNetwork.LocalPlayer.ActorNumber} " +
            $"| Is MasterClient: {PhotonNetwork.IsMasterClient}" + 
            $"| Master Client: {PhotonNetwork.MasterClient.ActorNumber}"
        );
    }

    public void OnEvent(EventData eventData)
    {
        if (eventData.Code == PlayerReadyCode)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            RegisterReadyPlayer(eventData.Sender);
        }
        else if (eventData.Code == GameReadyCode)
        {
            if (eventData.Sender != PhotonNetwork.MasterClient.ActorNumber)
                return;

            MarkGameReady();
        }
    }

    private void RegisterReadyPlayer(int actorNumber)
    {
        if (!PhotonNetwork.IsMasterClient || gameReady) 
            return;

        if (!PhotonNetwork.CurrentRoom.Players.ContainsKey(actorNumber))
            return;

        if (!readyPlayers.Add(actorNumber))
            return;

        Debug.Log($"Player {actorNumber} is ready to play");

        if (PhotonNetwork.PlayerList.Length == 2 && readyPlayers.Count == 2)
        {
            MarkGameReady();

            PhotonNetwork.RaiseEvent(
                GameReadyCode,
                null,
                new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.Others
                },
                SendOptions.SendReliable
            );
        }
    }

    private void MarkGameReady()
    {
        if (gameReady)
            return;

        gameReady = true;
        Debug.Log("Both players are ready!");
    }

    
}
