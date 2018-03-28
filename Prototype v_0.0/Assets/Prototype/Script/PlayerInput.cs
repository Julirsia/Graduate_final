using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Com.MyCompany.MyGame
{
    public class PlayerInput : Photon.PunBehaviour
    {
        private Player user;
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        void Awake()
        {
            user = GetComponent<Player>();

            if (!photonView.isMine)
            { enabled = false; }
        }

        // Use this for initialization
        void Start()
        {
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

        // Update is called once per frame
        void Update()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            GetInput();
            UserStateChange();
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v * m_CamForward + h * m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v * Vector3.forward + h * Vector3.right;
            }
        }



        public void GetInput()
        {
            user.Horizontal = Input.GetAxis("Horizontal");
            user.Vertical = Input.GetAxis("Vertical");

            //user.Run = Input.GetKey("space");
            user.Attack = Input.GetMouseButtonDown(0);

            //rotation
            if (user.charState != Character.State.idle)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(ray, out hit, 100f, user.floorMask))
                {
                    user.Rotate(hit.point);
                }
            }
        }
        public void UserStateChange()
        {
            if (user.Vertical == 0 && user.Horizontal == 0)
                user.charState = Character.State.idle;
            else
                user.charState = Character.State.walk;

            if (user.Attack)
                user.charState = Character.State.attack;

            if (user.HP <= 0)
                user.charState = Character.State.die;
        }


        void OnTriggerEnter(Collider coll)
        {

        }


    }
}
