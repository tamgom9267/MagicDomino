using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public partial class MenuController : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    [Header("Main Screen")]
    public Button btnCreateRoom;
    public Button btnFindRoom;

    public void UpdateNickName(TMP_InputField _playerName)
    {
        PhotonNetwork.NickName = _playerName.text;
    }

    public void ShowCreateRoom()
    {
        SetScreen(CreateRoomView);
    }

    public void ShowFindRoom()
    {
        SetScreen(GeneralLobbyView);
    }
}
