using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private float maxHealth;
    private float currentHealth;

    [SerializeField] private ArtifactController artifactController;
    [SerializeField] private GoblinController goblinController;

    void Awake()
    {
        artifactController = GetComponentInParent<ArtifactController>();

		goblinController = GetComponentInParent<GoblinController>();

		slider = GetComponent<Slider>();
    }

    void Start()
    {
        if (artifactController != null)
        {
			maxHealth = artifactController.health;
		}

		if (goblinController != null)
		{
			maxHealth = goblinController.health;
		}
	}

    // Update is called once per frame
    void Update()
    {
        if (artifactController != null)
        {
            currentHealth = artifactController.health;
        }
		
        if (goblinController != null)
		{
			currentHealth = goblinController.health;
		}
	}

	public void UpdateHealthBar()
	{
        slider.value = currentHealth / maxHealth;

        Debug.Log($"currentHealth = {currentHealth}");
        Debug.Log($"maxhealth = {maxHealth}");
        Debug.Log($"slider value = {slider.value}");

    }
}
