using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class IrisVisualizer : MonoBehaviour
{
    public GameObject spherePrefab;
    public IrisDataLoader dataLoader;
    public List<GameObject> dataPoints = new List<GameObject>();
    private Vector2 minValues, maxValues;
    private const float PLANE_Z_POSITION = -60.4f;  // Match your plane's Z position
    private const float PLANE_X_POSITION = -236.3f; // Match your plane's X position
    private const float PLANE_Y_POSITION = -0.4f;   // Match your plane's Y position
    
    void Start()
    {
        Debug.Log("IrisVisualizer Start called");
        if (spherePrefab == null || dataLoader == null)
        {
            Debug.LogError("Required references are missing!");
            return;
        }

        if (dataLoader.allIrisData.Count == 0)
        {
            Debug.LogError("No Iris data available.");
            return;
        }
    }

    void CalculateDataRanges(int xFeature, int yFeature)
    {
        minValues = new Vector2(float.MaxValue, float.MaxValue);
        maxValues = new Vector2(float.MinValue, float.MinValue);

        foreach (var iris in dataLoader.allIrisData)
        {
            float xValue = GetFeatureValue(iris, xFeature);
            float yValue = GetFeatureValue(iris, yFeature);

            minValues.x = Mathf.Min(minValues.x, xValue);
            minValues.y = Mathf.Min(minValues.y, yValue);
            maxValues.x = Mathf.Max(maxValues.x, xValue);
            maxValues.y = Mathf.Max(maxValues.y, yValue);
        }
        Debug.Log($"Data ranges - Min: ({minValues.x}, {minValues.y}), Max: ({maxValues.x}, {maxValues.y})");
    }

    public void UpdateVisualization(int xFeature, int yFeature, string species1, string species2)
    {
        Debug.Log($"Updating visualization: X={xFeature}, Y={yFeature}, Species1={species1}, Species2={species2}");
        Debug.Log($"Using plane position: X={PLANE_X_POSITION}, Y={PLANE_Y_POSITION}, Z={PLANE_Z_POSITION}");
        
        // Clean up existing visualization
        foreach (var point in dataPoints)
        {
            Destroy(point);
        }
        dataPoints.Clear();

        // Calculate ranges for selected features
        CalculateDataRanges(xFeature, yFeature);

        var filteredData = dataLoader.allIrisData
            .Where(iris => iris.species == species1 || iris.species == species2)
            .ToList();

        Debug.Log($"Plotting {filteredData.Count} data points");

        foreach (var iris in filteredData)
        {
            float xValue = GetFeatureValue(iris, xFeature);
            float yValue = GetFeatureValue(iris, yFeature);

            // Calculate normalized position relative to the plane
            Vector3 position = new Vector3(
                (NormalizeValue(xValue, minValues.x, maxValues.x) * 10 - 5) + PLANE_X_POSITION,
                (NormalizeValue(yValue, minValues.y, maxValues.y) * 10 - 5) + PLANE_Y_POSITION,
                PLANE_Z_POSITION
            );

            GameObject sphere = Instantiate(spherePrefab, position, Quaternion.identity, transform);
            sphere.transform.localScale = Vector3.one * 0.3f; // Increased scale for better visibility

            Renderer sphereRenderer = sphere.GetComponent<Renderer>();
            if (sphereRenderer != null)
            {
                sphereRenderer.material.color = GetColorForSpecies(iris.species);
                Debug.Log($"Created sphere at position: {position} for species: {iris.species}");
            }
            else
            {
                Debug.LogError("Sphere prefab is missing Renderer component!");
            }

            dataPoints.Add(sphere);
        }
    }

    float NormalizeValue(float value, float min, float max)
    {
        if (Mathf.Approximately(min, max))
        {
            return 0.5f; // Default to middle if min and max are the same
        }
        return (value - min) / (max - min);
    }

    Color GetColorForSpecies(string species)
    {
        switch (species.ToLower())
        {
            case "iris-setosa": return Color.red;
            case "iris-versicolor": return Color.green;
            case "iris-virginica": return Color.blue;
            default: return Color.white;
        }
    }

    float GetFeatureValue(IrisData iris, int featureIndex)
    {
        switch (featureIndex)
        {
            case 0: return iris.sepalLength;
            case 1: return iris.sepalWidth;
            case 2: return iris.petalLength;
            case 3: return iris.petalWidth;
            default: return 0f;
        }
    }
}