using TMPro;
using UnityEngine;

public class RhythmTargetManager: MonoBehaviour
{
    public GameObject movingTarget; // This is the object that will move from it's spwn to the target
    public GameObject target; // This is the target that the moving object will move towards
    public AudioSource songSource;

    public TextMeshProUGUI timeText; // This is the text that will display the current time and beat

    public int BPM = 60;

    private int previousBeat = -1;
    private double audioStartTime;
    private Vector3 initialPosition;

    // => this arrow shows that this is a "property".
    private double elapsedAudioTime => AudioSettings.dspTime - audioStartTime; // This is the time since the audio source started playing
    private float elapsedTimeMinutes => (float)(elapsedAudioTime / 60); // This is the time since the audio source started playing, in minutes
    private int currentBeat => Mathf.FloorToInt(elapsedTimeMinutes * BPM); // This is the current beat, rounded down

    private void Start()
    {
        double currentAudioTime = AudioSettings.dspTime; // This is the current time according to the audio source
        audioStartTime = (float)currentAudioTime + 3; // Applying an offset to the start time to account for the engine to start up first
        songSource.PlayScheduled(audioStartTime); // Play the audio source at the scheduled start time

        initialPosition = movingTarget.transform.position; // Save the initial position of the moving target
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

            previousBeat = beat; // Update the previous beat to the current beat
        }


        UpdateMovingTarget();

        string output = $"Time: <b>{time:00:00.00}</b> Beat: <b>{beat:000}</b>";

        timeText.text = output;
    }

    private bool hasArrived = false;
    private double spawnTime;
    private bool haveSpawnedObject = false;
    private float elapsedTime = 0f;


    [Header("Moving Settings")]
    public float time = 4f;


    private void UpdateMovingTarget()
    {
        // wait until the first beat to start moving
        if (currentBeat == 0 && haveSpawnedObject == false)
        {
            spawnTime = elapsedAudioTime;
            haveSpawnedObject = true;
        }

        // move the object
        if (!hasArrived && haveSpawnedObject && currentBeat >= 0)
        {
            // Calculate the normalized progress based on the elapsed time and the specified time
            float normalizedProgress = Mathf.Clamp01(elapsedTime / time);

            // Move the object towards the target position using Lerp
            movingTarget.transform.position = Vector3.Lerp(initialPosition, target.transform.position, normalizedProgress);

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            if (normalizedProgress >= 1f)
            {
                double duration = elapsedAudioTime - spawnTime;
                Debug.Log($"It took: {duration} seconds to arrive.");
                hasArrived = true;
            }
        }
    }

}
