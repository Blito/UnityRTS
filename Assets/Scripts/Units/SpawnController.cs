using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour {

	public Unit unitToSpawn;

	public void SpawnUnit()
	{
		Unit newUnit = Instantiate(unitToSpawn) as Unit;
		newUnit.SetSelected(false);
	}
}
