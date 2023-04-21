using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour {

	public float speed = 2f;
	public float distanceFromCam = 5f;

	void Update () {
		
		Vector3 mousePosition = Input.mousePosition;
		mousePosition.z = distanceFromCam;
		Vector3 mouseScreenToWorld = Camera.main.ScreenToWorldPoint(mousePosition);

		Vector3 position = Vector3.Lerp(transform.position, mouseScreenToWorld, 1.0f - Mathf.Exp(-speed * Time.deltaTime));

		transform.position = position;
		//Debug.Log (transform.position);
	}
}
