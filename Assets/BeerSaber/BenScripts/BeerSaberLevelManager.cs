using UnityEngine;

public class BeerSaberLevelManager : MonoBehaviour
{
    public BeerSaberScriptableObject levelData;
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
            BeerSaberScriptableObject.Slice currentSlice = null;
            foreach (BeerSaberScriptableObject.Slice slice in levelData.slices)
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

    private void SpawnNotes(BeerSaberScriptableObject.Slice slice)
    {
        spawnedNotes = new GameObject[4];

        // Spawn the notes based on the slice data
        spawnedNotes[0] = SpawnNoteObject(slice.topRight, "TopRightNote");
        spawnedNotes[1] = SpawnNoteObject(slice.topLeft, "TopLeftNote");
        spawnedNotes[2] = SpawnNoteObject(slice.bottomRight, "BottomRightNote");
        spawnedNotes[3] = SpawnNoteObject(slice.bottomLeft, "BottomLeftNote");
    }

    private GameObject SpawnNoteObject(BeerSaberScriptableObject.Note note, string noteObjectName)
    {
        if (!note.IsEmpty)
        {
            // Spawn the note object based on the note data
            GameObject noteObject = Instantiate(targetPrefab, GetSpawnPosition(noteObjectName), targetPrefab.transform.rotation, gridParent);

            // Set the color of the note object based on the note color
            Renderer renderer = noteObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                if (note.Color == BeerSaberScriptableObject.Color.Red)
                {
                    renderer.material.color = Color.red;
                }
                else if (note.Color == BeerSaberScriptableObject.Color.Blue)
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
        switch (noteObjectName)
        {
            case "TopRightNote":
                return gridParent.TransformPoint(new Vector3(spacing, 0f, spacing));
            case "TopLeftNote":
                return gridParent.TransformPoint(new Vector3(-spacing, 0f, spacing));
            case "BottomRightNote":
                return gridParent.TransformPoint(new Vector3(spacing, 0f, -spacing));
            case "BottomLeftNote":
                return gridParent.TransformPoint(new Vector3(-spacing, 0f, -spacing));
            default:
                return gridParent.position;
        }
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

    private void OnDrawGizmos()
    {
        if (gridParent != null)
        {
            Gizmos.color = Color.yellow;

            // Draw the gizmos for each note spawn position
            for (int i = 0; i < 4; i++)
            {
                string noteObjectName = GetNoteObjectName(i);
                Vector3 spawnPosition = GetSpawnPosition(noteObjectName);

                Gizmos.DrawWireCube(spawnPosition, Vector3.one * 0.5f);
            }
        }
    }

    private string GetNoteObjectName(int index)
    {
        // Get the note object name based on the index
        switch (index)
        {
            case 0:
                return "TopRightNote";
            case 1:
                return "TopLeftNote";
            case 2:
                return "BottomRightNote";
            case 3:
                return "BottomLeftNote";
            default:
                return string.Empty;
        }
    }
}
