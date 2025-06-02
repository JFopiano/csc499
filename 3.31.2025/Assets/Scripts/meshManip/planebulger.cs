using System.Collections.Generic;
using UnityEngine;

public class PlaneBulger : MonoBehaviour
{
    private Mesh mesh;
    private List<Vector3> vertices;

    public float bulgeFactor = 50f; // Amplified factor to control the bulge intensity
    public Vector3 decisionPoint;   // A point on the decision boundary
    public Vector3 decisionNormal;  // Normal vector of the decision boundary

    public PlaneDeformer planeDeformer; // Reference to PlaneDeformer

    void Start()
    {
        // Ensure the plane mesh and vertices are initialized
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = new List<Vector3>(mesh.vertices);

        // Check if PlaneDeformer is assigned and initialized
        if (planeDeformer == null)
        {
            planeDeformer = GetComponent<PlaneDeformer>();
            if (planeDeformer == null)
            {
                Debug.LogError("PlaneDeformer is missing from the same GameObject!");
            }
        }
    }

    public void BulgePlane()
    {
        Debug.Log("BulgePlane method triggered");

        if (mesh == null || vertices == null)
        {
            Debug.LogError("Mesh or vertices are not initialized.");
            return;
        }

        if (planeDeformer == null)
        {
            Debug.LogError("PlaneDeformer is not assigned!");
            return;
        }

        List<Vector3> updatedVertices = new List<Vector3>();

        foreach (var vertex in vertices)
        {
            // Calculate the distance of the vertex from the decision boundary
            float distanceToBoundary = Vector3.Dot(decisionNormal, vertex - decisionPoint);

            // Apply amplified deformation to make the effect more visible
            float deformation = bulgeFactor * distanceToBoundary;

            Vector3 updatedVertex = vertex;
            updatedVertex.y += deformation; // Apply amplified deformation

            updatedVertices.Add(updatedVertex);

            Debug.Log($"Original Vertex: {vertex}, Updated Vertex: {updatedVertex}"); // Log vertex changes
        }

        // Update the mesh with the new vertices
        mesh.vertices = updatedVertices.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        GetComponent<MeshCollider>().sharedMesh = mesh;

        // Update the positions of the dots via PlaneDeformer
        planeDeformer.UpdateVertices(updatedVertices);

        Debug.Log("Plane successfully bulged.");
    }
}