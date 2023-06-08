using UnityEngine;

public class BeatMapLevelManager : MonoBehaviour
{
    public BeatMapLevel levelData;
    public Transform gridParent; // Parent transform for the grid
    public float spacing = 1f; // Spacing between the notes

    private int currentBeat = -1;
    private GameObject[] spawnedNotes;
    public GameObject targetPrefab;
    public Metronome metronome;

    private void Update()
    {
        int newBeat = metronome.currentBeat;
        if (newBeat != currentBeat)
        {
            currentBeat = newBeat;

            // Destroy previously spawned notes
            DestroySpawnedNotes();

            // Find the slice that matches the current beat
            BeatMapLevel.Slice currentSlice = null;
            foreach (BeatMapLevel.Slice slice in levelData.slices)
            {
                if (slice.spawnBeat == currentBeat)
                {
                    currentSlice = slice;
                    break;
                }
            }

            if (currentSlice != null)
            {
                // Spawn the notes for the current slice
                SpawnNotes(currentSlice);
            }
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

    private GameObject SpawnNoteObject(BeatMapLevel.Note note, string noteObjectName)
    {
        if (!note.isEmpty)
        {
            // Spawn the note object based on the note data
            GameObject noteObject = Instantiate(targetPrefab, GetSpawnPosition(noteObjectName), targetPrefab.transform.rotation, gridParent);

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

    private Vector3 GetSpawnPosition(string noteObjectName)
    {
        // Calculate the position offset based on the note object name
        string[] splitName = noteObjectName.Split('_');
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

     private string GetNoteObjectName(int row, int col)
    {
        return $"Note_{row}_{col}";
    }

    private void OnDrawGizmos()
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

}
