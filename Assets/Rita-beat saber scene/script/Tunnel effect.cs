using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Tunneleffect : MonoBehaviour
{
    public float speed = 10;
    public float travel distance = 20;

    public Vector3 initial position;

    private void Update()

    {
        transform.position += speed * transform.forward * Time.deltaTime;

        if (Vector3.Distance(transform.position, initialPosition) > travelDistance)
        {
            transform.position = initialPosition;
        }
    }
