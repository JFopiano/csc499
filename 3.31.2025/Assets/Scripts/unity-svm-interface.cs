using UnityEngine;
using System.Diagnostics;
using System.IO;
using System;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System.Collections;  // Add this if not present

public class SVMInterface : MonoBehaviour
{
    public GameObject separationPlane;
    public IrisVisualizer irisVisualizer;
    public FeatureSelector featureSelector;
    
    [System.Serializable]
    public class SeparationPlaneData
    {
        public float[] normal;
        public float[] point;
        public float margin;
        public float bias;
    }

    void Start()
    {
        ValidateReferences();
    }

    public void ValidateReferences()
    {
        if (separationPlane == null)
            UnityEngine.Debug.LogError($"Separation plane reference is missing on {gameObject.name}");
        if (irisVisualizer == null)
            UnityEngine.Debug.LogError($"IrisVisualizer reference is missing on {gameObject.name}");
        if (featureSelector == null)
            UnityEngine.Debug.LogError($"FeatureSelector reference is missing on {gameObject.name}");
    }
    
    public void SolveCurrentLevel()
    {
        if (featureSelector == null)
        {
            UnityEngine.Debug.LogError("FeatureSelector reference is missing!");
            return;
        }

        try 
        {
            int[] selectedFeatures = new int[] {
                featureSelector.xAxisDropdown.value,
                featureSelector.yAxisDropdown.value
            };
            
            string result = CallPythonSolver(selectedFeatures);
            
            if (string.IsNullOrEmpty(result))
            {
                UnityEngine.Debug.LogError("No response from Python solver");
                return;
            }

            SeparationPlaneData planeData = JsonUtility.FromJson<SeparationPlaneData>(result);
            UpdateSeparationLine(planeData);
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError($"Error in SolveCurrentLevel: {e.Message}\n{e.StackTrace}");
        }
    }

public void UpdateSeparationLine(SeparationPlaneData planeData)
{
    try
    {
        // Separate points into two clusters
        List<Vector3> cluster1 = new List<Vector3>();
        List<Vector3> cluster2 = new List<Vector3>();
        List<Vector3> cluster3 = new List<Vector3>();

        
        foreach (GameObject point in irisVisualizer.dataPoints)
        {
            if (point.GetComponent<Renderer>().material.color == Color.red)
            {
                cluster1.Add(point.transform.position);
            }
            else if (point.GetComponent<Renderer>().material.color == Color.blue)
            {
                cluster2.Add(point.transform.position);
            }
            else if (point.GetComponent<Renderer>().material.color == Color.green)
            {
                cluster3.Add(point.transform.position);
            }
        }

        // Structure to store point pairs and their distances
        var closestPairs = new List<(Vector3 point1, Vector3 point2, float distance)>();

        bool openSpace = true;
        // Find all pairs and their distances
        if (cluster1.Count == 0 && cluster2.Count == 0)
        {
            UnityEngine.Debug.LogError("Both cluster1 and cluster2 are empty. Cannot proceed.");
            return;
        }

        if (cluster1.Count == 0)
        {
            cluster1 = cluster3;
        }

        if (cluster2.Count == 0)
        {
            cluster2 = cluster3;
        }

        foreach (Vector3 point1 in cluster1)
        {
            foreach (Vector3 point2 in cluster2)
            {
                float distance = Vector3.Distance(
                    new Vector3(point1.x, point1.y, 0),
                    new Vector3(point2.x, point2.y, 0)
                );
                if(distance <= 0){
                    openSpace = false;  // Fixed the == to =
                }
                UnityEngine.Debug.Log("distance " + distance);
                
                closestPairs.Add((point1, point2, distance));
            }
        }

        closestPairs.Sort((a, b) => a.distance.CompareTo(b.distance));

        // Declare variables outside the if/else blocks
        Vector3 avgPoint1;
        Vector3 avgPoint2;
        float avgDistance;

        // Sort pairs by distance and process accordingly

            var top3Pairs = closestPairs.Take(3).ToList();

            // Calculate average position and distance from the 3 closest pairs
            avgPoint1 = Vector3.zero;
            avgPoint2 = Vector3.zero;
            avgDistance = 0f;

            foreach (var pair in top3Pairs)
            {
                avgPoint1 += pair.point1;
                avgPoint2 += pair.point2;
                avgDistance += pair.distance;
            }

            avgPoint1 /= 3f;
            avgPoint2 /= 3f;
            avgDistance /= 3f;

        // Use slightly less than the average distance to ensure no overlap
        float width = avgDistance * 0.95f;
        const float height = 8f;
        const float thickness = 1f;

        Vector2 normal2D = new Vector2(planeData.normal[0], planeData.normal[1]).normalized;
        float angle = Mathf.Atan2(normal2D.y, normal2D.x) * Mathf.Rad2Deg;
        
        // Position plane at average position
        Vector3 centerPosition = (avgPoint1 + avgPoint2) / 2f;
        centerPosition.z = -60.4f;

        // Start the line growth animation
        StartCoroutine(GrowLine(centerPosition, angle, width, height, thickness));
        
        UnityEngine.Debug.Log($"Average position - Point1: {avgPoint1}, Point2: {avgPoint2}");
        UnityEngine.Debug.Log($"Average distance between points: {avgDistance}");
        UnityEngine.Debug.Log($"Using width: {width}");
        UnityEngine.Debug.Log($"Plane Transform - Position: {centerPosition}, " +
                            $"Rotation: {angle}, " +
                            $"Scale: {width}, {height}, {thickness}");
    }
    catch (Exception e)
    {
        UnityEngine.Debug.LogError($"Error updating separation line: {e.Message}");
    }
}

private IEnumerator GrowLine(Vector3 centerPosition, float angle, float targetWidth, float height, float thickness)
{
    float currentWidth = 0.1f;
    float growthRate = 0.5f;
    float redLineWidth = 0.5f;  // Fixed width for red line

    separationPlane.transform.position = centerPosition;
    separationPlane.transform.rotation = Quaternion.Euler(0, 0, angle);

    // Make the blue plane semi-transparent
    Renderer planeRenderer = separationPlane.GetComponent<Renderer>();
    Material planeMaterial = planeRenderer.material;
    Color planeColor = planeMaterial.color;
    planeMaterial.color = new Color(planeColor.r, planeColor.g, planeColor.b, 0.5f);
    
    // Transparency setup
    planeMaterial.SetFloat("_Mode", 3);
    planeMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
    planeMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
    planeMaterial.SetInt("_ZWrite", 0);
    planeMaterial.DisableKeyword("_ALPHATEST_ON");
    planeMaterial.EnableKeyword("_ALPHABLEND_ON");
    planeMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
    planeMaterial.renderQueue = 3000;

    // Get the red line child object
    Transform redLine = separationPlane.transform.GetChild(0);
    
    // Unparent the red line so it's not affected by parent's scale
    redLine.SetParent(null);
    
    // Set initial red line transform
    redLine.position = centerPosition;
    redLine.rotation = Quaternion.Euler(0, 0, angle);
    redLine.localScale = new Vector3(redLineWidth, height, thickness);

    while (currentWidth < targetWidth)
    {
        currentWidth += growthRate;
        separationPlane.transform.localScale = new Vector3(currentWidth, height, thickness);
        
        // Keep red line at center position
        redLine.position = centerPosition;
        redLine.rotation = Quaternion.Euler(0, 0, angle);
        
        UpdateDotHighlights();
        yield return new WaitForSeconds(0.01f);
    }

    // Set final scales
    separationPlane.transform.localScale = new Vector3(targetWidth, height, thickness);
    
    // Re-parent the red line
    redLine.SetParent(separationPlane.transform);
    redLine.localScale = new Vector3(0.5f, 1, 1);
}


