using UnityEngine;
using UnityEngine.InputSystem;

public class playerMove : MonoBehaviour
{

    public float speed = 10.0f;
    private Vector3 direction;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    void OnMove(InputValue value)
    {
        Vector2 inputVector = value.Get<Vector2>();
        rb.velocity = new Vector3(Mathf.Lerp(0, inputVector.x * speed, 0.8f), 0, Mathf.Lerp(0, inputVector.y * speed, 0.8f));
        if (rb.velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }
}
