using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public float m_StartDelay = 3f;
    public float m_EndDelay = 3f;
    private WaitForSeconds m_StartWait;
    private WaitForSeconds m_EndWait;
    // Use this for initialization
    void Start () {

        // Create the delays so they only have to be made once.
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        if (!PhotonNetwork.inRoom)
            return;

        if (PhotonNetwork.isMasterClient)
        {
            SpawnPlayer1();
        }
        else
        {
            SpawnPlayer2();
            Invoke("EnterGameLoop", 3);
        }



    }

    private void SpawnPlayer1()
    {
        GameObject Player = PhotonNetwork.Instantiate("Character", Vector3.zero, Quaternion.identity, 0) as GameObject;

        Player.name = "Player1";

        Debug.Log("SpawnPlayer1");

    }
    private void SpawnPlayer2()
    {
        GameObject Player = PhotonNetwork.Instantiate("Character", Vector3.zero, Quaternion.identity, 0) as GameObject;

        Player.name = "Player2";

        Debug.Log("SpawnPlayer2");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
