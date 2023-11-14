using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactController : MonoBehaviour, IDamageable
{
	public Entity entityData;

	private string entityName;
	[HideInInspector] public float health;

	// Awake is called before Start
	void Awake()
	{
		// Access character data - Entity.cs
		entityName = entityData.entityName;
		health = entityData.health;
	}

    // Update is called once per frame
    void Update()
    {
		// If artifact health is less than or equal to 0
		if (health <= 0)
		{
			Debug.Log("Artifact destroyed");
		}

		Debug.Log($"{gameObject.name} health = {health}");
    }

	// Class needs to derive from 'IDamageable' for this function to work
	public void TakeDamage(float amount)
	{
		if (health > 0)
		{
			health -= amount;
		}
	}
}
