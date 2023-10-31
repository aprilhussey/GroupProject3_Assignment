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
    public GameObject paladinBeam;
    float attackTime;
    private float paladinAttackTime = 0.9f;
    private float rogueAttackTime = 0.2f;
    private float warlockAttackTime = 0.5f;
    private float clericAttackTime = 0.7f;
    private float paladinSpecialCooldown = 1200;
    private float rogueSpecialCooldown = 600;
    private float warlockSpecialCooldown = 900;
    private float clericSpecialCooldown = 600;
    float specialCooldown;
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
                specialCooldown = paladinSpecialCooldown;
                break;
            case 1:
                attackCollider = rogueAttackCollider;
                attackTime = rogueAttackTime;
                specialCooldown = rogueSpecialCooldown;
                break;
            case 2:
                attackCollider = paladinAttackCollider;
                attackTime = warlockAttackTime;
                specialCooldown = warlockSpecialCooldown;
                break;
            case 3:
                attackCollider = clericAttackCollider;
                attackTime = clericAttackTime;
                specialCooldown = clericSpecialCooldown;
                break;
        }
        attackCollider.enabled = false;
    }

    private void Update()
    {
        if (specialCooldown > 0)
        {
            specialCooldown -= 1;
        }
    }
    void OnAttack()
    {
        if (!attacking)
        {
            StartCoroutine(AttackCooldown());
        }
    }

    void OnSpecialAttack()
    {
        if (specialCooldown == 0)
        {
            switch (classID)
            {
                case 0:
                    StartCoroutine(PaladinSpecialAttack());
                    break;
                case 1:
                    StartCoroutine(RogueSpecialAttack());
                    break;
                case 2:
                    StartCoroutine(WarlockSpecialAttack());
                    break;
                case 3:
                    StartCoroutine(ClericSpecialAttack());
                    break;
            }
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

    IEnumerator PaladinSpecialAttack()
    {
        print("special attacking");
        specialCooldown = 1200;
        GameObject childObject = Instantiate(paladinBeam) as GameObject;
        childObject.transform.parent = this.transform;
        childObject.transform.position = this.transform.position + new Vector3(0, 1.5f, 10.5f);
        yield return new WaitForSeconds(3);
        Destroy(childObject);
        print("special attack done");
    }

    IEnumerator RogueSpecialAttack()
    {
        yield return new WaitForSeconds(5);
    }

    IEnumerator WarlockSpecialAttack()
    {
        yield return new WaitForSeconds(5);
    }

    IEnumerator ClericSpecialAttack()
    {
        yield return new WaitForSeconds(5);
    }
}
