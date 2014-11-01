using UnityEngine;
using System.Collections;

public class AttackController : MonoBehaviour {

	public enum Stance { Chase, Stand };
	
	public enum Status { Idle, Attacking, Disabled };

	// Damage caused to victim, before mitigations
	public float AttackDamage = 1f;

	// Max distance from target
	public float attackRange = 1f;

	// Attack fire rate (rounds/sec)
	public float fireRate = 3f;
	
	// Defines the unit's behaviour when an enemy enters its radius
	public Stance stance = Stance.Chase;
	
	UnitMovement movementController;

	LineRenderer	gunLine;
	Light			gunLight;
	float			effectsTime = 0.1f;
	
	Unit currentObjective;
	ArrayList objectivePool = new ArrayList();
	
	Status status = Status.Idle;

	IEnumerator attackRoutine;

	void Awake()
	{
		gunLine = GetComponentInChildren<LineRenderer>();
		gunLight = GetComponentInChildren<Light>();
		movementController = GetComponentInParent<UnitMovement>();
	}
	
	public void SetEnabled(bool enableAttack)
	{
		status = enableAttack ? Status.Disabled : Status.Idle;
	}
	
	public Status GetStatus()
	{
		return status;
	}
	
	public Stance GetStance()
	{
		return stance;
	}

	public void StartAttack(Unit objective)
	{
		if (objective && objective != currentObjective)
		{
			if (status == Status.Attacking)
			{
				StopAttack();
			}
			status = Status.Attacking;
			currentObjective = objective;
			attackRoutine = (stance == Stance.Chase) ? 
								ChaseAndAttack(currentObjective) :
								StandAndAttack(currentObjective);
			StartCoroutine(attackRoutine);
		}
	}

	public void StopAttack()
	{
		if (attackRoutine != null)
		{
			StopCoroutine(attackRoutine);
			currentObjective = null;
			status = Status.Idle;
		}
	}
	
	public void UnitEnteredRange(Unit unit)
	{
		if (unit && unit != currentObjective && 
			!objectivePool.Contains(unit))
		{
			objectivePool.Add(unit);
			
			if (status != Status.Disabled)
			{
				StartAttack(unit);
			}
		}
		
	}
	
	public void UnitLeftRange(Unit unit)
	{
		if (unit && objectivePool.Contains(unit))
		{
			objectivePool.Remove(unit);
		}
	}
	
	bool IsInRange(Unit objective)
	{
		return Vector3.Distance(transform.position, objective.transform.position) < attackRange;
	}
	
	IEnumerator Attack(Unit objective)
	{
		while (objective)
		{
			StartCoroutine(ShowEffects(objective));

			objective.TakeDamage(AttackDamage, gameObject);

			yield return new WaitForSeconds(1/fireRate);
		}
		StopAttack();
		ChooseNextTarget();
	}

	void OnDestroy()
	{
		StopAttack();
	}
	
	void ChooseNextTarget()
	{
		if (objectivePool.Count > 0)
		{
			currentObjective = (Unit)objectivePool[objectivePool.Count-1];
			objectivePool.RemoveAt(objectivePool.Count-1);
		}
	}

	IEnumerator ShowEffects(Unit objective)
	{
		if (!objective)
		{
			yield break;
		}
		// Enable bullet line
		if (gunLine)
		{
			gunLine.enabled = true;
			gunLine.SetPosition(0, transform.position);
			gunLine.SetPosition(1, objective.transform.position);
		}
		
		// Enable fire light
		if (gunLight)
		{
			gunLight.enabled = true;
		}
		
		yield return new WaitForSeconds(effectsTime);

		gunLine.enabled = false;
		gunLight.enabled = false;
	}
	
	IEnumerator ChaseAndAttack(Unit objective)
	{
		// Until objective is dead or we're stopped
		while (objective)
		{
			// Chase
			while (objective && !IsInRange(objective))
			{
				movementController.Move (objective.transform.position);
				yield return null;
			}
			
			// In range, start shooting
			movementController.Stop();
			IEnumerator shootingRoutine = Attack(objective);
			if (objective)
			{
				StartCoroutine(shootingRoutine);
			}
			 
			// Update status twice a second
			while (objective && IsInRange(objective))
			{
				yield return new WaitForSeconds(0.5f);
			}
			
			// Enemy dead or out of range, stop shooting
			StopCoroutine(shootingRoutine);
		}
		StopAttack();
		yield break;
	}
	
	IEnumerator StandAndAttack(Unit objective)
	{
		yield break;
	}

}
