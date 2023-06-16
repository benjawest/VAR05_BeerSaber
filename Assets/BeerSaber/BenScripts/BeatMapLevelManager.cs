using System;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;

public class BeatMapLevelManager : MonoBehaviour
{
    public BeatMapLevel levelData;
    public Transform gridParent;
    public float spacing = 1f;
    public GameObject notePrefab;
    public GameObject ringPrefab;
    public Metronome metronome;
    public int beatDistance = 2;
    public float ringInitScaleFactor = 3f;
    public float ringFinalScaleFactor = 0.5f;
    public TextMeshProUGUI missCountText;
    public TextMeshProUGUI hitCountText;
    public int hitCount = 0;
    public int missCount = 0;
    public GameObject particleSystemPrefab; // Reference to the particle system prefab
    public float particleDuration = 1f; // Duration of the particle system before it's destroyed


    private int currentBeat = -1;
    private GameObject[] spawnedNotes;
    private GameObject spawnedRing;
    private float ringScaleDuration;
    private float ringElapsedTime;
    private bool isScalingRing = false;
    private int newBeat;

    private void Update()
    {
        newBeat = metronome.currentBeat;
        if (newBeat != currentBeat)
        {
            currentBeat = newBeat;
            DestroySpawnedNotes();
            NewBeatSpawnObjects();
        }

        // Check if the player has pressed the space bar
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("Space bar was pressed");
            // Destroy any spawned notes and add one to the hit count for each note destroyed
            DebugDestroyNotes();

        }

        if (isScalingRing)
        {
            ScaleRing();
        }
    }

    private void DebugDestroyNotes()
    {
        if (spawnedNotes != null)
        {
            foreach (GameObject noteObject in spawnedNotes)
            {
                if (noteObject != null)
                {
                    hitCount++;
                    if(hitCountText != null)
                    {
                        hitCountText.text = $"Hits: {hitCount.ToString("D3")}";
                    }
                    else
                    {
                        Debug.Log("Hit count text is null");
                    }
                    Destroy(noteObject);
                }
            }
        }
    }

    private void NewBeatSpawnObjects()
    {
        BeatMapLevel.Slice currentSlice = null;
        BeatMapLevel.Slice nextSlice = null;

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
            SpawnNotes(currentSlice);
        }

        if (nextSlice != null)
        {
            SpawnRing(nextSlice);
        }
    }

    private void ScaleRing()
    {
        ringElapsedTime += Time.deltaTime;
        if (ringElapsedTime <= ringScaleDuration)
        {
            float t = ringElapsedTime / ringScaleDuration;
            float currentScale = Mathf.Lerp(ringInitScaleFactor, ringFinalScaleFactor, t);

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
            DestroySpawnedRing();
            isScalingRing = false;
        }
    }

    private void SpawnNotes(BeatMapLevel.Slice slice)
    {
        int gridSize = levelData.gridSize;
        spawnedNotes = new GameObject[gridSize * gridSize];

        int index = 0;
        for (int i = 0; i < gridSize * gridSize; i++)
        {
            BeatMapLevel.Note note = slice.notes[i];
            if (!note.isEmpty)
            {
                spawnedNotes[index] = SpawnNoteObject(note, i);
                index++;
            }
        }
    }

    private void SpawnRing(BeatMapLevel.Slice slice)
    {
        int noteBeat = currentBeat + beatDistance;
        Debug.Log($"Spawning ring at beat: {currentBeat} for slice: {noteBeat}");

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
            int index = -1;

            for (int i = 0; i < nextSlice.notes.Length; i++)
            {
                if (!nextSlice.notes[i].isEmpty)
                {
                    index = i;
                    break;
                }
            }

            if (index != -1)
            {
                spawnedRing = SpawnRingObject(nextSlice, index);
                ringScaleDuration = metronome.BeatDuration;
                ringElapsedTime = 0f;
                isScalingRing = true;
                Debug.Log($"Ring Spawned at Beat: {currentBeat}");
            }
            else
            {
                Debug.Log("No Ring Spawned");
            }
        }
        else
        {
            Debug.Log("Next Slice not found");
        }
    }



    private GameObject SpawnNoteObject(BeatMapLevel.Note note, int index)
    {
        GameObject noteObject = Instantiate(notePrefab, GetSpawnPosition(index), notePrefab.transform.rotation, gridParent);
        Renderer renderer = noteObject.GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.material.color = note.color == BeatMapLevel.NoteColor.Red ? Color.red : Color.blue;
        }

        return noteObject;
    }

    private GameObject SpawnRingObject(BeatMapLevel.Slice slice, int index)
    {
        GameObject ringObject = Instantiate(ringPrefab, GetSpawnPosition(index), ringPrefab.transform.rotation, gridParent);
        return ringObject;
    }

    private Vector3 GetSpawnPosition(int index)
    {
        int gridSize = levelData.gridSize;
        int row = index / gridSize;
        int col = index % gridSize;
        return gridParent.TransformPoint(new Vector3(col * spacing, 0f, row * spacing));
    }

    private void DestroySpawnedNotes()
    {
        if (spawnedNotes != null)
        {
            foreach (GameObject noteObject in spawnedNotes)
            {
                if (noteObject != null)
                {
                    // Update missCountText to have the format "Miss: {missCount}"
                    missCount++;
                    if( missCountText != null)
                    {
                        missCountText.text = $"Miss: {missCount.ToString("D3")}";
                    }
                    else {Debug.Log("Miss count text is null");}
                    // instantiate a particle system at the noteObject's position
                    SpawnParticleSystem(noteObject.transform.position);
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

    private void OnDrawGizmos()
    {
        if (gridParent != null)
        {
            Gizmos.color = Color.yellow;
            int gridSize = levelData.gridSize;

            for (int i = 0; i < gridSize * gridSize; i++)
            {
                Vector3 spawnPosition = GetSpawnPosition(i);
                Gizmos.DrawWireCube(spawnPosition, Vector3.one * 0.5f);
            }
        }
    }

    private void SpawnParticleSystem(Vector3 position)
    {
        GameObject particleSystemObject = Instantiate(particleSystemPrefab, position, Quaternion.identity);
        ParticleSystem particleSystem = particleSystemObject.GetComponent<ParticleSystem>();

        // Customize the particle system properties if needed
        // For example, you can modify particleSystem.startColor to change the color of the particles

        particleSystem.Play(); // Start playing the particle system

        StartCoroutine(DestroyParticleSystem(particleSystemObject, particleDuration));
    }

    private IEnumerator DestroyParticleSystem(GameObject particleSystemObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(particleSystemObject);
    }

}
