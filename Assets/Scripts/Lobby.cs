using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : MonoBehaviourPunCallbacks
{
    [Tooltip("Content Object")]
    public GameObject ScrollViewContent;

    [Tooltip("UI ROW Prefab Containing the room details")]
    public GameObject RowRoom;

    [Tooltip("Player Name")]
    public GameObject InputPlayerName;

    [Tooltip("Room Name")]
    public GameObject InputRoomName;

    [Tooltip("Status Message")]
    public GameObject Status;

    [Tooltip("Button Create Room")]
    public GameObject BtnCreateRoom;

    [Tooltip("Panel Lobby")]
    public GameObject PanelLobby;

    [Tooltip("Panel Waiting for Players")]
    public GameObject PanelWaitingForPlayers;

    List<RoomInfo> availableRooms = new List<RoomInfo>();

    UnityEngine.Events.UnityAction buttonCallback;


    // Start is called before the first frame update
    void Start()
    {
        //this makes sure that everyone (all connected clients) is on the same scene
        PhotonNetwork.AutomaticallySyncScene = true;

        if (!PhotonNetwork.IsConnected)
        {
            //set the app version before connecting
            PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = "1.0";
            //connect to photon 
            PhotonNetwork.ConnectUsingSettings();
        }

        InputRoomName.GetComponent<TMP_InputField>().text = "Room1";
        InputPlayerName.GetComponent<TMP_InputField>().text = "Player 1";

    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print("OnConnectedToMaster");
        //after we connect to master server, we have to join a lobby
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 2;

        PhotonNetwork.JoinOrCreateRoom(InputRoomName.GetComponent<TMP_InputField>().text, 
            roomOptions, TypedLobby.Default);


    }

    public override void OnCreatedRoom()
    {
        print("Created room");
        //set our player name (nickname)
        PhotonNetwork.NickName = InputPlayerName.GetComponent<TMP_InputField>().text;
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print("room creation failed:"+message);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        print("Number of rooms:" + roomList.Count);
        availableRooms = roomList;
        UpdateRoomList();
    }

    private void UpdateRoomList()
    {
        foreach(RoomInfo roomInfo in availableRooms)
        {
            GameObject rowRoom = Instantiate(RowRoom);
            rowRoom.transform.parent = ScrollViewContent.transform;
            rowRoom.transform.localScale = Vector3.one;

            rowRoom.transform.Find("RoomName").GetComponent<TextMeshProUGUI>().text = roomInfo.Name;
            rowRoom.transform.Find("RoomPlayers").GetComponent<TextMeshProUGUI>().text = roomInfo.PlayerCount.ToString();

            buttonCallback = () => this.OnClickJoinRoom(roomInfo.Name);
            rowRoom.transform.Find("BtnJoin").GetComponent<Button>().onClick.AddListener(buttonCallback);

        }
    }

    private void OnClickJoinRoom(string roomName)
    {
        //set our player name (nickname)
        PhotonNetwork.NickName = InputPlayerName.GetComponent<TMP_InputField>().text;
        //join the room
        PhotonNetwork.JoinRoom(roomName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnGUI()
    {
        Status.GetComponent<TextMeshProUGUI>().text = "Status:" + PhotonNetwork.NetworkClientState.ToString();
    }
}
