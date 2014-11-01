using UnityEngine;
using System.Collections;

public class AttackController : MonoBehaviour {

	// Damage caused to victim, before mitigations
	public float AttackDamage = 1f;

	// Max distance from target
	public float attackRange = 1f;

	// Attack fire rate (rounds/sec)
	public float fireRate = 3f;

	LineRenderer	gunLine;
	Light			gunLight;
	float			effectsTime = 0.1f;

	IEnumerator attackRoutine;

	void Awake()
	{
		gunLine = GetComponentInChildren<LineRenderer>();
		gunLight = GetComponentInChildren<Light>();
	}

	public bool IsInRange(Unit objective)
	{
		return Vector3.Distance(transform.position, objective.transform.position) < attackRange;
	}

	public void StartAttack(Unit objective)
	{
		attackRoutine = Attack(objective);
		StartCoroutine(attackRoutine);
	}

	public void StopAttack()
	{
		if (attackRoutine != null)
		{
			StopCoroutine(attackRoutine);
		}
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
	}

	void OnDestroy()
	{
		StopAttack();
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

}
