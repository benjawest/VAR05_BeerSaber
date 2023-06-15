using UnityEngine;

public class BeatMapLevelManager : MonoBehaviour
{
    public BeatMapLevel levelData;
    public Transform gridParent; // Parent transform for the grid
    public float spacing = 1f; // Spacing between the notes

    public GameObject notePrefab;
    public GameObject ringPrefab;
    public Metronome metronome;

    private int currentBeat = -1;
    private GameObject[] spawnedNotes;
    private GameObject spawnedRing;
    public int beatDistance = 2; // Number of beats before the notes to spawn the ring
    private float ringScaleDuration; // Duration to scale down the ring
    private float ringElapsedTime; // Elapsed time for scaling the ring
    private bool isScalingRing = false; // Flag to indicate if ring scaling is in progress
    private int newBeat;
    // Scale factor for the ring
    public float ringInitScaleFactor = 3f;
    public float ringFinalScaleFactor = 0.5f;

    private void Update()
    {
        newBeat = metronome.currentBeat;
        if (newBeat != currentBeat) // If the beat has changed
        {
            currentBeat = newBeat;
            DestroySpawnedNotes();
            NewBeatSpawnObjects();   
        }

        if (isScalingRing)
        {
            ScaleRing(); 
        }
    }

    // Spawn the notes and rings for the current slice
    private void NewBeatSpawnObjects()
    {
        BeatMapLevel.Slice currentSlice = null;
        BeatMapLevel.Slice nextSlice = null;
        // Find the slices that match current and next beat to spawn the notes and ring
        foreach (BeatMapLevel.Slice slice in levelData.slices)
        {
            if (slice.spawnBeat == currentBeat)
            {
                currentSlice = slice;
                break;
            }
            else if (slice.spawnBeat == currentBeat + beatDistance)
            {
                nextSlice = slice;
            }
        }

        if (currentSlice != null)
        {
            Debug.Log($"Spawning Notes on Beat: {currentBeat}");
            SpawnNotes(currentSlice); // Spawn the notes for the current slice
        }

        if (nextSlice != null)
        {
            
            SpawnRing(nextSlice); // Spawn the ring for the next slice
        }
    }

    private void ScaleRing()
    {
        ringElapsedTime += Time.deltaTime;
        if (ringElapsedTime <= ringScaleDuration)
        {
            float t = ringElapsedTime / ringScaleDuration; // Calculate progress (0 to 1)
            float currentScale = Mathf.Lerp(ringInitScaleFactor, ringFinalScaleFactor, t); // Interpolate the scale based on progress

            if (spawnedRing != null)
            {
                spawnedRing.transform.localScale = new Vector3(currentScale, currentScale, currentScale);
            }
            else
            {
                Debug.Log("Ring object is null, nothing to scale");
            }
        }
        else
        {
            // Ring scaling is complete
            DestroySpawnedRing();
            isScalingRing = false;
        }
    }