    public string CallPythonSolver(int[] selectedFeatures)
    {
        try
        {
            string scriptPath = Path.Combine(Application.dataPath, "Scripts", "iris_svm_solver.py");
            if (!File.Exists(scriptPath))
            {
                UnityEngine.Debug.LogError($"Python script not found at: {scriptPath}");
                return null;
            }

            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"C:\Users\mbarb\AppData\Local\Programs\Python\Python312\python.exe";
            start.Arguments = $"\"{scriptPath}\" {string.Join(" ", selectedFeatures)}";
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            start.CreateNoWindow = true;
            
            start.EnvironmentVariables["PYTHONPATH"] = @"C:\Users\mbarb\AppData\Local\Programs\Python\Python312\Lib\site-packages";
            
            UnityEngine.Debug.Log($"Executing Python script with arguments: {start.Arguments}");
            
            using (Process process = Process.Start(start))
            {
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                
                if (!string.IsNullOrEmpty(error))
                {
                    UnityEngine.Debug.LogError($"Python script error: {error}");
                }
                
                UnityEngine.Debug.Log($"Python script output: {output}");
                return output;
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError($"Error calling Python solver: {e.Message}\n{e.StackTrace}");
            return null;
        }
    }

void Update()
{
    if (Input.GetKeyDown(KeyCode.T))
    {
        TestPythonSetup();
        CheckFiles();
    }

    // Add this to continuously update dot highlights
    if (separationPlane.activeInHierarchy)
    {
        UpdateDotHighlights();
    }
}

    public void TestPythonSetup()
    {
        UnityEngine.Debug.Log("Testing Python setup...");
        try
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "/Library/Frameworks/Python.framework/Versions/3.12/bin/python3";
            start.Arguments = "-c \"print('Python test successful!')\"";
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            start.CreateNoWindow = true;
            
            using (Process process = Process.Start(start))
            {
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                
                UnityEngine.Debug.Log($"Python test output: {output}");
                if (!string.IsNullOrEmpty(error))
                {
                    UnityEngine.Debug.LogError($"Python test error: {error}");
                }
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError($"Python test failed: {e.Message}\n{e.StackTrace}");
        }
    }

    private void UpdateDotHighlights()
{
    // Get the plane's world position and normal
    Vector3 planePosition = separationPlane.transform.position;
    Vector3 planeNormal = separationPlane.transform.up;
    float planeWidth = separationPlane.transform.localScale.x;
    float planeHeight = separationPlane.transform.localScale.y;

    // Create a plane for intersection testing
    Plane plane = new Plane(planeNormal, planePosition);

    foreach (GameObject dot in irisVisualizer.dataPoints)
    {
        // Calculate dot's distance from plane center
        Vector3 dotToPlane = dot.transform.position - planePosition;
        float distanceFromCenter = Vector3.ProjectOnPlane(dotToPlane, planeNormal).magnitude;

        // Check if dot is within plane bounds
        bool isWithinPlane = distanceFromCenter <= planeWidth / 2;

        Renderer dotRenderer = dot.GetComponent<Renderer>();
        Material dotMaterial = dotRenderer.material;
        Color currentColor = dotMaterial.color;

        if (isWithinPlane)
        {
            // Make dot brighter
            dotMaterial.color = new Color(
                currentColor.r + 0.3f,
                currentColor.g + 0.3f,
                currentColor.b + 0.3f,
                1f
            );
        }
        else
        {
            // Reset to original color
            // If dot is red
            if (currentColor.r > currentColor.g && currentColor.r > currentColor.b)
            {
                dotMaterial.color = Color.red;
            }
            // If dot is blue
            else if (currentColor.b > currentColor.r && currentColor.b > currentColor.g)
            {
                dotMaterial.color = Color.blue;
            }
            else if(currentColor.g > currentColor.r && currentColor.g > currentColor.b){
                dotMaterial.color = Color.green;
            }
        }
    }
}

    public void CheckFiles()
    {
        string scriptPath = Path.Combine(Application.dataPath, "Scripts", "iris_svm_solver.py");
        string csvPath = Path.Combine(Application.dataPath, "iris.csv");
        
        UnityEngine.Debug.Log($"Checking Python script at: {scriptPath}");
        UnityEngine.Debug.Log($"Python script exists: {File.Exists(scriptPath)}");
        
        UnityEngine.Debug.Log($"Checking CSV file at: {csvPath}");
        UnityEngine.Debug.Log($"CSV file exists: {File.Exists(csvPath)}");
    }
}