using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : Photon.PunBehaviour
{
    private Player user;

    void Awake()
    {
        user = GetComponent<Player>();

       if(!photonView.isMine)
        { enabled = false; }
    }

	// Use this for initialization
	void Start ()
    {
		
	}

	// Update is called once per frame
	void Update ()
    {
        GetInput();
        UserStateChange();

    }

    public void GetInput()
    {
        user.Horizontal = Input.GetAxis("Horizontal");
        user.Vertical = Input.GetAxis("Vertical");

        //user.Run = Input.GetKey("space");
        user.Attack = Input.GetMouseButtonDown(0);
        
        //rotation
        if(user.charState != Character.State.idle)
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
