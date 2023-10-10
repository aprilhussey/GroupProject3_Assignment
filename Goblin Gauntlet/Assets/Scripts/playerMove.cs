using UnityEngine;
using UnityEngine.InputSystem;

public class playerMove : MonoBehaviour
{

    public float speed = 0.01f;
    float maxSpeed;
    float currentSpeed;
    Rigidbody rb;
    private Vector2 movementInput = new Vector2();
    PlayerInput playerInput;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = new PlayerInput();
        playerInput.Enable();
        playerInput.Player.Move.performed += context => movementInput = context.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentSpeed = speed;
        maxSpeed = currentSpeed;
    }

    void OnMove(InputValue input)
    {
        print("movement");
        Vector3 moveInput = new Vector3 (input.Get<Vector2>().x, rb.velocity.y, input.Get<Vector2>().y);
        rb.velocity = new Vector3(
            Mathf.Lerp(0, moveInput.x * currentSpeed, 0.8f),
            rb.velocity.y,
            Mathf.Lerp(0, moveInput.y * currentSpeed, 0.8f)
            );
        if (rb.velocity != Vector3.zero)
        {
            transform.forward = rb.velocity;
        }
    }
}
