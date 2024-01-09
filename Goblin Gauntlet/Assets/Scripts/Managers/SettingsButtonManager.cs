using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SettingsButtonManager : MonoBehaviour
{
	// Singleton instance
	//public static SettingsButtonManager Instance = null;

	[Header("Options Buttons")]
	public GameObject settingsCanvas;
	public GameObject settingsFirstButton;
	public GameObject settingsClosedButton;

	public GameObject volumeButton;
	[SerializeField]
	private GameObject volumeSlider;

	public GameObject brightnessButton;
	[SerializeField]
	private GameObject brightnessSlider;

	[HideInInspector]
	public float beforeChangeVolumeValue;
	[HideInInspector]
	public float beforeChangeBrightnessValue;

	// Awake is called before Start
	void Awake()
	{
		// Ensure only one instance exists
		/*if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}*/
	}

	public void OnSoundButtonClick()
	{
		VolumeController volumeController = this.gameObject.GetComponent<VolumeController>();
		beforeChangeVolumeValue = volumeController.GetCurrentVolumeValue();

		// Clear selected button
		EventSystem.current.SetSelectedGameObject(null);
		// Set selected button
		EventSystem.current.SetSelectedGameObject(volumeSlider);
	}

	public void OnBrightnessButtonClick()
	{
		BrightnessController brightnessController = this.gameObject.GetComponent<BrightnessController>();
		beforeChangeBrightnessValue = brightnessController.GetCurrentBrightnessValue();

		// Clear selected button
		EventSystem.current.SetSelectedGameObject(null);
		// Set selected button
		EventSystem.current.SetSelectedGameObject(brightnessSlider);
	}
}
