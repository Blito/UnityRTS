using UnityEngine;
using System.Collections;

public class UnitMovement : MonoBehaviour {

	public enum Status { Idle, Moving };
	
	Status status = Status.Idle;

	NavMeshAgent navAgent;

	void Awake()
	{
		navAgent = GetComponent<NavMeshAgent> ();
	}
	
	void Update()
	{
		if (status == Status.Moving)
		{
			Vector3 distanceVector = transform.position - navAgent.destination;
			if (distanceVector.sqrMagnitude <= navAgent.stoppingDistance * navAgent.stoppingDistance)
			{
				status = Status.Idle;
			}
		}
	}

	public bool Move(Vector3 destination)
	{
		status = Status.Moving;
		return navAgent.SetDestination (destination);
	}

	public void Stop()
	{
		status = Status.Idle;
		navAgent.Stop();
	}
	
	public Status GetStatus()
	{
		return status;
	}

}
