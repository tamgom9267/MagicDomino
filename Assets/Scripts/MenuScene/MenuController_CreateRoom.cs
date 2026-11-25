using Photon.Realtime;
using TMPro;
using UnityEngine;

public partial class MenuController
{
    [Header("Create Room")]
    [SerializeField] private TMP_Text createRoomStatus;

    public void CreateNewRoom(TMP_InputField roomNameInput)
    {
        createRoomStatus.text = NetworkManager.instance.CreateRoom(roomNameInput.text.Trim())
            ? "Creating room..."
            : "The lobby is busy. Try again in a moment.";
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        createRoomStatus.text = returnCode == ErrorCode.GameIdAlreadyExists
            ? "That room name is already in use."
            : "Could not create the room. Try again.";
    }
}
