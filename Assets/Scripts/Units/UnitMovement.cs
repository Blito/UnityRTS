using UnityEngine;
using System.Collections;

public class UnitMovement : MonoBehaviour {

	NavMeshAgent navAgent;

	void Awake()
	{
		navAgent = GetComponent<NavMeshAgent> ();
	}

	public bool Move(Vector3 destination)
	{
		return navAgent.SetDestination (destination);
	}

	public void Stop()
	{
		navAgent.Stop();
	}

}
