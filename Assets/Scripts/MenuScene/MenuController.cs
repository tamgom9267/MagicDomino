using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public partial class MenuController : MonoBehaviourPunCallbacks
{
    [Header("Views")]
    [SerializeField] private GameObject titleView;
    [SerializeField] private GameObject howToPlayView;
    [SerializeField] private GameObject MainView;
    [SerializeField] private GameObject CreateRoomView;
    [SerializeField] private GameObject LobbyRoomView;
    [SerializeField] private GameObject GeneralLobbyView;

    private GameObject activeView;

    private void Start()
    {
        bool online = PhotonNetwork.InLobby;
        SetOnlineStatus(online, online ? "Online" : "Connecting...");

        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.CurrentRoom.IsVisible = true;
            PhotonNetwork.CurrentRoom.IsOpen = true;
            OnJoinedRoom();
            return;
        }

        SetScreen(titleView);
    }

    public override void OnJoinedLobby()
    {
        roomsInfo.Clear();
        SetOnlineStatus(true, "Online");
        UpdateRoomList();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        roomsInfo.Clear();
        SetOnlineStatus(false, "Disconnected");

        if (activeView == CreateRoomView || activeView == GeneralLobbyView || activeView == LobbyRoomView)
            BackMainView();
    }

    private void SetScreen(GameObject screen)
    {
        titleView.SetActive(screen == titleView);
        howToPlayView.SetActive(screen == howToPlayView);
        MainView.SetActive(screen == MainView);
        CreateRoomView.SetActive(screen == CreateRoomView);
        LobbyRoomView.SetActive(screen == LobbyRoomView);
        GeneralLobbyView.SetActive(screen == GeneralLobbyView);
        activeView = screen;

        if (screen == GeneralLobbyView)
            UpdateRoomList();

        if (EventSystem.current != null)
        {
            foreach (Button button in screen.GetComponentsInChildren<Button>())
            {
                if (!button.interactable)
                    continue;

                EventSystem.current.SetSelectedGameObject(button.gameObject);
                break;
            }
        }
    }

    public void ShowTitle()
    {
        SetScreen(titleView);
    }

    public void ShowHowToPlay()
    {
        SetScreen(howToPlayView);
    }

    public void BackMainView()
    {
        SetScreen(MainView);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
