using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShot : MonoBehaviour
{
    [SerializeField] private float laserSpeed = 10f;
    [SerializeField] private float destroyDelay = 5f;

    // Start is called before the first frame update
    void Start()
    {
        // Destroy the laser projectile after the specified delay
        Destroy(gameObject, destroyDelay);
    }


    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * laserSpeed;
    }
}
