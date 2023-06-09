using UnityEngine;

public class RingScaler : MonoBehaviour
{
    public Material ringMaterial; // The material for the ring shape
    public float animationDuration = 0.5f; // The duration of the scaling animation in seconds
    public float startingRadius = 0.5f; // The starting radius of the ring shape
    public float width = 0.1f; // The width between the outer and inner radius of the ring shape

    private GameObject ring; // Reference to the ring shape
    private float targetScaleMultiplier; // Target scale multiplier for the ring shape
    public Vector3 targetPosition; // Target position for the ring shape
    public Vector3 targetScale;
    private float animationStartTime; // Start time of the scaling animation
    

    public void InitializeRing(float scaleMultiplier, Vector3 position)
    {
        // Create the ring shape GameObject
        ring = CreateRingShape();

        // Set the target scale multiplier and position for the ring shape
        targetScaleMultiplier = scaleMultiplier;
        targetPosition = position;

        // Rotate the ring 90 degrees on the x-axis
        ring.transform.Rotate(90f, 0f, 0f);

        // Start the scaling animation
        StartScalingAnimation();
    }

    private void Update()
    {
        if(ring != null)
        {
            // Perform the scaling animation
            ScaleRingShape();
        }
    }

    public GameObject CreateRingShape()
    {
        // Create an empty GameObject for the ring shape
        GameObject ring = new GameObject("RingShape");

        // Add a MeshFilter and MeshRenderer components to render the ring shape
        MeshFilter meshFilter = ring.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = ring.AddComponent<MeshRenderer>();

        // Set the material for the ring shape
        meshRenderer.material = ringMaterial;

        // Create a new mesh for the ring shape
        Mesh mesh = new Mesh();

        // Set up the vertices, triangles, and normals for the ring shape
        int segments = 32; // Number of segments in the ring

        Vector3[] vertices = new Vector3[segments * 2];
        int[] triangles = new int[segments * 6];
        Vector3[] normals = new Vector3[segments * 2];

        float angleStep = 360f / segments;
        float angle = 0f;

        for (int i = 0; i < segments; i++)
        {
            // Calculate the vertex positions for the inner and outer edges of the ring
            Vector3 innerVertex = Quaternion.Euler(0f, angle, 0f) * Vector3.forward * (startingRadius - width * 0.5f);
            Vector3 outerVertex = Quaternion.Euler(0f, angle, 0f) * Vector3.forward * (startingRadius + width * 0.5f);

            // Set the vertex positions for the inner and outer edges of the ring
            vertices[i] = innerVertex;
            vertices[i + segments] = outerVertex;

            // Set up the triangles for the ring shape
            int triangleIndex = i * 6;

            triangles[triangleIndex] = i;
            triangles[triangleIndex + 1] = (i + 1) % segments;
            triangles[triangleIndex + 2] = i + segments;

            triangles[triangleIndex + 3] = (i + 1) % segments;
            triangles[triangleIndex + 4] = (i + 1) % segments + segments;
            triangles[triangleIndex + 5] = i + segments;

            // Set up the normals for the ring shape
            normals[i] = Vector3.up;
            normals[i + segments] = Vector3.up;

            angle += angleStep;
        }

        // Assign the vertices, triangles, and normals to the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;

        // Assign the mesh to the MeshFilter component
        meshFilter.mesh = mesh;

        return ring;
    }

    public void StartScalingAnimation()
    {
        // Record the start time of the animation
        animationStartTime = Time.time;
    }

    public void ScaleRingShape()
    {
        // Calculate the elapsed time since the start of the animation
        float elapsedTime = Time.time - animationStartTime;

        // Calculate the interpolation factor based on the elapsed time and animation duration
        float t = Mathf.Clamp01(elapsedTime / animationDuration);

        // Interpolate the scale from the initial scale (scaled by targetScaleMultiplier) to the target scale
        Vector3 scale = Vector3.Lerp(Vector3.one * targetScaleMultiplier, Vector3.one, t);

        // Set the scale of the ring shape
        ring.transform.localScale = scale;

        // Set the position of the ring shape to match the target position
        ring.transform.position = targetPosition;
    }
}
