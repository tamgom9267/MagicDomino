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
    [Header("GeneralRooms")]

    public Transform roomListContainer;

    public RoomData roomPrefab;
    private List<GameObject> roomButtons = new List<GameObject>();

    private List<RoomInfo> roomsInfo = new List<RoomInfo>();

    public Button btnUpdate;
    // public Button btnBack;

    GameObject CreateRoomButton(string _NameRoom, int _currentUsers)
    {
        GameObject go = Instantiate(roomPrefab.gameObject, roomListContainer);
        go.GetComponent<RoomData>().UpdateData(_NameRoom, _currentUsers);

        roomButtons.Add(go);
        return go;
    }

    void updateLobbyBrowserUI()
    {
        foreach (Transform child in roomListContainer)
        {
            Destroy(child.gameObject);
        }

        for (int x = 0; x < roomsInfo.Count; x++)
        {
            CreateRoomButton(roomsInfo[x].Name, roomsInfo[x].PlayerCount);
        }
    }

    public void UpdateRoomList()
    {
        roomButtons.Clear();

        updateLobbyBrowserUI();
    }

    public override void OnRoomListUpdate(List<RoomInfo> Activeroom)
    {
        Debug.Log("Lista actualizada");
        roomsInfo = Activeroom;
    }
}