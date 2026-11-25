using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public partial class MenuController : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    public void CreateNewRoom(TMP_InputField _RoomName)
    {
        NetworkManager.instance.CreateRoom(_RoomName.text);
    }
}
