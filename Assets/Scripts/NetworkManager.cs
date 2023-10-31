using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour, IPunObservable
{
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
       // throw new System.NotImplementedException();
    }

    private PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        photonView = PhotonView.Get(this);
    }

    public void NotifySelectBoardPiece(GameObject gameObject)
    {
        if ((int)GetComponent<GameManager>().currentActivePlayer.id
            == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            photonView.RPC("RPC_NotifySelectBoardPiece", RpcTarget.All, gameObject.name);
        }
    }

    [PunRPC]
    public void RPC_NotifySelectBoardPiece(string gameObjectName)
    {
        print("Received message from server:" + gameObjectName);
       
        GetComponent<GameManager>().SelectBoardPiece(GameObject.Find(gameObjectName));
       
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
