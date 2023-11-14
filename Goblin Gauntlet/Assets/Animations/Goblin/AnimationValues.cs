using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AnimationValues : MonoBehaviour
{
    private float speed;
    private GoblinController goblincontrol;
    private Rigidbody rb;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        goblincontrol = GetComponent<GoblinController>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (goblincontrol.health <= 0)
        {
            animator.SetBool("Dead", true);
        }

        speed = rb.velocity.magnitude;
        animator.SetFloat("speed", speed);
    }
}
