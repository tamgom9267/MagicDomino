using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomData : MonoBehaviour
{
    public TextMeshProUGUI NameRoom;
    public TextMeshProUGUI currentTotalPlayers;

    public Button joinButton;
    
    public void UpdateData(string _RoomName, int _CurrentPlayers)
    {
        NameRoom.text = _RoomName;
        currentTotalPlayers.text = _CurrentPlayers.ToString() + "/" + NetworkManager.instance.maxPlayer.ToString();

        AddListenerToButton(_RoomName);
    }

    public void AddListenerToButton(string _RoomName)
    {
        joinButton.onClick.RemoveAllListeners();
        joinButton.onClick.AddListener(() => { JoinRoomOnClick(_RoomName); });
    }

    public void JoinRoomOnClick(string _roomName)
    {
        NetworkManager.instance.JoinRoom(_roomName);
    }
}
