using System;
using UnityEngine;

[CreateAssetMenu(fileName = "newBeerSaberLevel", menuName = "Beer Saber/New Level")]
public class BeerSaberScriptableObject : ScriptableObject
{
    // A "level" in Beat Saber can be roughly defined as a sequence of
    // 4x3 grids, with each grid cell optionally containing a note.
    // So in a sense, we can think of a level is a 3D grid of cubes (notes).
    // This is a "sparse" grid, in that not every cell is filled (in fact,
    // the overwhelming majority of possible cells would not be).

    // So our level "playback" should check if there is a "slice" of this
    // 3D grid that corresponds to the current time, and if so, spawn the objects in.

    public enum Color { Red = 0, Blue = 1 }

    [Serializable]
    public class Slice
    {
        public int spawnBeat;

        public Note topRight;
        public Note topLeft;
        public Note bottomRight;
        public Note bottomLeft;

        public override string ToString()
        {
            return $"Beat {spawnBeat}";
        }
    }

    [Serializable]
    public class Note
    {
        public Color Color;
        public bool IsEmpty; // Indicates if this note is empty (no object spawned)
    }

    public Slice[] slices;
}
