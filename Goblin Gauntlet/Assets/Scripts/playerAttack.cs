using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAttack : MonoBehaviour
{
    BoxCollider attackCollider;
    public BoxCollider paladinAttackCollider;
    public BoxCollider rogueAttackCollider;
    public GameObject warlockCastShot;
    public BoxCollider clericAttackCollider;
    float attackTime;
    private float paladinAttackTime = 0.9f;
    private float rogueAttackTime = 0.2f;
    private float warlockAttackTime = 0.5f;
    private float clericAttackTime = 0.7f;
    public bool attacking = false;
    private Rigidbody rb;
    public float classID = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        switch (classID)
        {
            case 0:
                attackCollider = paladinAttackCollider;
                attackTime = paladinAttackTime;
                return;
            case 1:
                attackCollider = rogueAttackCollider;
                attackTime = rogueAttackTime;
                return;
            case 2:
                attackCollider = paladinAttackCollider;
                attackTime = warlockAttackTime;
                return;
            case 3:
                attackCollider = clericAttackCollider;
                attackTime = clericAttackTime;
                return;
        }
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
        if (classID == 2)
        {
            GameObject childObject = Instantiate(warlockCastShot) as GameObject;
            childObject.transform.parent = this.transform;
            childObject.transform.position = this.transform.position + new Vector3(0, 2, 1);
        }
        else
        {
            attackCollider.enabled = true;
        }
        yield return new WaitForSeconds(attackTime);
        attackCollider.enabled = false;
        attacking = false;
        print("attack done!");
    }
}