    private void SpawnNotes(BeatMapLevel.Slice slice)
    {
        int gridSize = 2; // Assuming a 2x2 grid

        spawnedNotes = new GameObject[gridSize * gridSize];

        // Spawn the notes based on the slice data
        int index = 0;
        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                BeatMapLevel.Note note = null;

                if (row == 0 && col == 0)
                {
                    note = slice.topLeft;
                }
                else if (row == 0 && col == 1)
                {
                    note = slice.topRight;
                }
                else if (row == 1 && col == 0)
                {
                    note = slice.bottomLeft;
                }
                else if (row == 1 && col == 1)
                {
                    note = slice.bottomRight;
                }

                spawnedNotes[index] = SpawnNoteObject(note, $"Note_{row}_{col}");
                index++;
            }
        }
    }

    private void SpawnRing(BeatMapLevel.Slice slice)
    {
        // What is the beat we are spawning the ring on?
        int noteBeat = currentBeat + beatDistance;
        Debug.Log($"Spawning ring at beat: {currentBeat} for slice: {noteBeat}");

        // Find the slice that matches the note beat
        BeatMapLevel.Slice nextSlice = null;
        foreach (BeatMapLevel.Slice noteSlice in levelData.slices)
        {
            if (noteSlice.spawnBeat == noteBeat)
            {
                nextSlice = noteSlice;
                break;
            }
        }

        if (nextSlice != null)
        {
            // Find the row and column to spawn the ring
            int row = -1;
            int col = -1;

            if (!nextSlice.topLeft.isEmpty)
            {
                row = 0;
                col = 0;
            }
            else if (!nextSlice.topRight.isEmpty)
            {
                row = 0;
                col = 1;
            }
            else if (!nextSlice.bottomLeft.isEmpty)
            {
                row = 1;
                col = 0;
            }
            else if (!nextSlice.bottomRight.isEmpty)
            {
                row = 1;
                col = 1;
            }

            if (row != -1 && col != -1)
            {
                spawnedRing = SpawnRingObject(nextSlice, row, col);
                ringScaleDuration = metronome.BeatDuration;
                ringElapsedTime = 0f;
                isScalingRing = true;

                Debug.Log($"Ring Spawned at Beat: {currentBeat}");
            }
            else { Debug.Log("No Ring Spawned"); } 
        }
        else
        { Debug.Log("Next Slice not found");}
    }

    private GameObject SpawnNoteObject(BeatMapLevel.Note note, string noteObjectName)
    {
        if (!note.isEmpty)
        {
            // Spawn the note object based on the note data
            GameObject noteObject = Instantiate(notePrefab, GetSpawnPosition(noteObjectName), notePrefab.transform.rotation, gridParent);

            // Set the color of the note object based on the note color
            Renderer renderer = noteObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                if (note.color == BeatMapLevel.NoteColor.Red)
                {
                    renderer.material.color = Color.red;
                }
                else if (note.color == BeatMapLevel.NoteColor.Blue)
                {
                    renderer.material.color = Color.blue;
                }
            }
            return noteObject;
        }
        return null;
    }

    private GameObject SpawnRingObject(BeatMapLevel.Slice slice, int row, int col)
    {
        // Spawn outer Ring
        GameObject ringObject = Instantiate(ringPrefab, GetSpawnPosition($"Note_{row}_{col}"), ringPrefab.transform.rotation, gridParent);
        return ringObject;
    }

    private Vector3 GetSpawnPosition(string noteObjectName)
    {
        // Calculate the position offset based on the note object name
        string[] splitName = noteObjectName.Split('_'); // Split the name by underscore
        // Note name format: Note_{row}_{col}
        if (splitName.Length == 3 && int.TryParse(splitName[1], out int row) && int.TryParse(splitName[2], out int col))
        {
            return gridParent.TransformPoint(new Vector3(col * spacing, 0f, row * spacing));
        }
        return gridParent.position;
    }

    private void DestroySpawnedNotes()
    {
        if (spawnedNotes != null)
        {
            foreach (GameObject noteObject in spawnedNotes)
            {
                if (noteObject != null)
                {
                    Destroy(noteObject);
                }
            }
        }
    }

    private void DestroySpawnedRing()
    {
        if (spawnedRing != null)
        {
            Destroy(spawnedRing);
        }
    }

    private void OnDrawGizmos() // Debug function to draw gizmos in the editor for where the notes will spawn
    {
        if (gridParent != null)
        {
            Gizmos.color = Color.yellow;

            // Draw the gizmos for each note spawn position
            for (int row = 0; row < 2; row++)
            {
                for (int col = 0; col < 2; col++)
                {
                    string noteObjectName = GetNoteObjectName(row, col);
                    Vector3 spawnPosition = GetSpawnPosition(noteObjectName);

                    Gizmos.DrawWireCube(spawnPosition, Vector3.one * 0.5f);
                }
            }
        }
    }

    private string GetNoteObjectName(int row, int col)
    {
        return $"Note_{row}_{col}";
    }
}
