using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhotonManager : Photon.PunBehaviour
{
    public static PhotonManager instance;
    public static GameObject localPlayer;

    void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;

        PhotonNetwork.automaticallySyncScene = true;
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("Prototype_v0.0");
    }

    public void JoinGame()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
        PhotonNetwork.JoinOrCreateRoom("Fighting Room", options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("당신은 이미 게임 룸에 진입했습니다!!");

        if (PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.LoadLevel("GameRoomScene");
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Master Server에연결되었습니다");
    }


    void OnLevelWasLoaded(int levelNumber)
    {

        if (!PhotonNetwork.inRoom)
            return;
        Debug.Log("게임에 접속 되었습니다");


    }






}