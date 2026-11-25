using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomData : MonoBehaviour
{
    [SerializeField] private TMP_Text roomName;
    [SerializeField] private TMP_Text playerCount;
    [SerializeField] private Button joinButton;

    public void UpdateData(RoomInfo room)
    {
        roomName.text = room.Name;
        playerCount.text = room.MaxPlayers == 0
            ? room.PlayerCount.ToString()
            : $"{room.PlayerCount} / {room.MaxPlayers}";
        joinButton.interactable = room.IsOpen && (room.MaxPlayers == 0 || room.PlayerCount < room.MaxPlayers);
        joinButton.onClick.AddListener(() => NetworkManager.instance.JoinRoom(room.Name));
    }
}
