using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {

	public Camera sourceCamera;

	void Update () {
		if (sourceCamera)
		{
			transform.rotation = sourceCamera.transform.rotation;
		}
	}
}
