using TMPro;
using UnityEngine;

public class Metronome : MonoBehaviour
{
    public AudioSource songSource;
    public TextMeshProUGUI timeText; // This is the text that will display the current time and beat
    public int BPM = 150;
    private int previousBeat = -1;
    private double audioStartTime;
    public float BeatDuration => 60f / BPM; // This is the duration of a single beat, in seconds

    // => this arrow shows that this is a "property".
    private double elapsedAudioTime => AudioSettings.dspTime - audioStartTime; // This is the time since the audio source started playing
    private float elapsedTimeMinutes => (float)(elapsedAudioTime / 60); // This is the time since the audio source started playing, in minutes
    public int currentBeat => Mathf.FloorToInt(elapsedTimeMinutes * BPM); // This is the current beat, rounded down

    public bool isPaused;
    public bool songCompletePlay = false;

    private void Start()
    {
        

    }

    private void Update()
    {
        if(!isPaused) // If the metronome is not paused, update the time and beat
        {
            if (!songSource.isPlaying) // If the song is done playing, stop the metronome and show the song complete screen
            {
                songCompletePlay = true;
            }
            
            double time = elapsedAudioTime;  // This is the time since the audio source started playing

            // Convert from seconds to beat, rounding down (e.g., 45.67 seconds = beat 45 at 60 bpm).
            int beat = currentBeat;

            // If the beat has changed, update the color of the target.
            if (previousBeat != beat)
            {
                previousBeat = beat; // Update the previous beat to the current beat
            }

            string output = $"Time: <b>{time:00:00.00}</b> Beat: <b>{beat:000}</b>";
            timeText.text = output;
        }
    }

    public void StartMetronome()
    {
        double currentAudioTime = AudioSettings.dspTime; // This is the current time according to the audio source
        audioStartTime = (float)currentAudioTime + 3; // Applying an offset to the start time to account for the engine to start up first
        songSource.PlayScheduled(audioStartTime); // Play the audio source at the scheduled start time
        isPaused = false;
    }

    public void RestartMetronome() // Not tested yet
    {
        songSource.Stop();
        StartMetronome();
    }

}
