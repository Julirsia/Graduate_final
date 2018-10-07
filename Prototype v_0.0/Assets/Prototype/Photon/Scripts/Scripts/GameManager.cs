using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Complete {

    public class GameManager : Photon.MonoBehaviour {


        public int m_NumRoundsToWin = 5;            // The number of rounds a single player has to win to win the game.
        public float m_StartDelay = 3f;             // The delay between the start of RoundStarting and RoundPlaying phases.
        public float m_EndDelay = 3f;
        bool flag = false;// The delay between the end of RoundPlaying and RoundEnding phases.
                          //  public CameraControl m_CameraControl; 
        public CameraTest m_CameraControl;
        public GameObject Player;                   // Reference to the CameraControl script for control during different phases.
        public Text m_MessageText;                  // Reference to the overlay Text to display winning text, etc.
        public GameObject m_TankPrefab;             // Reference to the prefab the players will control.
        public PlayerManager[] m_Players;
        public InputHandler control;
        public PlayerManager[] spawnpoint;
        public Camera m_camera;
        // A collection of managers for enabling and disabling different aspects of the tanks.


        private int m_RoundNumber;                  // Which round the game is currently on.
        private WaitForSeconds m_StartWait;         // Used to have a delay whilst the round starts.
        private WaitForSeconds m_EndWait;           // Used to have a delay whilst the round or game ends.
        private PlayerManager m_RoundWinner;          // Reference to the winner of the current round.  Used to make an announcement of who won.
        private PlayerManager m_GameWinner;           // Reference to the winner of the game.  Used to make an announcement of who won.





        private void Awake()
        {
            m_camera = GetComponentInChildren<Camera>();
        }

        // Use this for initialization
        void Start() {

            // Create the delays so they only have to be made once.
            m_StartWait = new WaitForSeconds(m_StartDelay);
            m_EndWait = new WaitForSeconds(m_EndDelay);
           // gameObject.GetComponent<CameraControl>().enabled = false;



            if (!PhotonNetwork.inRoom)
                return;

            if (PhotonNetwork.isMasterClient) {
                SpawnMasterTank();

                




            }
                //  StartCoroutine(GameLoop());

            else {
                SpawnTank2();
                

                //StartCoroutine(GameLoop());

            }
            
   

       /* if (!PhotonNetwork.inRoom)
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
        */


    }



      
    /*
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
        }*/

    // This is called from start and will run each phase of the game one after another.
    private void SpawnAllTanks()
        {
            // For all the tanks...
            for (int i = 0; i < m_Players.Length; i++)
            {
                // ... create them, set their player number and references needed for control.
                m_Players[i].m_Instance =
                    Instantiate(m_TankPrefab, m_Players[i].m_SpawnPoint.position, m_Players[i].m_SpawnPoint.rotation) as GameObject;
                m_Players[i].m_PlayerNumber = i + 1;
                m_Players[i].Setup();
            
            }
        }

         

    // create one Tank, set its player number and references needed for control.
    private void SpawnMasterTank()
        {
            int i = 0;
            GameObject Player = PhotonNetwork.Instantiate(
                         "Character",
                         m_Players[i].m_SpawnPoint.position,
                         m_Players[i].m_SpawnPoint.rotation,
                         0) as GameObject;

            Player.name = "MasterPlayer";
            m_camera.transform.SetParent(Player.transform);
           // m_camera.transform.LookAt(Player.transform.position);
            m_camera.transform.position = Player.transform.GetChild(0).position;

            m_Players[i].m_Instance = Player;
            m_Players[i].m_PlayerNumber = i + 1;
            m_Players[i].Setup();
          
            Debug.Log("SpawnMasterPlayer");
        }

        private int SpawnTank2()
        {
            int i = 1;
            GameObject Player = PhotonNetwork.Instantiate(
                         "Character",
                         m_Players[i].m_SpawnPoint.position,
                         m_Players[i].m_SpawnPoint.rotation,
                         0) as GameObject;

            Player.name = "Player2";
            m_Players[i].m_Instance = Player;
            m_Players[i].m_PlayerNumber = i + 1;
            m_Players[i].Setup();
            

            Debug.Log("SpawnPlayer2");
            return 0;
        }
        
        private void FindEnemy()
        {
            if (PhotonNetwork.isMasterClient)
            {
                m_Players[1].m_Instance = PhotonView.Find(2001).gameObject;
               
               // m_Players[1].Setup();
            }
            else
            {
                m_Players[0].m_Instance = PhotonView.Find(1001).gameObject;
                //  m_Players[0].Setup();
            }
        }


        public void SetCameraTargets()
        {
            // Create a collection of transforms the same size as the number of tanks.
            Transform targets;

            // For each of these transforms...

            // ... set it to the appropriate tank transform.

            if (PhotonNetwork.isMasterClient)
            {
                targets = PhotonView.Find(1001).gameObject.transform;
                

            }
            else
            {
                targets = PhotonView.Find(2001).gameObject.transform;
                
            }

            // targets[1] = PhotonView.Find(2001).gameObject.transform;
          //  targets[1] = spawnpoint[1].m_SpawnPoint.transform;

            // m_CameraControl.m_Targets[1] = PhotonView.Find(2001).gameObject.transform;

            // These are the targets the camera should follow.
            m_CameraControl.Targets = targets;
        }

        public void SetEnemyCameraTargets()
        {
            m_CameraControl.Targets = PhotonView.Find(2001).gameObject.transform;
        }


        // This is called from start and will run each phase of the game one after another.
        private IEnumerator GameLoop()
        {
            // Start off by running the 'RoundStarting' coroutine but don't return until it's finished.

            //  GameObject.Find("CameraRig").GetComponent<CameraControl>().enabled = true;

            if (PhotonNetwork.isMasterClient)
            {
                m_camera.transform.SetParent(PhotonView.Find(1001).gameObject.transform);
                // m_camera.transform.LookAt(Player.transform.position);
                m_camera.transform.position = PhotonView.Find(1001).gameObject.transform.GetChild(0).position;
            }
            else
            {
                m_camera.transform.SetParent(PhotonView.Find(2001).gameObject.transform);
                // m_camera.transform.LookAt(Player.transform.position);
                m_camera.transform.position = PhotonView.Find(2001).gameObject.transform.GetChild(0).position;
            }
            yield return StartCoroutine(RoundStarting());

            // Once the 'RoundStarting' coroutine is finished, run the 'RoundPlaying' coroutine but don't return until it's finished.
            yield return StartCoroutine(RoundPlaying());

            // Once execution has returned here, run the 'RoundEnding' coroutine, again don't return until it's finished.
            yield return StartCoroutine(RoundEnding());

            yield return StartCoroutine(RoundReset());

            // This code is not run until 'RoundEnding' has finished.  At which point, check if a game winner has been found.
            if (m_GameWinner != null)
            {
                // If there is a game winner, restart the level.
                SceneManager.LoadScene(0);
            }
            else
            {
                // If there isn't a winner yet, restart this coroutine so the loop continues.
                // Note that this coroutine doesn't yield.  This means that the current version of the GameLoop will end.
                StartCoroutine(GameLoop());
               // Invoke("GameLoop", 2f);
            }
        }
        // 게임시작시 설정사항

     /*   private IEnumerator RoundSetting()
        {
            GameObject.Find("CameraRig").GetComponent<CameraControl>().enabled = true;
            yield return null;
        } */

        private IEnumerator RoundStarting()
        {

            
            // As soon as the round starts reset the tanks and make sure they can't move.
            ResetAllTanks();
            DisableTankControl();

            //  Invoke("SetCameraTargets", 2);
            
           // SetCameraTargets();
            
            // Snap the camera's zoom and position to something appropriate for the reset tanks.
           // m_CameraControl.SetStartPositionAndSize();

            // Increment the round number and display text showing the players what round it is.
            m_RoundNumber++;
            m_MessageText.text = "ROUND " + m_RoundNumber;

            // Wait for the specified length of time until yielding control back to the game loop.
            yield return m_StartWait;
        }

        // 게임 플레이 중 설정사항
        private IEnumerator RoundPlaying()
        {
            // As soon as the round begins playing let the players control the tanks.
            EnableTankControl();
           // SetEnemyCameraTargets();
            // Clear the text from the screen.
            m_MessageText.text = string.Empty;

            // While there is not one tank left...
            while (!OneTankLeft())
            {
                // ... return on the next frame.
                yield return null;
            }
        }

        //게임 종료시 설정사항
        private IEnumerator RoundEnding()
        {
            // Stop tanks from moving.
            DisableTankControl();

            // Clear the winner from the previous round.
            m_RoundWinner = null;

            // See if there is a winner now the round is over.
            m_RoundWinner = GetRoundWinner();

            // If there is a winner, increment their score.
            if (m_RoundWinner != null)
                m_RoundWinner.m_Wins++;

            // Now the winner's score has been incremented, see if someone has one the game.
            m_GameWinner = GetGameWinner();

            // Get a message based on the scores and whether or not there is a game winner and display it.
            string message = EndMessage();
            m_MessageText.text = message;

            

            // Wait for the specified length of time until yielding control back to the game loop.
            yield return m_EndWait;
        }

        private IEnumerator RoundReset()
        {
            m_Players[0].m_Instance.SetActive(true);

            m_Players[1].m_Instance.SetActive(true);
            PhotonView.Find(1001).gameObject.transform.position = spawnpoint[0].m_SpawnPoint.position;
            PhotonView.Find(2001).gameObject.transform.position = spawnpoint[1].m_SpawnPoint.position;

            yield return m_EndWait;
        }


        // This is used to check if there is one or fewer tanks remaining and thus the round should end.
        private bool OneTankLeft()
        {
            

            // Start the count of tanks left at zero.
            int numTanksLeft = 0;

            // Go through all the tanks...

            if (PhotonNetwork.isMasterClient)
            {
                FindEnemy();
                for (int i = 0; i < 2; i++)
                {
                    // ... and if they are active, increment the counter.
                    if (m_Players[i].m_Instance.activeSelf)
                        numTanksLeft++;
                }
            }
            else
            {
                FindEnemy();
                for (int i = 0; i < 2; i++)
                {
                    // ... and if they are active, increment the counter.
                    if (m_Players[i].m_Instance.activeSelf)
                        numTanksLeft++;
                }
            }

                // ... and if they are active, increment the counter.
              /*  if (m_Players[0].m_Instance.activeSelf)
                    numTanksLeft++;
                else if (m_Players[1].m_Instance.activeSelf)
                numTanksLeft++; */


            // If there are one or fewer tanks remaining return true, otherwise return false.
            return numTanksLeft <= 1;
        }


        // This function is to find out if there is a winner of the round.
        // This function is called with the assumption that 1 or fewer tanks are currently active.
        private PlayerManager GetRoundWinner()
        {
            // Go through all the tanks...

            /* if (PhotonNetwork.isMasterClient)
             {
                 if (m_Players[0].m_Instance.activeSelf)
                     return m_Players[0];
             }
             else
             {
                 if (m_Players[1].m_Instance.activeSelf)
                     return m_Players[1];
             }*/
            for (int i = 0; i < m_Players.Length; i++)
                  {
                      // ... and if one of them is active, it is the winner so return it.
                      if (m_Players[i].m_Instance.activeSelf)
                          return m_Players[i];
                  }

            // If none of the tanks are active it is a draw so return null.
            return null;
        }


        // This function is to find out if there is a winner of the game.
        private PlayerManager GetGameWinner()
        {

            /* if (PhotonNetwork.isMasterClient)
             {
                 if (m_Players[0].m_Wins == m_NumRoundsToWin)
                     return m_Players[0];
             }
             else
             {
                 if (m_Players[1].m_Wins == m_NumRoundsToWin)
                     return m_Players[1];
             }*/

            // Go through all the tanks...
            for (int i = 0; i < m_Players.Length; i++)
                 {
                     // ... and if one of them has enough rounds to win the game, return it.
                     if (m_Players[i].m_Wins == m_NumRoundsToWin)
                         return m_Players[i];
                 }

            // If no tanks have enough rounds to win, return null.
            return null;
        }

        // 게임 종료시 출력 메세지
        private string EndMessage()
        {
            // By default when a round ends there are no winners so the default end message is a draw.
            string message = "DRAW!";

            // If there is a winner then change the message to reflect that.
            if (m_RoundWinner != null)
                message = m_RoundWinner.m_ColoredPlayerText + " WINS THE ROUND!";

            // Add some line breaks after the initial message.
            message += "\n\n\n\n";

            if (PhotonNetwork.isMasterClient)
            {
                message += m_Players[0].m_ColoredPlayerText + ": " + m_Players[0].m_Wins + " WINS\n";
            }
            else
            {
                message += m_Players[1].m_ColoredPlayerText + ": " + m_Players[1].m_Wins + " WINS\n";
            }

                // Go through all the tanks and add each of their scores to the message.
                /*
                for (int i = 0; i < m_Players.Length; i++)
                {
                    message += m_Players[i].m_ColoredPlayerText + ": " + m_Players[i].m_Wins + " WINS\n";
                }*/

                // If there is a game winner, change the entire message to reflect that.
                if (m_GameWinner != null)
                message = m_GameWinner.m_ColoredPlayerText + " WINS THE GAME!";

            return message;
        }

        private void ResetAllTanks()
        {
            if (PhotonNetwork.isMasterClient)
            {


                m_Players[0].m_Instance.transform.position = spawnpoint[0].m_SpawnPoint.position;
            }
            else
            {

                m_Players[1].m_Instance.transform.position = spawnpoint[1].m_SpawnPoint.position;
            }
        }


        private void EnableTankControl()
        {
            if (PhotonNetwork.isMasterClient) {
                m_Players[0].m_actor.enabled = true;
                m_Players[0].m_Movement.enabled = true;
                m_Players[0].m_Shooting.enabled = true;
                m_Players[0].m_CanvasGameObject.SetActive(true);
                m_Players[0].m_Instance.SetActive(true);
            }
            else { 
              m_Players[1].m_actor.enabled = true;
              m_Players[1].m_Movement.enabled = true;
              m_Players[1].m_Shooting.enabled = true;
              m_Players[1].m_CanvasGameObject.SetActive(true);
                m_Players[1].m_Instance.SetActive(true);
            }
        }


        private void DisableTankControl()
        {

            if (PhotonNetwork.isMasterClient) {
                m_Players[0].m_actor.enabled = false;
                m_Players[0].m_Movement.enabled = false;
                m_Players[0].m_Shooting.enabled = false;
                m_Players[0].m_CanvasGameObject.SetActive(false);
            }
            else { 
             m_Players[1].m_actor.enabled = false;
             m_Players[1].m_Movement.enabled = false;
             m_Players[1].m_Shooting.enabled = false;
             m_Players[1].m_CanvasGameObject.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update () {
            if (PhotonNetwork.playerList.Length > 1 && !flag)
            {

                flag = true;
                
                StartCoroutine(GameLoop());
                
            }
            else
            {

            }
        }
}
}