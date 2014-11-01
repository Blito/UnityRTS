using UnityEngine;
using System.Collections;

public class Selectable : MonoBehaviour {

	// Player reference
	public PlayerController playerController;

	bool isSelected = false;

	CameraController cameraController;
	Unit thisUnit;

	void Awake()
	{
		cameraController = Camera.main.GetComponent<CameraController> ();
		thisUnit = GetComponentInParent<Unit> ();
	}

	void Update()
	{
		if (cameraController && 
		    playerController &&
		    thisUnit &&
		    renderer.isVisible && 
		    cameraController.IsSelecting())
		{
			Vector3 camPos = Camera.main.WorldToScreenPoint(transform.position);
			camPos.y = CameraController.WorldToScreenY(camPos.y);
			bool selected = cameraController.GetSelectionRectangle().Contains(camPos);

			// if selection state changed
			if (selected != isSelected)
			{
				if (selected)
				{
					playerController.AddUnitToSelection(thisUnit);
				}
				else
				{
					playerController.RemoveUnitFromSelection(thisUnit);
				}
			}
		}
	}
	
	void OnMouseDown()
	{
		playerController.SelectUnit(thisUnit);
	}

	public bool IsSelected()
	{
		return isSelected;
	}

	public void SetSelected(bool selected)
	{
		isSelected = selected;

		// Change color according to selection status
		renderer.material.color = isSelected ? new Color (1f, 0f, 0f) : new Color (1f, 1f, 1f);
	}
}
