using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Unit : MonoBehaviour {

	// Internal components
	Selectable selectionMgr;
	UnitMovement movementMgr;
	PlayerController playerController;
	AttackController attackController;
	SpawnController spawnController;

	public int teamNumber = 0;
	public float initialHealth = 10f;
	public float maxHealth = 10f;

	float currentHealth;
	Slider healthSlider;

	IEnumerator attackRoutine;

	void Awake()
	{
		selectionMgr = GetComponent<Selectable> ();
		movementMgr = GetComponent<UnitMovement> ();
		playerController = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		attackController = GetComponent<AttackController> ();
		spawnController = GetComponent<SpawnController> ();

		currentHealth = initialHealth;
		healthSlider = GetComponentInChildren<Slider> ();
		if (healthSlider)
		{
			healthSlider.maxValue = maxHealth;
		}
	}

	public float TakeDamage(float damage, GameObject instigator)
	{
		currentHealth -= damage;

		if (healthSlider)
		{
			healthSlider.value = currentHealth;
		}

		if (currentHealth <= 0)
		{
			Destroy(gameObject);
		}

		return damage;
	}

	/**
	 * Selectable interfaces
	 */

	public void SetSelected(bool selected)
	{
		if (selectionMgr)
		{
			selectionMgr.SetSelected (selected);
		}
	}

	/**
	 * UnitMovement interfaces
	 */

	public bool Move(Vector3 destination)
	{
		if (attackController)
		{
			attackController.StopAttack();
		}

		if (attackRoutine != null)
		{
			StopCoroutine(attackRoutine);
			//attackRoutine = null;
		}

		return movementMgr ? movementMgr.Move (destination) : false;
	}

	/**
	 * Attack interfaces
	 */

	public void Attack(Unit objective)
	{
		if (objective && attackController && movementMgr && attackRoutine == null)
		{
			attackRoutine = ChaseAndAttack(objective);
			StartCoroutine(attackRoutine);
		}
	}

	/**
	 * Spawning interfaces
	 */

	public void SpawnUnit()
	{
		if (spawnController)
		{
			spawnController.SpawnUnit();
		}
	}

	void OnTriggerEnter(Collider other)
	{
		Unit unit = other.gameObject.GetComponent<Unit> ();
		if (unit && unit.teamNumber != teamNumber)
		{
			Attack(unit);
		}
	}

	void OnDestroy()
	{
		if (playerController)
		{
			playerController.UnitDestroyed(this);
		}
	}

	IEnumerator ChaseAndAttack(Unit objective)
	{
		while (objective)
		{
			while (objective && !attackController.IsInRange(objective))
			{
				movementMgr.Move (objective.transform.position);
				yield return null;
			}

			movementMgr.Stop();
			if (objective)
			{
				attackController.StartAttack(objective);
			}

			while (objective && attackController.IsInRange(objective))
			{
				yield return new WaitForSeconds(0.5f);
			}
			attackController.StopAttack();
		}
		attackRoutine = null;
		yield break;
	}
}
