using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    public float speed = 2f;
    Vector3 moveDirection;
    Rigidbody platformRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        moveDirection = Vector3.left;
        platformRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(moveDirection.x * speed, moveDirection.y * speed, moveDirection.z);
        platformRigidbody.velocity = movement;
    }

    private void OnTriggerEnter(Collider other)
    {
        print("trigger entered!");
        if (other.gameObject.CompareTag("TurnLeft"))
        {
           switch(moveDirection)
            {
                case Vector3 moveDirection when moveDirection.Equals(Vector3.left):
                    moveDirection = Vector3.up;
                    break;
                case Vector3 moveDirection when moveDirection.Equals(Vector3.up):
                    moveDirection = Vector3.right;
                    break;
                case Vector3 moveDirection when moveDirection.Equals(Vector3.right):
                    moveDirection = Vector3.down;
                    break;
                case Vector3 moveDirection when moveDirection.Equals(Vector3.down):
                    moveDirection = Vector3.left;
                    break;
            }
        }
        else if (other.gameObject.CompareTag("TurnRight"))
        {
            switch (moveDirection)
            {
                case Vector3 moveDirection when moveDirection.Equals(Vector3.left):
                    moveDirection = Vector3.down;
                    break;
                case Vector3 moveDirection when moveDirection.Equals(Vector3.down):
                    moveDirection = Vector3.right;
                    break;
                case Vector3 moveDirection when moveDirection.Equals(Vector3.right):
                    moveDirection = Vector3.up;
                    break;
                case Vector3 moveDirection when moveDirection.Equals(Vector3.up):
                    moveDirection = Vector3.left;
                    break;
            }
        }
    }
}
