using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class CamRot : MonoBehaviour
{
    public bool lockCursor;     //커서 숨김 여부
    public float mouseSensitivity = 10f;    //마우스 감도
    public Vector2 vertMinMax = new Vector2(-40, 85);   //세로축 최소값 최대값
    public float rotSmoothTime = 0.12f;  //smoothing delaytime
    public float distFromTarget = 2f;
    public Transform target;

    private Vector3 rotSmoothVelocity;
    private Vector3 currentRot;

    private float horizontal;   //가로축 회전값
    private float vertical;     //세로축 회전값

    void Start ()
    {
        if (target == null)
            target = GetComponent<SmoothFollow>().target;

        if(lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
		
	}
	
	
	void LateUpdate ()
    {
        vertical += Input.GetAxis("Mouse X") * mouseSensitivity;
        horizontal -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        horizontal = Mathf.Clamp(horizontal, vertMinMax.x, vertMinMax.y);

        currentRot = Vector3.SmoothDamp(currentRot, new Vector3(horizontal, vertical), ref rotSmoothVelocity, rotSmoothTime);
        transform.eulerAngles = currentRot;

        transform.position = target.position - transform.forward * distFromTarget;

	}
}
