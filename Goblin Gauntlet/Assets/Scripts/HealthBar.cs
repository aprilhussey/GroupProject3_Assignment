using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	private Slider slider;

	// Awake is called before Start
	private void Awake()
	{
		slider = this.GetComponent<Slider>();
	}

	public void SetMaxHealth(float health)
	{
		if (slider != null)
		{
			slider.maxValue = health;
			slider.value = health;
		}
	}

	public void SetHealth(float health)
	{
		if (slider != null)
		{
			slider.value = health;
		}
	}
}
