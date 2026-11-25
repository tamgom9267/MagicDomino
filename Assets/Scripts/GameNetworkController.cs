/*
    GameNetworkController.cs

    This script handles the synchronization between the players and the game during online

*/
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameNetworkController : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameController gameController;

    private void Start()
    {
        if (!PhotonNetwork.InRoom)
        {
            Debug.LogWarning("Not connected to a Photon room.");
            return;
        }

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            gameController.RegisterNetworkPlayer(PhotonNetwork.LocalPlayer.ActorNumber);

            Debug.Log($"Registered player: {player.ActorNumber}");
        }

        Debug.Log(
            $"Local Actor: {PhotonNetwork.LocalPlayer.ActorNumber} " +
            $"| Is MasterClient: {PhotonNetwork.IsMasterClient}" + 
            $"| Master Client: {PhotonNetwork.MasterClient.ActorNumber}"
        );
    }

    
}
