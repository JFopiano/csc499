using System.Collections.Generic;
using UnityEngine;

public class PlaneGenerator : MonoBehaviour
{
    public Material planeMaterial; // Reference to the material
    
    [Header("Starting Position")]
    public float startingPosX;
    public float startingPosY;
    public float startingPosZ;

    public void GeneratePlane(Vector3 size, int resolution)
    {
        Debug.Log("Generating plane...");

        // Check if the GameObject already has a MeshFilter component
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
        }

        // Check if the GameObject already has a MeshRenderer component
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }

        // Create Vertices
        var vertices = new List<Vector3>();
        float xPerStep = size.x / resolution;
        float yPerStep = size.y / resolution;
        Vector3 offset = new Vector3(size.x / 2, 0, size.y / 2);

        for (int y = 0; y <= resolution; y++)
        {
            for (int x = 0; x <= resolution; x++)
            {
                vertices.Add(new Vector3(x * xPerStep, 0, y * yPerStep) - offset);
            }
        }

        // Create Triangles
        var triangles = new List<int>();

        for (int row = 0; row < resolution; row++)
        {
            for (int column = 0; column < resolution; column++)
            {
                triangles.Add(row * (resolution + 1) + column);
                triangles.Add((row + 1) * (resolution + 1) + column);
                triangles.Add(row * (resolution + 1) + column + 1);

                triangles.Add((row + 1) * (resolution + 1) + column);
                triangles.Add((row + 1) * (resolution + 1) + column + 1);
                triangles.Add(row * (resolution + 1) + column + 1);
            }
        }

        // Create Mesh
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();


        Debug.Log("Plane generated successfully.");
    }

    public void DestroyPlane()
    {
        Debug.Log("Destroying plane...");

        // Remove the MeshFilter component if it exists
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            Destroy(meshFilter);
        }

        // Remove the MeshRenderer component if it exists
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            Destroy(meshRenderer);
        }

        Debug.Log("Plane destroyed successfully.");
    }
    void Start()
    {
        transform.localPosition = new Vector3(startingPosX, startingPosY, startingPosZ); // Set the starting position
        transform.rotation = Quaternion.Euler(0, -90, -450); // Rotate the plane to stand up
        GeneratePlane(new Vector3(5, 5, 1), 10);
    }
}
