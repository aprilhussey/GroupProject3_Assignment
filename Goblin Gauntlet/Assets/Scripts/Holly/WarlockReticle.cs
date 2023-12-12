using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WarlockReticle : MonoBehaviour
{
    float cooldownTime = 3;
    Boolean reticleUsed = false;
    public GameObject warlockRing;
    Rigidbody rb;
    Vector2 inputVector;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ReticleCooldown());
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTime -= 1;
        transform.Translate(new Vector3(inputVector.x, 0, inputVector.y) * 10 * Time.deltaTime);
        if (reticleUsed)
        {
            transform.parent.GetComponent<PlayerController>().canMove = true;
            Destroy(gameObject);
        }
    }

    void OnAltMovement(InputValue value)
    {
        print("moving");
        inputVector = value.Get<Vector2>();
    }

    void OnSpecialAttack()
    {
        GameObject warlockFlame = Instantiate(warlockRing, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity, transform) as GameObject;
        warlockFlame.transform.localScale = new Vector3(1, 200, 1);
        reticleUsed = true;
    }

    IEnumerator ReticleCooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        GameObject warlockFlame = Instantiate(warlockRing, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity, transform) as GameObject;
        warlockFlame.transform.localScale = new Vector3(1, 200, 1);
        reticleUsed = true;
    }
}
