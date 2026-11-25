using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public partial class MenuController : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    [Header("Views")]
    public GameObject MainView;
    public GameObject CreateRoomView;
    public GameObject LobbyRoomView;
    public GameObject GeneralLobbyView;

    void Start()
    {
        btnCreateRoom.interactable = false;
        btnFindRoom.interactable = false;

        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.CurrentRoom.IsVisible = true;
            PhotonNetwork.CurrentRoom.IsOpen = true;
        }
    }

    public override void OnConnectedToMaster()
    {
        //base.OnConnectedToMaster();
        btnCreateRoom.interactable = true;
        btnFindRoom.interactable = true;
    }

    void SetScreen(GameObject _screen)
    {
        LobbyRoomView.SetActive(false);
        CreateRoomView.SetActive(false);
        GeneralLobbyView.SetActive(false);
        MainView.SetActive(false);

        _screen.SetActive(true);

        if (_screen == GeneralLobbyView)
        {
            updateLobbyBrowserUI();
        }
    }

    public void BackMainView()
    {
        SetScreen(MainView);
    }

    
}