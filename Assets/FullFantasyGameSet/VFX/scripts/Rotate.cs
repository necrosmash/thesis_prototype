using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {
	public float rotationSpeed = 1.0f;

	void Start () {

	}

	void Update() {
		transform.Rotate(Vector3.up * Time.deltaTime*rotationSpeed, Space.Self);
	}
}
