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

    private List<RoomInfo> roomsInfo = new List<RoomInfo>();

    public Button btnUpdate;
    // public Button btnBack;

    GameObject CreateRoomButton(string _NameRoom, int _currentUsers, int _index)
    {
        GameObject go = new GameObject("Room - " + _NameRoom, typeof(RectTransform), typeof(Image), typeof(Button), typeof(RoomData));
        RectTransform rowTransform = go.GetComponent<RectTransform>();
        rowTransform.SetParent(roomListContainer, false);
        rowTransform.anchorMin = new Vector2(0, 1);
        rowTransform.anchorMax = new Vector2(1, 1);
        rowTransform.pivot = new Vector2(0.5f, 1);
        rowTransform.sizeDelta = new Vector2(0, 48);
        rowTransform.anchoredPosition = new Vector2(0, -_index * 52);

        Image background = go.GetComponent<Image>();
        background.sprite = Resources.GetBuiltinResource<Sprite>("UI/Skin/UISprite.psd");
        background.color = new Color(0.9f, 0.9f, 0.9f, 1);

        Button button = go.GetComponent<Button>();
        button.targetGraphic = background;

        TextMeshProUGUI roomName = CreateRoomLabel(go.transform, "Room Name", new Vector2(0, 0), new Vector2(0.7f, 1));
        roomName.alignment = TextAlignmentOptions.MidlineLeft;

        TextMeshProUGUI playerCount = CreateRoomLabel(go.transform, "Player Count", new Vector2(0.7f, 0), new Vector2(1, 1));
        playerCount.alignment = TextAlignmentOptions.MidlineRight;

        RoomData data = go.GetComponent<RoomData>();
        data.NameRoom = roomName;
        data.currentTotalPlayers = playerCount;
        data.joinButton = button;
        data.UpdateData(_NameRoom, _currentUsers);

        return go;
    }

    TextMeshProUGUI CreateRoomLabel(Transform parent, string objectName, Vector2 anchorMin, Vector2 anchorMax)
    {
        GameObject labelObject = new GameObject(objectName, typeof(RectTransform), typeof(TextMeshProUGUI));
        labelObject.transform.SetParent(parent, false);

        RectTransform labelTransform = labelObject.GetComponent<RectTransform>();
        labelTransform.anchorMin = anchorMin;
        labelTransform.anchorMax = anchorMax;
        labelTransform.offsetMin = new Vector2(8, 0);
        labelTransform.offsetMax = new Vector2(-8, 0);

        TextMeshProUGUI label = labelObject.GetComponent<TextMeshProUGUI>();
        label.font = TMP_Settings.defaultFontAsset;
        label.fontSize = 20;
        label.color = Color.black;
        label.raycastTarget = false;
        return label;
    }

    void updateLobbyBrowserUI()
    {
        foreach (Transform child in roomListContainer)
        {
            Destroy(child.gameObject);
        }

        for (int x = 0; x < roomsInfo.Count; x++)
        {
            CreateRoomButton(roomsInfo[x].Name, roomsInfo[x].PlayerCount, x);
        }
    }

    public void UpdateRoomList()
    {
        updateLobbyBrowserUI();
    }

    public override void OnRoomListUpdate(List<RoomInfo> Activeroom)
    {
        Debug.Log("Lista actualizada");
        foreach (RoomInfo room in Activeroom)
        {
            int existingIndex = roomsInfo.FindIndex(existingRoom => existingRoom.Name == room.Name);

            if (room.RemovedFromList)
            {
                if (existingIndex >= 0)
                {
                    roomsInfo.RemoveAt(existingIndex);
                }
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

        updateLobbyBrowserUI();
    }
}