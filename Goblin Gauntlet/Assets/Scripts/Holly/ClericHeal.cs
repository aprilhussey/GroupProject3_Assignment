using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClericHeal : MonoBehaviour
{
    PlayerController playerController;
    // Update is called once per frame
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController = other.GetComponent<PlayerController>();
            StartCoroutine(PlayerHeal(playerController));
        }
    }

    IEnumerator PlayerHeal(PlayerController healthScript)
    {
        if (healthScript.canHeal && healthScript.currentHealth <= healthScript.characterData.health)
        {
            healthScript.canHeal = false;
            healthScript.currentHealth += healthScript.currentHealth / 10;
            yield return new WaitForSeconds(1);
        }
    }
}
