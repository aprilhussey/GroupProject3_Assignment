using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectPrefabBehaviour : MonoBehaviour
{
    private Rigidbody prefabRigidbody;
    public float gravityMultipler = 9999f;

    // Start is called before the first frame update
    void Start()
    {
        prefabRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 extraGravity = Physics.gravity * gravityMultipler;
        prefabRigidbody.AddForce(extraGravity, ForceMode.Acceleration);
    }
}
