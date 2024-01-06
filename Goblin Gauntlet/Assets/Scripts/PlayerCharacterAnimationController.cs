using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerCharacterAnimationController : MonoBehaviour
{
    private float speed;
    private PlayerController playerController;
    private Rigidbody playerRigidbody;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    //Update is called once per frame
    void Update()
    {
        if (playerController.currentHealth <= 0)
        {
            animator.SetBool("Dead", true);
        }

        speed = playerRigidbody.velocity.magnitude;
        //Debug.Log("speed; "+ speed);
        animator.SetFloat("speed", speed);
        animator.SetBool("basicAttacking", playerController.basicAttack.attacking);
        animator.SetBool("speacialAttacking", playerController.specialAbility.attacking);
    }
}
