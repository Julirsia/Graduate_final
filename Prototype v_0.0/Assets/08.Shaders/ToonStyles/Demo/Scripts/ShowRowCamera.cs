using UnityEngine;
using System.Collections;

/// <summary>
/// Moves the gameobject with mouse or input axis.
/// </summary>
public class ShowRowCamera : MonoBehaviour
{
	public Vector3 speedHorizontal;
	public Vector3 speedVertical;
	public Vector3 speedWheel;
	public Vector3 min;
	public Vector3 max;
	public float mouseDragSpeed;
	
	private Vector3 lastMousePos;
	
	void Update ()
	{
		transform.position += (speedHorizontal * Time.deltaTime * Input.GetAxis("Horizontal"));
		transform.position += (speedVertical * Time.deltaTime * Input.GetAxis("Vertical"));
		transform.position += (speedWheel * Input.GetAxis("Mouse ScrollWheel"));
		
		if (Input.GetMouseButtonDown(0))
		{
			lastMousePos = Input.mousePosition;
		}
		else if (Input.GetMouseButton(0))
		{
			float deltaX = Input.mousePosition.x - lastMousePos.x;
			float deltaY = Input.mousePosition.y - lastMousePos.y;
			transform.position -= (speedHorizontal * deltaX * mouseDragSpeed);
			transform.position -= (speedVertical * deltaY * mouseDragSpeed);
			
			lastMousePos = Input.mousePosition;
		}
		
		transform.position = new Vector3(
			Mathf.Clamp(transform.position.x, min.x, max.x),
			Mathf.Clamp(transform.position.y, min.y, max.y),
			Mathf.Clamp(transform.position.z, min.z, max.z)
		);
	}
}
