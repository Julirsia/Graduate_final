using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTest : Photon.MonoBehaviour
{
    [HideInInspector] public Transform Targets;
    private Camera m_Camera;
    private Vector3 m_DesiredPosition;
    private Vector3 m_MoveVelocity;
    public float m_DampTime = 0.2f;
   



    private void Awake()
    {
        m_Camera = GetComponentInChildren<Camera>();
    }

    private void FixedUpdate()
    {
        // Move the camera towards a desired position.
        Move();

      
    }

    private void Move()
    {


        if (PhotonNetwork.isMasterClient) {
            transform.position = PhotonView.Find(1001).gameObject.transform.position;
            m_Camera.transform.LookAt(PhotonView.Find(1001).gameObject.transform.position);
            m_Camera.transform.RotateAround(PhotonView.Find(1001).gameObject.transform.position, Vector3.up, PhotonView.Find(1001).gameObject.transform.rotation.z);
        }
       
    }

    private void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();
      

        
            averagePos = Targets.position;
     

       
        averagePos.y = transform.position.y;

        // The desired position is the average position;
        m_DesiredPosition = averagePos;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
