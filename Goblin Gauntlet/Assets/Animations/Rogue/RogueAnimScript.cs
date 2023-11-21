using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RogueAnimScript : MonoBehaviour
{
    private float speed;
    private PlayerController PlayerControl;
    private Rigidbody rb;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        PlayerControl = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    //Update is called once per frame
    void Update()
    {
        if (PlayerControl.health <= 0)
        {
            animator.SetBool("Dead", true);
        }

        speed = rb.velocity.magnitude;
        animator.SetFloat("speed", speed);
    }
}
