using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentMover : MonoBehaviour
{
    public Transform[] points;
    public float speed = 5f;
    private int pointIndex = 0;

    private void Start()
    {
        transform.position = points[pointIndex].transform.position;
    }

    private void Update()
    {
        //Debug.Log(points[pointIndex].name);
        if(pointIndex <= points.Length - 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, points[pointIndex].transform.position, speed * Time.deltaTime);

            if (transform.position == points[pointIndex].transform.position)
            {
                pointIndex += 1;
            }
        }
    }
}
