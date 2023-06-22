using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class Tunneleffect : MonoBehaviour
{
    public float speed = 20;
    public float travelDistance = 20;

    public Vector3 initialPosition;

    public void Awake()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        transform.position += speed * -transform.forward * Time.deltaTime;

        if (Vector3.Distance(transform.position, initialPosition) > travelDistance)
        {
            transform.position = initialPosition;
        }
    }
}