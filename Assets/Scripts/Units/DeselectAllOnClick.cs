using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DeselectAllOnClick : MonoBehaviour {

	PlayerController player;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
	}

	void OnMouseDown()
	{
		if (EventSystem.current.IsPointerOverGameObject())
		{
			return;
		}
		player.DeselectAllUnits ();
	}

}
