using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockCastCollide : MonoBehaviour
{
    public bool hitDetected = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            print("hit detected");
            GameObject.Destroy(this.transform.parent);
            hitDetected = true;
        }
    }
}
