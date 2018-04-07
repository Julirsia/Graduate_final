using UnityEngine;

namespace UnityStandardAssets.Utility
{
	public class SmoothFollow : MonoBehaviour
	{
        #region camera rotation Field
        private const float Y_ANGLE_MIN = 0.0f;
        private const float Y_ANGLE_MAX = 30.0f;

        private float currentX = 0f;
        private float currentY = 45.0f;
        private float sensitivityX = 4.0f;
        private float sensitivityY = 1.0f;
        #endregion

        #region smooth follow Values
        // The target we are following
        [SerializeField]
		private Transform target;
		// The distance in the x-z plane to the target
		[SerializeField]
		private float distance = 10.0f;
		// the height we want the camera to be above the target
		[SerializeField]
		private float height = 5.0f;

		[SerializeField]
		private float rotationDamping;
		[SerializeField]
		private float heightDamping;
        #endregion
        // Use this for initialization
        void Start()
        {
        }
        private void Update()
        {
            currentX += Input.GetAxis("Mouse X");
            currentY += Input.GetAxis("Mouse Y");

            currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
        }

        // Update is called once per frame
        void LateUpdate()
		{
			// Early out if we don't have a target
			if (!target)
				return;

			// Calculate the current rotation angles
			//var wantedRotationAngle = target.eulerAngles.y;
			//var wantedHeight = target.position.y + height;

			var currentRotationAngle = transform.eulerAngles.y;
			var currentHeight = transform.position.y;

			// Damp the rotation around the y-axis
			//currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

			// Damp the height
			//currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

			// Convert the angle into a rotation
			//var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

            // Set the position of the camera on the x-z plane to:
            // distance meters behind the target
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
            transform.position = target.position;
			transform.position += rotation * Vector3.back*distance;

			// Set the height of the camera
			//transform.position = new Vector3(transform.position.x ,currentHeight , transform.position.z);

			// Always look at the target
			transform.LookAt(target);
		}
	}
}