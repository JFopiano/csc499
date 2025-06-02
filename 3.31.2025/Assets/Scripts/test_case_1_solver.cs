using System.Collections.Generic;
using UnityEngine;

public class SVMUnitySolver : MonoBehaviour
{
    public GameObject separatorCube; // The cube representing the separator
    public List<GameObject> redSpheres; // List of red spheres
    public List<GameObject> blueSpheres; // List of blue spheres
    public float expansionSpeed = 5f; // Speed of X scale expansion
    public float planeLengthZ = 20f; // Length of the plane in the Z direction
    public float fixedYScale = 1f; // Fixed Y scale of the separator cube
    public Color brighterRed = new Color(1f, 0.5f, 0.5f); // Brighter red color
    public Color brighterBlue = new Color(0.5f, 0.5f, 1f); // Brighter blue color

    private Vector3 decisionNormal; // Normal vector of the decision boundary
    private Vector3 decisionPoint; // A point on the decision boundary
    private float maxDistance; // Maximum distance from the decision point to any sphere
    private bool isInitialized = false; // Flag to track whether the SVM logic has been initialized

    // Public method to trigger SVM logic
    public void InitializeSVM()
    {
        if (isInitialized)
        {
            Debug.LogWarning("SVM has already been initialized.");
            return;
        }

        // Collect sphere positions and train the SVM
        List<Vector3> spherePositions = new List<Vector3>();
        List<int> labels = new List<int>();

        // Add red spheres (class -1)
        foreach (var sphere in redSpheres)
        {
            spherePositions.Add(sphere.transform.position);
            labels.Add(-1);
        }

        // Add blue spheres (class +1)
        foreach (var sphere in blueSpheres)
        {
            spherePositions.Add(sphere.transform.position);
            labels.Add(1);
        }

        // Train the SVM and calculate the initial separator
        TrainSVM(spherePositions, labels);

        // Visualize the initial separator
        VisualizeSeparator();

        isInitialized = true; // Set flag to true
    }

    void TrainSVM(List<Vector3> points, List<int> labels)
    {
        Vector3 redAverage = Vector3.zero;
        Vector3 blueAverage = Vector3.zero;
        int redCount = 0, blueCount = 0;

        for (int i = 0; i < points.Count; i++)
        {
            if (labels[i] == -1)
            {
                redAverage += points[i];
                redCount++;
            }
            else if (labels[i] == 1)
            {
                blueAverage += points[i];
                blueCount++;
            }
        }

        if (redCount > 0) redAverage /= redCount;
        if (blueCount > 0) blueAverage /= blueCount;

        decisionPoint = (redAverage + blueAverage) / 2;
        decisionNormal = (blueAverage - redAverage).normalized;

        maxDistance = 0f;
        foreach (var point in points)
        {
            float distance = Vector3.Distance(point, decisionPoint);
            if (distance > maxDistance)
            {
                maxDistance = distance;
            }
        }
    }

    void VisualizeSeparator()
    {
        if (separatorCube == null)
        {
            Debug.LogError("Separator cube is not assigned!");
            return;
        }

        separatorCube.transform.position = decisionPoint;

        Quaternion baseRotation = Quaternion.FromToRotation(Vector3.up, decisionNormal);
        Quaternion correctedRotation = baseRotation * Quaternion.Euler(0, 0, 90);
        separatorCube.transform.rotation = correctedRotation;

        separatorCube.transform.localScale = new Vector3(
            maxDistance * 0.5f,
            fixedYScale,
            planeLengthZ
        );
    }

    void Update()
    {
        if (isInitialized && Input.GetKey(KeyCode.Space))
        {
            ExpandSeparator();
        }

        if (isInitialized)
        {
            CheckContactingSpheres();
        }
    }

    void ExpandSeparator()
    {
        if (separatorCube != null)
        {
            float targetWidth = maxDistance * 2;
            Vector3 currentScale = separatorCube.transform.localScale;

            currentScale.x = Mathf.Lerp(currentScale.x, targetWidth, Time.deltaTime * expansionSpeed);
            currentScale.y = fixedYScale;
            currentScale.z = planeLengthZ;

            separatorCube.transform.localScale = currentScale;
        }
    }

    void CheckContactingSpheres()
    {
        Vector3 cubeCenter = separatorCube.transform.position;
        Vector3 cubeHalfExtents = separatorCube.transform.localScale / 2;

        Collider[] colliders = Physics.OverlapBox(cubeCenter, cubeHalfExtents, separatorCube.transform.rotation);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("RedSphere"))
            {
                Renderer renderer = collider.gameObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = brighterRed;
                }
            }
            else if (collider.gameObject.CompareTag("BlueSphere"))
            {
                Renderer renderer = collider.gameObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = brighterBlue;
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        if (separatorCube != null)
        {
            Gizmos.color = Color.yellow;
            Vector3 cubeCenter = separatorCube.transform.position;
            Vector3 cubeHalfExtents = separatorCube.transform.localScale / 2;
            Gizmos.matrix = Matrix4x4.TRS(cubeCenter, separatorCube.transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, cubeHalfExtents * 2);
        }
    }
}
