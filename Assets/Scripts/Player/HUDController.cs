using UnityEngine;
using System.Collections;

public class HUDController : MonoBehaviour {

	public CanvasGroup actionsCanvas;

	public CanvasGroup selectedUnitsCanvas;

	ArrayList selectedUnits = new ArrayList();

	void Start()
	{
		if (actionsCanvas)
		{
			actionsCanvas.alpha = 0f;
		}
		if (selectedUnitsCanvas)
		{
			selectedUnitsCanvas.alpha = 0f;
		}
	}

	public void SelectUnit(Unit unit)
	{
		if (unit && actionsCanvas && !selectedUnits.Contains(unit))
		{
			selectedUnits.Add(unit);
			actionsCanvas.alpha = 1f;
			selectedUnitsCanvas.alpha = 1f;
		}
	}

	public void DeselectUnit(Unit unit)
	{
		if (unit && selectedUnits.Contains(unit))
		{
			selectedUnits.Remove(unit);
			if (selectedUnits.Count == 0)
			{
				actionsCanvas.alpha = 0f;
				selectedUnitsCanvas.alpha = 0f;
			}
		}
	}

	public void SelectionCleared()
	{
		if (actionsCanvas)
		{
			selectedUnits.Clear();
			actionsCanvas.alpha = 0f;
			selectedUnitsCanvas.alpha = 0f;
		}
	}
}
