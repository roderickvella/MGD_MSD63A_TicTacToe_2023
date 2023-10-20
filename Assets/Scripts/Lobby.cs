using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnGUI()
    {
        Status.GetComponent<TextMeshProUGUI>().text = "Status:" + PhotonNetwork.NetworkClientState.ToString();
    }
}
