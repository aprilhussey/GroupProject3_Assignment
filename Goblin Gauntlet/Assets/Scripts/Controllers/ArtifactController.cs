using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArtifactController : MonoBehaviour, IDamageable
{
	public Entity entityData;

	private string entityName;
	[HideInInspector] 
	public float maxHealth;
	public float currentHealth;

	private HealthBar healthBar;

	// Particle Spark
	public ParticleSystem artefactSpark;

	//Defeat Screen
	public GameObject deathScreen;
	public GameObject VictoryScreen;

	// Awake is called before Start
	void Awake()
	{
		// Access character data - Entity.cs
		entityName = entityData.entityName;
		maxHealth = entityData.health;
		currentHealth = entityData.health;

		healthBar = GameObject.Find("ArtifactHealthBar").GetComponentInChildren<HealthBar>();
	}

	// Start is called before the first frame
	void Start()
	{
		healthBar.SetMaxHealth(maxHealth);
	}

    // Update is called once per frame
    void Update()
    {
		// If artifact health is less than or equal to 0
		if (currentHealth <= 0)
		{
			deathScreen.SetActive(true);
			Debug.Log("Artifact destroyed");
			//Destroy(gameObject);
		}

		Debug.Log($"{gameObject.name} health = {currentHealth}");
    }

	// Class needs to derive from 'IDamageable' for this function to work
	public void TakeDamage(float amount)
	{
		if (currentHealth > 0)
		{
			artefactSpark.Play();
			currentHealth -= amount;

			healthBar.SetHealth(currentHealth);
		}
	}
}
