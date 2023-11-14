using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockCast : MonoBehaviour
{
    public float speed = 10.0f;
    public Vector3 facingDirection;
    WarlockCastAim warlockCastAim;
    WarlockCastCollide warlockCastCollide;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;
        warlockCastAim = GetComponentInChildren<WarlockCastAim>();
        warlockCastCollide = GetComponentInChildren<WarlockCastCollide>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        facingDirection = warlockCastAim.faceDirection;
        transform.rotation = Quaternion.LookRotation(facingDirection);
        rb.velocity = transform.forward * speed;
        if (warlockCastCollide.hitDetected)
        {
            Destroy(gameObject);
        }
    }
}
