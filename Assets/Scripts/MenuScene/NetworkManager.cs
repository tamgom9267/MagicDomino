using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public int maxPlayer = 2;

    public static NetworkManager instance;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected");
        PhotonNetwork.JoinLobby();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room");
    }

    public bool CreateRoom(string _RoomName)
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = (byte)maxPlayer;

        return PhotonNetwork.CreateRoom(_RoomName, options);
    }

    public void JoinRoom(string _RoomName)
    {
        PhotonNetwork.JoinRoom(_RoomName);
    }

    [PunRPC]
    public void ChangeScene(string _sceneName)
    {
        PhotonNetwork.LoadLevel(_sceneName);
    }
}
