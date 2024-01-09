using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BrightnessController : MonoBehaviour
{
	[SerializeField]
	private Slider brightnessSlider = null;
	[SerializeField]
	private TMP_Text brightnessText = null;

	[SerializeField]
	private Image darkOverlay = null;

	private SettingsButtonManager settingsButtonManager = null;

	// Awake is called before Start
	void Awake()
	{
		if (brightnessSlider != null)
		{
			LoadValues();
			BrightnessSlider();
		}
	}
	void Start()
	{
		settingsButtonManager = this.gameObject.GetComponent<SettingsButtonManager>();
	}

	// Update is called once per frame
	void Update()
	{
		if (brightnessSlider != null)
		{
			SaveBrightness();
		}
	}

	public void BrightnessSlider()
	{
		brightnessText.text = (brightnessSlider.value).ToString("0") + "%";
	}

	public void SaveBrightness()
	{
		float brightnessValue = brightnessSlider.value;
		PlayerPrefs.SetFloat("ImageBrightness", brightnessValue);
		LoadValues();
	}

	public void LoadValues()
	{
		float brightnessValue = PlayerPrefs.GetFloat("ImageBrightness");
		brightnessSlider.value = brightnessValue;

		// Cover the brightness value to a transparency value
		float transparencyValue = 1f - brightnessValue / 100f;

		darkOverlay.color = new Color(darkOverlay.color.r, darkOverlay.color.g, darkOverlay.color.b, transparencyValue);
	}

	public float GetCurrentBrightnessValue()
	{
		float brightnessValue = brightnessSlider.value;
		return brightnessValue;
	}

	public void SetBrightnessValueToBeforeChange()
	{
		//float brightnessValue = SettingsButtonManager.Instance.beforeChangeBrightnessValue;
		float brightnessValue = settingsButtonManager.beforeChangeBrightnessValue;
		PlayerPrefs.SetFloat("ImageBrightness", brightnessValue);
		LoadValues();
	}

	public void OnSubmit(BaseEventData eventData)
	{
		// Clear selected button
		EventSystem.current.SetSelectedGameObject(null);
		// Set selected button
		//EventSystem.current.SetSelectedGameObject(SettingsButtonManager.Instance.brightnessButton);
		EventSystem.current.SetSelectedGameObject(settingsButtonManager.brightnessButton);
	}

	public void OnCancel(BaseEventData eventData)
	{
		SetBrightnessValueToBeforeChange();

		// Clear selected button
		EventSystem.current.SetSelectedGameObject(null);
		// Set selected button
		//EventSystem.current.SetSelectedGameObject(SettingsButtonManager.Instance.brightnessButton);
		EventSystem.current.SetSelectedGameObject(settingsButtonManager.brightnessButton);
	}
}
