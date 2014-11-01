using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {

	public int teamNumber = 0;

	// List of selected units
	ArrayList	selectedUnits;

	// Floor's layer
	int floorMask;
	int unitMask;
	float camRayLength = 100f;

	HUDController hudController;

	void Awake()
	{
		selectedUnits = new ArrayList ();
		floorMask = LayerMask.GetMask ("Floor");
		unitMask = LayerMask.GetMask ("Unit");

		hudController = GetComponent<HUDController> ();
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			//OnLeftClick();
		}
		// If right-clicking, move the select units
		else if (selectedUnits.Count > 0 && Input.GetMouseButtonDown (1))
		{
			OnRightClick();
		}
	}

	void OnLeftClick()
	{
		// Ray cast to see where to go
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit rayHit;
		
		if (Physics.Raycast(camRay, out rayHit, camRayLength, unitMask))
		{
			Unit unitHit = rayHit.collider.GetComponent<Unit>();
			if (unitHit && unitHit.teamNumber == teamNumber)
			{
				SelectUnit(unitHit);
			}
		} 
	}

	void OnRightClick()
	{
		// Ray cast to see where to go
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit rayHit;
		
		if (Physics.Raycast(camRay, out rayHit, camRayLength, unitMask))
		{
			Unit unitHit = rayHit.collider.GetComponent<Unit>();
			if (unitHit && unitHit.teamNumber != teamNumber)
			{
				foreach (Unit unit in selectedUnits)
				{
					unit.Attack(unitHit);
				}
			}
		} 
		else if (Physics.Raycast(camRay, out rayHit, camRayLength, floorMask))
		{
			foreach (Unit unit in selectedUnits)
			{
				unit.Move(rayHit.point);
			}
		}
	}

	bool SelectUnit(Unit unit)
	{
		// Clear list of selected units if not pressing shift
		if (!Input.GetKey (KeyCode.LeftShift)) 
		{
			DeselectAllUnits();
		}

		AddUnitToSelection(unit);

		return false;
	}

	public void AddUnitToSelection(Unit unit)
	{
		if (!selectedUnits.Contains(unit))
		{
			selectedUnits.Add (unit);
			
			unit.SetSelected (true);
			
			if (hudController)
			{
				hudController.SelectUnit(unit);
			}
		}
	}

	public void RemoveUnitFromSelection(Unit unit)
	{
		if (selectedUnits.Contains(unit))
		{
			selectedUnits.Remove (unit);
			
			unit.SetSelected (false);
			
			if (hudController)
			{
				hudController.DeselectUnit(unit);
			}
		}
	}

	public void DeselectAllUnits()
	{
		// Set selected = false in every unit
		foreach (Unit unit in selectedUnits)
		{
			unit.SetSelected(false);
		}

		hudController.SelectionCleared();

		selectedUnits.Clear ();
	}

	public void OnClickNewUnit()
	{
		foreach (Unit unit in selectedUnits)
		{
			unit.SpawnUnit();
		}
	}

	public void UnitDestroyed(Unit unit)
	{
		if (selectedUnits.Contains(unit) && selectedUnits.Count == 1)
		{
			selectedUnits.Remove(unit);
			hudController.SelectUnit(null);
		}
	}

}
