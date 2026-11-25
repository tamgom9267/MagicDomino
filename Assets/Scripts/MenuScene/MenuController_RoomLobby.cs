using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Animations;

public partial class MenuController : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    [Header("Lobby Room")]
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI roomName;
    public Button startButton;

    public override void OnJoinedRoom()
    {
        //base.OnJoinedRoom();
        Debug.Log("Joined Room");

        SetScreen(LobbyRoomView);
        UpdatePlayerList();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    public void UpdatePlayerList()
    {
        startButton.interactable = PhotonNetwork.IsMasterClient;

        playerName.text = "";

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            string displayName = string.IsNullOrWhiteSpace(player.NickName)
                ? "Player " + player.ActorNumber
                : player.NickName;
            playerName.text += displayName + "\n";
        }
        roomName.text = PhotonNetwork.CurrentRoom.Name;
    }

    public void StartGame()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        // tell everyone to load the game scene
        NetworkManager.instance.photonView.RPC("ChangeScene", RpcTarget.All, "GameScene");
    }

    public void LeaveCurrentLobby()
    {
        PhotonNetwork.LeaveRoom();
        SetScreen(MainView);
    }
}
