using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraColorController : MonoBehaviour
{
    public List<Camera> cameras;
    public List<Color> colors;
    public Metronome metronome;
    public int colorChangeInterval = 4;

    private int CurrentColorIndex = 0;
    private int lastBeat = -1;
    private Color currentColor;
    
    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(lastBeat != metronome.currentBeat)
        {
            lastBeat = metronome.currentBeat;
            if(lastBeat % colorChangeInterval == 0)
            {
                CurrentColorIndex++;
                if(CurrentColorIndex >= colors.Count)
                {
                    CurrentColorIndex = 0;
                }
                currentColor = colors[CurrentColorIndex];
                foreach(Camera camera in cameras)
                {
                    camera.backgroundColor = currentColor;
                }
            }
        }


    }
}
