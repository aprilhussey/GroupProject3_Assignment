using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerMove : MonoBehaviour
{

    public float speed = 10.0f;
    private Vector3 direction;
    private Rigidbody rb;
    public CapsuleCollider capsule;
    playerAttack attackScript;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        attackScript = GetComponentInChildren<playerAttack>();
        capsule.enabled = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!attackScript.attacking)
        {
            rb.velocity = direction * speed;
            if (rb.velocity != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rb.velocity), 0.15f);
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
        
    }

    void OnMove(InputValue value)
    {
        Vector2 inputVector = value.Get<Vector2>();
        direction = new Vector3(Mathf.Lerp(0, inputVector.x, 0.8f), 0, Mathf.Lerp(0, inputVector.y, 0.8f));
    }
}
