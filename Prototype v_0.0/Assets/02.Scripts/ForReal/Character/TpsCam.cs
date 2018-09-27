using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpsCam : MonoBehaviour {

    [SerializeField] Vector3 cameraOffset;
    [SerializeField] float damping;
    [SerializeField] Transform Target;
    public bool lockCursor;


    Transform cameraLookTarget;

    
    
   void Awake()
    {
        cameraLookTarget = transform.Find("cameraLookTarget");

        if (cameraLookTarget == null)
            cameraLookTarget = Target.transform;
    }
   

	// Use this for initialization
	void Start () {

       

       
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 targetPosition = cameraLookTarget.position + Target.transform.forward * cameraOffset.z +
             Target.transform.up * cameraOffset.y +
             Target.transform.up * cameraOffset.x;

        Quaternion targetRotation = Quaternion.LookRotation(Target.forward, Vector3.up);

        transform.position = Vector3.Lerp(transform.position, targetPosition, damping * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, damping * Time.deltaTime);

	}
}
