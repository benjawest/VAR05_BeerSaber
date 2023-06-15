using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "newBeatMapLevel", menuName = "Game/BeatMap Grid")]
public class BeatMapLevel : ScriptableObject
{
    public enum NoteColor { Red = 0, Blue = 1 }

    [Serializable]
    public class Slice
    {
        public int spawnBeat; // The beat at which the slice will spawn
        public int gridSize = 2; // The size of the grid for the slice
        public Note[] notes; // 1D array to store the notes

        public Slice()
        {
            notes = new Note[gridSize * gridSize];
        }

        public override string ToString()
        {
            return $"Beat {spawnBeat}";
        }
    }

    [Serializable]
    public class Note
    {
        public NoteColor color;
        public bool isEmpty = true;
    }

    public int numberOfSlices = 1;
    public int noteFrequency = 4;
    public int gridSize = 2;
    public Slice[] slices = new Slice[0];

    public void GenerateSlices()
    {
        slices = new Slice[numberOfSlices];
        System.Random random = new System.Random();

        for (int i = 0; i < numberOfSlices; i++)
        {
            Slice slice = new Slice();
            slice.spawnBeat = i * noteFrequency;
            slice.gridSize = gridSize;
            slice.notes = new Note[slice.gridSize * slice.gridSize];

            // Generate a note for the slice
            Note note = new Note();
            note.color = (NoteColor)random.Next(0, 2);
            note.isEmpty = false;

            // Select a random position for the normal note within the slice
            int randomPosition = random.Next(0, slice.gridSize * slice.gridSize);
            slice.notes[randomPosition] = note;

            slices[i] = slice;
        }
    }


    public void DeleteSlices()
    {
        slices = new Slice[0];
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(BeatMapLevel))]
public class BeatMapLevelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BeatMapLevel beatMapLevel = (BeatMapLevel)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Generate Slices"))
        {
            Undo.RecordObject(beatMapLevel, "Generate Slices");
            beatMapLevel.GenerateSlices();
            EditorUtility.SetDirty(beatMapLevel);
        }

        if (GUILayout.Button("Delete Slices"))
        {
            Undo.RecordObject(beatMapLevel, "Delete Slices");
            beatMapLevel.DeleteSlices();
            EditorUtility.SetDirty(beatMapLevel);
        }
    }
}
#endif
