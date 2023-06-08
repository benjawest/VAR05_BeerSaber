using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "newBeatMapLevel", menuName = "Game/Beat Map Level")]
public class BeatMapLevel : ScriptableObject
{
    public enum NoteColor { Red = 0, Blue = 1 }

    [Serializable]
    public class Slice
    {
        public int spawnBeat;

        public Note topRight = new Note();
        public Note topLeft = new Note();
        public Note bottomRight = new Note();
        public Note bottomLeft = new Note();

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

    public int numberOfSlices = 0;
    public int noteFrequency = 4;
    public Slice[] slices = new Slice[0];

    public void GenerateSlices()
    {
        slices = new Slice[numberOfSlices];

        System.Random random = new System.Random();

        // Generate normal slices with matching spawn beats
        for (int i = 0; i < numberOfSlices; i++)
        {
            Slice slice = new Slice();
            slice.spawnBeat = i * noteFrequency;

            // Generate a normal note for the slice
            Note Note = new Note();
            Note.color = (NoteColor)random.Next(0, 2);
            Note.isEmpty = false;

            // Select a random position for the normal note within the slice
            int randomPosition = random.Next(0, 4);
            switch (randomPosition)
            {
                case 0:
                    slice.topLeft = Note;
                    break;
                case 1:
                    slice.topRight = Note;
                    break;
                case 2:
                    slice.bottomLeft = Note;
                    break;
                case 3:
                    slice.bottomRight = Note;
                    break;
            }

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
