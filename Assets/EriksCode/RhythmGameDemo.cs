using TMPro;
using UnityEngine;

public class RhythmGameDemo : MonoBehaviour
{
    public GameObject movingTarget; // This is the object that will move from it's spwn to the target
    public GameObject target; // This is the target that the moving object will move towards
    public AudioSource songSource;

    public TextMeshProUGUI timeText; // This is the text that will display the current time and beat

    public int BPM = 60;

    private int previousBeat = -1;
    private double audioStartTime;

    // => this arrow shows that this is a "property".
    private double elapsedAudioTime => AudioSettings.dspTime - audioStartTime; // This is the time since the audio source started playing
    private float elapsedTimeMinutes => (float)(elapsedAudioTime / 60); // This is the time since the audio source started playing, in minutes
    private int currentBeat => Mathf.FloorToInt(elapsedTimeMinutes * BPM); // This is the current beat, rounded down

    private void Start()
    {
        double currentAudioTime = AudioSettings.dspTime; // This is the current time according to the audio source
        audioStartTime = (float)currentAudioTime + 3; // This is the time we want the audio source to start playing

        songSource.PlayScheduled(audioStartTime);

        UpdateMovingTarget();
    }

    private void Update()
    {
        // Use the Unity time for now, even though it WOULD desync from a song!
        // float time = Time.time;

        double time = elapsedAudioTime;  // This is the time since the audio source started playing

        // Convert it from seconds to minutes, e.g., 30 seconds = 0.5 minutes.
        // float toMinutes = elapsedTimeMinutes;

        // Convert from seconds to beat, rounding down (e.g., 45.67 seconds = beat 45 at 60 bpm).
        int beat = currentBeat;

        // If the beat has changed, update the color of the target.
        if (previousBeat != beat)
        {
            target.GetComponent<Renderer>().material.color = beat % 2 != 0 ? Color.red : Color.blue;

            previousBeat = beat;
        }

      
        
            UpdateMovingTarget();
      

        string output = $"Time: <b>{time:00:00.00}</b> Beat: <b>{beat:000}</b>";

        timeText.text = output;
    }

    //private bool haveSpawnedObject = false;
    private bool hasArrived = false;
    private double spawnTime;
    private float initialDistance;

    [Header("Moving Settings")]
    public float distance = 8;
    public float time = 4;
    private bool haveSpawnedObject = false;

    private void UpdateMovingTarget()
    {
        // wait until the first beat to start moving
        if (currentBeat == 0 && haveSpawnedObject == false)
        {
            spawnTime = elapsedAudioTime;
            haveSpawnedObject = true;
        }

        if (!hasArrived && haveSpawnedObject && currentBeat >= 0)
        {
            if (initialDistance == 0f)
            {
                initialDistance = Vector3.Distance(movingTarget.transform.position, target.transform.position);
                spawnTime = elapsedAudioTime;
            }

            float progress = (float)((Time.time - spawnTime) / time);
            float currentDistance = Mathf.Lerp(initialDistance, 0f, progress);

            movingTarget.transform.position = Vector3.Lerp(movingTarget.transform.position, target.transform.position, currentDistance / initialDistance);

            if (progress >= 1f)
            {
                double duration = elapsedAudioTime - spawnTime;
                Debug.Log($"It took: {duration} seconds to arrive.");
                hasArrived = true;
            }
        }
    }

}
