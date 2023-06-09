using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTunnel : MonoBehaviour
{
   // direction this lightTunnel is rotating
   public int direction = 1;
   public float speed = 10;

    private void Start()
    {
        // Set the direction of the lightTunnel randomly
        direction = Random.Range(0, 2) == 0 ? -1 : 1;
    }

    private void FixedUpdate()
    {
        // Rotate the lightTunnel on the z axis based on the direction
        transform.Rotate(0, 0, speed * direction * Time.fixedDeltaTime);
    }

    public void ChangeDirection()
    {
          // change the direction of the lightTunnel
        direction = direction * -1;
    }

}
