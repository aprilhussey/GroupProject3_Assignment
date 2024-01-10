using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeathOverlay : MonoBehaviour
{
    public GameObject confirmButton;
    public GameObject hud;

    // Start is called before the first frame update
    void Start()
    {
        hud.SetActive(false);
        EventSystem.current.SetSelectedGameObject(confirmButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
