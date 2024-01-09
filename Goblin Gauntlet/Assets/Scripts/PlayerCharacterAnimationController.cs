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

    bool basicAttacking = false;
    bool specialAttacking = false;

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
		if (playerController.basicAttackState == Ability.AbilityState.active)
		{
			basicAttacking = true;
		}
		else
		{
			basicAttacking = false;
		}

        if (playerController.specialAbilityState == Ability.AbilityState.active)
        {
            specialAttacking = true;
        }
        else
        {
            specialAttacking = false;
        }

        if (playerController.currentHealth <= 0)
        {
            animator.SetBool("Dead", true);
        }

        speed = playerRigidbody.velocity.magnitude;
        //Debug.Log("speed; "+ speed);
        animator.SetFloat("speed", speed);
        animator.SetBool("basicAttacking", basicAttacking);
        animator.SetBool("specialAttacking", specialAttacking);
    }
}
