using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class MenuController
{
    [Header("Online Lobby")]
    [SerializeField] private Button btnCreateRoom;
    [SerializeField] private Button btnFindRoom;
    [SerializeField] private TMP_Text connectionStatus;

    private void SetOnlineStatus(bool online, string status)
    {
        btnCreateRoom.interactable = online;
        btnFindRoom.interactable = online;
        connectionStatus.text = status;
    }

    public void UpdateNickName(TMP_InputField playerNameInput)
    {
        PhotonNetwork.NickName = playerNameInput.text.Trim();
    }

    public void ShowCreateRoom()
    {
        createRoomStatus.text = "";
        SetScreen(CreateRoomView);
    }

    public void ShowFindRoom()
    {
        SetScreen(GeneralLobbyView);
    }
}
