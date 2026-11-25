using System.Text;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class MenuController
{
    [Header("Room Lobby")]
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text roomName;
    [SerializeField] private TMP_Text roomLobbyStatus;
    [SerializeField] private Button startButton;

    public override void OnJoinedRoom()
    {
        UpdatePlayerList();
        SetScreen(LobbyRoomView);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        UpdatePlayerList();
    }

    private void UpdatePlayerList()
    {
        startButton.interactable = PhotonNetwork.IsMasterClient;
        roomLobbyStatus.text = PhotonNetwork.IsMasterClient
            ? "You are the host."
            : "Waiting for the host to start.";

        var roster = new StringBuilder();
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (roster.Length > 0)
                roster.AppendLine();

            string displayName = string.IsNullOrWhiteSpace(player.NickName)
                ? "Player " + player.ActorNumber
                : player.NickName.Replace('\r', ' ').Replace('\n', ' ');
            roster.Append(displayName);
            if (player.IsMasterClient)
                roster.Append(" (Host)");
        }

        playerName.text = roster.ToString();
        roomName.text = PhotonNetwork.CurrentRoom.Name;
    }

    public void StartGame()
    {
        if (!PhotonNetwork.InRoom || !PhotonNetwork.IsMasterClient)
            return;

        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        NetworkManager.instance.photonView.RPC("ChangeScene", RpcTarget.All, "GameScene");
    }

    public void LeaveCurrentLobby()
    {
        if (PhotonNetwork.LeaveRoom())
        {
            SetOnlineStatus(false, "Rejoining the lobby...");
            BackMainView();
        }
    }
}
