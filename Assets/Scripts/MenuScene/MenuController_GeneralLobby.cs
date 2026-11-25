using System.Collections.Generic;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public partial class MenuController
{
    [Header("Find a Room")]
    [SerializeField] private RectTransform roomListContainer;
    [SerializeField] private RoomData roomRowTemplate;
    [SerializeField] private TMP_Text roomListStatus;

    private readonly List<RoomInfo> roomsInfo = new();

    public void UpdateRoomList()
    {
        if (activeView != GeneralLobbyView)
            return;

        foreach (Transform child in roomListContainer)
        {
            child.gameObject.SetActive(false);
            Destroy(child.gameObject);
        }

        foreach (RoomInfo room in roomsInfo)
        {
            RoomData row = Instantiate(roomRowTemplate, roomListContainer);
            row.UpdateData(room);
            row.gameObject.SetActive(true);
        }

        roomListStatus.text = roomsInfo.Count == 0 ? "No rooms available. Create one to get started." : "";
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo room in roomList)
        {
            int existingIndex = roomsInfo.FindIndex(existingRoom => existingRoom.Name == room.Name);

            if (room.RemovedFromList)
            {
                if (existingIndex >= 0)
                    roomsInfo.RemoveAt(existingIndex);
            }
            else if (existingIndex >= 0)
            {
                roomsInfo[existingIndex] = room;
            }
            else
            {
                roomsInfo.Add(room);
            }
        }

        UpdateRoomList();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        roomListStatus.text = "Could not join that room. Choose another or refresh the list.";
    }
}
