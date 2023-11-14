using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockCastAim : MonoBehaviour
{
    public bool homing = false;
    public GameObject target;
    public Vector3 faceDirection;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            Vector3 targetDirection = target.transform.position - transform.position;
            faceDirection = Vector3.RotateTowards(transform.forward, targetDirection, 5 * Time.deltaTime, 0.0f);
        }
        else
        {
            faceDirection = Vector3.forward;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && homing == false)
        {
            target = other.gameObject;
            homing = true;
        }
    }
}
