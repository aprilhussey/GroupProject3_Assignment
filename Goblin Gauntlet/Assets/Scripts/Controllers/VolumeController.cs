using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField]
    private Slider volumeSlider = null;
    [SerializeField]
    private TMP_Text volumeText = null;

    // Awake is called before Start
    void Awake()
    {
        if (volumeSlider != null)
        {
            LoadValues();
            VolumeSlider();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (volumeSlider != null)
        {
            SaveVolume();
        }
    }

    public void VolumeSlider()
    {
        volumeText.text = (volumeSlider.value).ToString("0") + "%";
    }

    public void SaveVolume()
    {
        float volumeValue = volumeSlider.value;
        PlayerPrefs.SetFloat("GameVolume", volumeValue);
        LoadValues();
    }

    public void LoadValues()
    {
		float volumeValue = PlayerPrefs.GetFloat("GameVolume");
		volumeSlider.value = volumeValue;
		AudioListener.volume = volumeValue;
	}

	public void OnSubmit(BaseEventData eventData)
	{
		// Clear selected button
		EventSystem.current.SetSelectedGameObject(null);
		// Set selected button
		EventSystem.current.SetSelectedGameObject(GameManager.Instance.volumeButton);
	}
}
