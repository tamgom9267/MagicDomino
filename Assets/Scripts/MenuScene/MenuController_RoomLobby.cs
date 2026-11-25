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

        photonView.RPC("UpdatePlayerList", RpcTarget.All);
        //UpdateLobbyUI();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    [PunRPC]
    public void UpdatePlayerList()
    {
        startButton.interactable = PhotonNetwork.IsMasterClient;

        playerName.text = "";

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerName.text += player.NickName + "\n";
        }
        roomName.text = PhotonNetwork.CurrentRoom.Name.ToString();
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
