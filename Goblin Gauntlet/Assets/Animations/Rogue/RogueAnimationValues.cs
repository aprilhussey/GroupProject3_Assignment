using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RogueAnimationValues : MonoBehaviour
{
    private float speed;
    private PlayerController playercontrol;
    private Rigidbody rb;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        playercontrol = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        speed = rb.velocity.magnitude;
        animator.SetFloat("speed", speed);
    }
}
