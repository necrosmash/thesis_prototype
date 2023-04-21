using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAt : MonoBehaviour 
{
	public Transform target;

		void LateUpdate()
		{
			// Rotate the camera every frame so it keeps looking at the target
			transform.LookAt(target);
		}

}
