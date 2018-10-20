using UnityEngine;
using System.Collections;

/// <summary>
/// Rotates the object around the desired axis.
/// </summary>
public class AutoRotate : MonoBehaviour
{
	public Vector3 axis = Vector3.forward;
	public float speed = 1;
	
	void Update ()
	{
		transform.RotateAroundLocal(axis, speed * Time.deltaTime);
	}
}
