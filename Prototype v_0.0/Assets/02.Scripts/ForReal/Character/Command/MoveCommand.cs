using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : Photon.PunBehaviour, ICommand 
{
    private Vector3 moveValue;
    private bool isJump;
    /*
     private Vector3 f_move;
     private bool m_Jump;
     private Transform m_Cam;                  // A reference to the main camera in the scenes transform
     private Vector3 m_CamForward;

    
     private void Start(){
         if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }
     }

     private void Update(){
         if (!m_Jump)
            {
                m_Jump = Input.GetKey(KeyCode.Space);
            }
     }

    private void FixedUpdate()
    {

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool crouch = Input.GetKey(KeyCode.C);

           if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                f_move = v*m_CamForward + h*m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                f_move = v*Vector3.forward + h*Vector3.right;
            }
            if (Input.GetKey(KeyCode.LeftShift)) f_move *= 0.5f;
    }
    */

     public MoveCommand(Actor actor, Vector3 moveVect , bool jump)
    {
        moveValue = moveVect;
        isJump = jump;
    
     }
    public void Execute(Actor actor)
    {
        actor.Move(moveValue);//, isJump);
        //m_Jump = false;
    }
}
