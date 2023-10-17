using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAttack : MonoBehaviour
{
    private BoxCollider attackCollider;
    private float attackTime = 0.5f;
    public bool attacking = false;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        attackCollider = GetComponentInChildren<BoxCollider>();
        attackCollider.enabled = false;
    }
    void OnAttack()
    {
        if (!attacking)
        {
            StartCoroutine(AttackCooldown());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            print("Hit detected");
        }
    }

    IEnumerator AttackCooldown()
    {
        print("attacking");
        attacking = true;
        attackCollider.enabled = true;
        yield return new WaitForSeconds(attackTime);
        attackCollider.enabled = false;
        attacking = false;
        print("attack done!");
    }
}
