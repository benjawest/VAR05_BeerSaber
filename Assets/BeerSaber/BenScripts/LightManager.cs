using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    // public list of lights
    public List<GameObject> lightTunnels = new List<GameObject>();
    public int LightDirectionChangeFrequency = 4;
    public Metronome metronome;
    public int currentBeat;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int newbeat = metronome.currentBeat;
        if (newbeat != currentBeat)
        {
            currentBeat = newbeat;

            // if metronome.currentbeat is a modulu of LightDirectionChangeFrequency
            // change the direction of the lights
            if (currentBeat % LightDirectionChangeFrequency == 0)
            {
                // loop through all the lights
                foreach (GameObject lightTunnel in lightTunnels)
                {
                    // Only do this 1/4 of the time
                    if (Random.Range(0, 4) == 0)
                    {
                        // get the lightTunnel script
                        LightTunnel lightTunnelScript = lightTunnel.GetComponent<LightTunnel>();
                        // change the direction of the lightTunnel
                        lightTunnelScript.ChangeDirection();
                    }


                }
            }

        }
        
    }
}
