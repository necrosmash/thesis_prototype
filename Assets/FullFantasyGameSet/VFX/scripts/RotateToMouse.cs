using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToMouse : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	void Update () {
		//rotation
		Vector3 mousePos = Input.mousePosition;
		//mousePos.z = 1.23f;

		Vector3 objectPos = Camera.main.WorldToScreenPoint (transform.position);
		mousePos.x = objectPos.x - mousePos.x;
		mousePos.y = objectPos.y - mousePos.y;
		mousePos.z = objectPos.z - mousePos.z;


		float angle = Mathf.Atan2(mousePos.x, mousePos.z) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(new Vector3(90, 90, angle));
	}
}
