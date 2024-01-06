using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            other.gameObject.GetComponent<GoblinController>().currentHealth = 0;
        }
        else
        {
            other.gameObject.GetComponent<PlayerController>().currentHealth = 0;
        }
    }
}
