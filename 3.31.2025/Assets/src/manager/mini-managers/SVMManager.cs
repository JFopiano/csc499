using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq; // Used for LINQ queries on collections, e.g., .Where(), .ToList().

// Represents the position and thickness of a visualized support vector.
public class SupportVectorPosition {
    public float x; // X-coordinate in the 2D visualization space.
    public float y; // Y-coordinate in the 2D visualization space.
    public float thickness; // Thickness or size of the visualized point.

    // Constructor for SupportVectorPosition.
    public SupportVectorPosition(float x, float y, float thickness) {
        this.x = x;
        this.y = y;
        this.thickness = thickness;
    }
}

// Commented out class: PlaneGenerator2.
// This class appears to be an an unused or old attempt at procedurally generating a plane mesh.
// It includes logic for creating vertices and triangles for a plane.
/*public class PlaneGenerator2 : MonoBehaviour
{
    public Material planeMaterial; // Reference to the material
    
    [Header("Starting Position")]
    public float startingPosX;
    public float startingPosY;
    public float startingPosZ;

    void GeneratePlane2(Vector3 size, int resolution)
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

    void DestroyPlane2()
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
}*/

// SVMManager class is responsible for creating and managing SVM instances for different levels.
// It handles the setup of data points (support vectors) and the hyperplane visualization.
// For Level 4, it includes functionality to load Iris dataset, normalize data, and train a simple SVM.
public class SVMManager {
    // List to store all created SVMInstance objects.
    private List<SVMInstance> svmInstances = new List<SVMInstance>();
    // Reference to the current SVMInstance specifically for Level 4, to manage its unique lifecycle.
    private SVMInstance currentLevel4Instance = null;
    
    // Creates a new SVMInstance, initializes its hyperplane, adds it to the list, and returns it.
    public SVMInstance CreateSVMInstance(string objectName, float scaleX, float scaleY, float scaleZ) {
        SVMInstance newSVM = new SVMInstance(); 
        // Initialize the hyperplane GameObject for the new SVM instance.
        newSVM.CreateHyperplane(objectName, scaleX, scaleY, scaleZ);
        svmInstances.Add(newSVM); // Add the new instance to the managed list.
        return newSVM;
    }

    // Sets up the SVM visualization for Level 1 with predefined data points.
    public void createLevel1SVM() {
            // Define positions for positive class support vectors for Level 1.
            List<SupportVectorPosition> positiveVectors = new List<SupportVectorPosition> {
                new SupportVectorPosition(-1.9f, -1.2f, 20f),
                new SupportVectorPosition(-1.3f, -1.0f, 20f),
                new SupportVectorPosition(-0.8f, -0.9f, 20f),
                new SupportVectorPosition(-2.1f, -0.5f, 20f),
                new SupportVectorPosition(-1.6f, -0.4f, 20f),
                new SupportVectorPosition(-0.9f, 0.1f, 20f),
                new SupportVectorPosition(-1.8f, 0.8f, 20f),
                new SupportVectorPosition(-0.5f, -0.6f, 20f),
                new SupportVectorPosition(-1.5f, 0.2f, 20f),
                new SupportVectorPosition(-1.2f, 0.6f, 20f),
            };
            // Define positions for negative class support vectors for Level 1.
            List<SupportVectorPosition> negativeVectors = new List<SupportVectorPosition> { 
                new SupportVectorPosition(1.8f, -1.0f, 20f),
                new SupportVectorPosition(1.1f, -0.7f, 20f),
                new SupportVectorPosition(1.9f, 0.5f, 20f),
                new SupportVectorPosition(1.7f, -0.4f, 20f),
                new SupportVectorPosition(0.8f, 0.1f, 20f),
                new SupportVectorPosition(0.9f, 1.2f, 20f),
                new SupportVectorPosition(1.4f, 0.0f, 20f),
                new SupportVectorPosition(0.4f, 0.7f, 20f),
                new SupportVectorPosition(1.3f, 0.9f, 20f),
                new SupportVectorPosition(0.2f, 1.1f, 20f),
            };
            
            // Create an SVM instance for Level 1, using a GameObject named "Level1SVMInstance".
            SVMInstance svmInstance = CreateSVMInstance("Level1SVMInstance", 4.7f, 0.01f, 2.75f);
            // Add the defined support vectors to this instance for visualization.
            svmInstance.AddSupportVectors(positiveVectors, negativeVectors);
            // Set the initial rotation of the hyperplane for Level 1.
            svmInstance.setHyperplaneRotation(90);
    }

    // Sets up the SVM visualization for Level 2 with predefined data points, including an outlier.
    public void createLevel2SVM() {
            // Define positions for positive class support vectors for Level 2.
            List<SupportVectorPosition> positiveVectors = new List<SupportVectorPosition> {
                new SupportVectorPosition(-1.9f, -1.2f, 20f),
                new SupportVectorPosition(-1.3f, -1.0f, 20f),
                new SupportVectorPosition(-0.8f, -0.9f, 20f),
                new SupportVectorPosition(-2.1f, -0.5f, 20f),
                new SupportVectorPosition(-1.6f, -0.4f, 20f),
                new SupportVectorPosition(-0.9f, 0.1f, 20f),
                new SupportVectorPosition(-1.8f, 0.8f, 20f),
                new SupportVectorPosition(-0.5f, -0.6f, 20f),
                new SupportVectorPosition(-1.5f, 0.2f, 20f),
                // Adding one outlier on the wrong side of the hyperplane for the positive class.
                new SupportVectorPosition(0.5f, 0.3f, 20f),
            };
            // Define positions for negative class support vectors for Level 2.
            List<SupportVectorPosition> negativeVectors = new List<SupportVectorPosition> { 
                new SupportVectorPosition(1.8f, -1.0f, 20f),
                new SupportVectorPosition(1.1f, -0.7f, 20f),
                new SupportVectorPosition(1.9f, 0.5f, 20f),
                new SupportVectorPosition(1.7f, -0.4f, 20f),
                new SupportVectorPosition(0.8f, 0.1f, 20f),
                new SupportVectorPosition(0.9f, 1.2f, 20f),
                new SupportVectorPosition(1.4f, 0.0f, 20f),
                new SupportVectorPosition(0.4f, 0.7f, 20f),
                new SupportVectorPosition(1.3f, 0.9f, 20f),
                new SupportVectorPosition(0.2f, 1.1f, 20f),
            };
            
            // Create an SVM instance for Level 2, using a GameObject named "Level2SVMInstance".
            SVMInstance svmInstance = CreateSVMInstance("Level2SVMInstance", 4.7f, 0.01f, 2.75f);
            // Add the defined support vectors to this instance.
            svmInstance.AddSupportVectors(positiveVectors, negativeVectors);
            // Set the initial rotation of the hyperplane for Level 2.
            svmInstance.setHyperplaneRotation(90);
    }

    // Helper method to get a display-friendly string name for a FeatureName enum value.
    private string GetFeatureName(FeatureName featureName) {
        switch (featureName) {
            case FeatureName.SepalLength: return "Sepal Length";
            case FeatureName.SepalWidth: return "Sepal Width";
            case FeatureName.PetalLength: return "Petal Length";
            case FeatureName.PetalWidth: return "Petal Width";
            default: return "Unknown Feature";
        }
    }

    // Helper method to get the specific feature value from an IrisData object based on the FeatureName enum.
    // Assumes IrisData class has public fields like sepalLength, sepalWidth, etc.
    private float GetFeatureValue(IrisData iris, FeatureName featureName) {
        switch (featureName) {
            case FeatureName.SepalLength: return iris.sepalLength;
            case FeatureName.SepalWidth: return iris.sepalWidth;
            case FeatureName.PetalLength: return iris.petalLength;
            case FeatureName.PetalWidth: return iris.petalWidth;
            default: return 0f; // Return 0 if the feature name is unknown.
        }
    }

    // Helper method to get a display-friendly string name for a SpeciesType enum value.
    public string GetSpeciesName(SpeciesType speciesType) {
        switch (speciesType) {
            case SpeciesType.IrisSetosa: return "Iris-setosa";
            case SpeciesType.IrisVersicolor: return "Iris-versicolor";
            case SpeciesType.IrisVirginica: return "Iris-virginica";
            default: return "Unknown Species";
        }
    }

    // Enum defining the available features from the Iris dataset that can be used for visualization/classification.
    public enum FeatureName {
        SepalLength,
        SepalWidth,
        PetalLength,
        PetalWidth
    }

    // Enum defining the species types in the Iris dataset.
    public enum SpeciesType {
        IrisSetosa,
        IrisVersicolor,
        IrisVirginica
    }

    // Converts a string representation of a feature name to its corresponding FeatureName enum value.
    // Defaults to SepalLength if the string is not recognized.
    public FeatureName getFeatureNameFromString(string featureName) {
        switch (featureName) {
            case "Sepal Length": return FeatureName.SepalLength;
            case "Sepal Width": return FeatureName.SepalWidth;
            case "Petal Length": return FeatureName.PetalLength;
            case "Petal Width": return FeatureName.PetalWidth;
            default: return FeatureName.SepalLength; // Default fallback.
        }
    }

    // Converts a string representation of a species type to its corresponding SpeciesType enum value.
    // Defaults to IrisSetosa if the string is not recognized.
    public SpeciesType getSpeciesTypeFromString(string speciesType) {
        switch (speciesType) {
            case "Iris-setosa": return SpeciesType.IrisSetosa;
            case "Iris-versicolor": return SpeciesType.IrisVersicolor;
            case "Iris-virginica": return SpeciesType.IrisVirginica;
            default: return SpeciesType.IrisSetosa; // Default fallback.
        }
    }

    // Creates or recreates the SVM instance for Level 4 using data from a CSV file.
    // Allows selection of features for X and Y axes and two species for classification.
    public SVMInstance createLevel4SVM(TextAsset csvFile, FeatureName xFeature = FeatureName.SepalLength, FeatureName yFeature = FeatureName.SepalWidth, SpeciesType species1 = SpeciesType.IrisSetosa, SpeciesType species2 = SpeciesType.IrisVersicolor) {
        // Clean up any previous Level 4 SVM instance to prevent duplicates or old data.
        if (currentLevel4Instance != null) {
            currentLevel4Instance.ClearSupportVectors(); // Remove its visualized points.
            svmInstances.Remove(currentLevel4Instance); // Remove from the manager's list.
            // currentLevel4Instance.Destroy(); // Could also call Destroy if SVMInstance has a full cleanup method for its GameObject.
            currentLevel4Instance = null;
        }

        // Create an IrisDataLoader to load data from the provided CSV TextAsset.
        // Assumes IrisDataLoader class exists and is set up to parse the CSV format.
        IrisDataLoader irisDataLoader = new IrisDataLoader(csvFile);
        List<IrisData> irisData = irisDataLoader.LoadIrisData();
        Debug.Log($"Iris data loaded: {irisData.Count}");
        Debug.Log($"Visualizing: X={GetFeatureName(xFeature)}, Y={GetFeatureName(yFeature)}, Species1={GetSpeciesName(species1)}, Species2={GetSpeciesName(species2)}");
        
        // Filter the loaded Iris data to include only the two selected species for classification.
        List<IrisData> filteredData = irisData
            .Where(data => data.species == GetSpeciesName(species1) || data.species == GetSpeciesName(species2))
            .ToList();
        
        Debug.Log($"Filtered Iris data: {filteredData.Count} points of {GetSpeciesName(species1)} and {GetSpeciesName(species2)}");
        
        // Calculate min and max values for the selected X and Y features from the filtered data.
        // These will be used for normalizing the data for consistent visualization.
        Vector2 minValues = new Vector2(float.MaxValue, float.MaxValue);
        Vector2 maxValues = new Vector2(float.MinValue, float.MinValue);
        
        foreach (var iris in filteredData) {
            float xValue = GetFeatureValue(iris, xFeature);
            float yValue = GetFeatureValue(iris, yFeature);
            
            minValues.x = Mathf.Min(minValues.x, xValue);
            minValues.y = Mathf.Min(minValues.y, yValue);
            maxValues.x = Mathf.Max(maxValues.x, xValue);
            maxValues.y = Mathf.Max(maxValues.y, yValue);
        }
        
        // Define maximum dimensions for the visualization area (hyperplane space).
        float maxWidth = 2.0f;  // Max width from center to edge (total width 4.0f).
        float maxHeight = 1.2f; // Max height from center to edge (total height 2.4f).
        
        // Create lists to hold the positions of the visualized support vectors for positive and negative classes.
        List<SupportVectorPosition> positiveVectors = new List<SupportVectorPosition>();
        List<SupportVectorPosition> negativeVectors = new List<SupportVectorPosition>();
        
        // Convert the filtered Iris data points into SupportVectorPosition objects with normalized coordinates.
        foreach (var iris in filteredData) {
            float xValue = GetFeatureValue(iris, xFeature);
            float yValue = GetFeatureValue(iris, yFeature);
            
            // Normalize the feature values to fit within the defined maxWidth and maxHeight for visualization.
            // The coordinates will range from -maxWidth to +maxWidth and -maxHeight to +maxHeight.
            float normalizedX = NormalizeValue(xValue, minValues.x, maxValues.x) * (maxWidth * 2) - maxWidth;
            float normalizedY = NormalizeValue(yValue, minValues.y, maxValues.y) * (maxHeight * 2) - maxHeight;
            
            // Clamp values to ensure they stay strictly within the defined visualization bounds.
            normalizedX = Mathf.Clamp(normalizedX, -maxWidth, maxWidth);
            normalizedY = Mathf.Clamp(normalizedY, -maxHeight, maxHeight);
            
            // Assign the point to either the positive or negative class based on its species.
            if (iris.species == GetSpeciesName(species1)) {
                positiveVectors.Add(new SupportVectorPosition(normalizedX, normalizedY, 20f)); // Thickness is hardcoded to 20f.
            } else { // Assumes any data not matching species1 must be species2 due to pre-filtering.
                negativeVectors.Add(new SupportVectorPosition(normalizedX, normalizedY, 20f));
            }
        }
        
        // Create the SVM instance for Level 4, using a GameObject named "Level4SVMInstance".
        SVMInstance svmInstance = CreateSVMInstance("Level4SVMInstance", 4.7f, 0.01f, 2.75f);
        // Add the generated support vectors (from Iris data) to this instance.
        svmInstance.AddSupportVectors(positiveVectors, negativeVectors);
        // Set the initial hyperplane rotation for Level 4 (0 degrees, likely flat or aligned with an axis).
        svmInstance.setHyperplaneRotation(0);
        
        // Set a label on the SVM instance to indicate which features are currently being visualized.
        svmInstance.SetLabel($"{GetFeatureName(xFeature)} vs {GetFeatureName(yFeature)}");
        
        // Store this new instance as the current one for Level 4.
        currentLevel4Instance = svmInstance;

        // The following line is commented out, suggesting that solving (training and visualizing the plane)
        // is triggered by a separate action, likely a button press (see SolveLevel4SVM).
        // SolveLevel4SVM();
        
        return svmInstance; // Return the newly created or updated Level 4 SVM instance.
    }

    

    // Helper method to normalize a value from its original range [min, max] to a [0, 1] range.
    private float NormalizeValue(float value, float min, float max) {
        // If min and max are effectively the same, return 0.5 to avoid division by zero and place the point in the middle.
        if (Mathf.Approximately(min, max)) {
            return 0.5f; 
        }
        return (value - min) / (max - min);
    }

    /// Solves the SVM for the current Level 4 instance using the loaded and filtered Iris data.
    /// This method trains a simple SVM model using the selected features and species, 
    /// then updates the position and rotation of the 'DecisionPlaneLevel4' GameObject 
    /// to visualize the calculated decision boundary.
    public void SolveLevel4SVM() {
        // Check if the Level 4 SVM instance and its vector data are initialized.
        if (currentLevel4Instance == null || currentLevel4Instance.PositiveVectors == null || currentLevel4Instance.NegativeVectors == null) {
            Debug.LogWarning("Cannot solve Level 4 SVM: Instance or vectors not initialized.");
            return;
        }

        List<SupportVectorPosition> positiveVectors = currentLevel4Instance.PositiveVectors;
        List<SupportVectorPosition> negativeVectors = currentLevel4Instance.NegativeVectors;

        // Check if there are data points in both classes to train on.
        if (positiveVectors.Count == 0 || negativeVectors.Count == 0) {
            Debug.LogWarning("Cannot solve Level 4 SVM: One or both vector classes are empty.");
            return;
        }

        // Prepare data arrays for the SVM training algorithm.
        // X: Feature data (normalized x, y positions from SupportVectorPosition).
        // y: Class labels (+1 for species1/positive, -1 for species2/negative).
        int nPos = positiveVectors.Count;
        int nNeg = negativeVectors.Count;
        int n = nPos + nNeg; // Total number of data points.
        float[,] X = new float[n, 2]; // n samples, 2 features (x, y).
        float[] y = new float[n];     // n samples, 1 label each.
        
        // Populate X and y arrays with data from positive vectors.
        for (int i = 0; i < nPos; i++) {
            X[i, 0] = positiveVectors[i].x;
            X[i, 1] = positiveVectors[i].y;
            y[i] =  1f; // Positive class label.
        }
        // Populate X and y arrays with data from negative vectors.
        for (int i = 0; i < nNeg; i++) {
            X[nPos + i, 0] = negativeVectors[i].x;
            X[nPos + i, 1] = negativeVectors[i].y;
            y[nPos + i] = -1f; // Negative class label.
        }

        // Train the SVM model using the prepared data.
        float[] w; // Output: Weight vector (w_x, w_y) for the decision boundary w.x + b = 0.
        float b;   // Output: Bias term for the decision boundary.
        // Call the TrainSVM method with specified hyperparameters (C, learning rate, max iterations).
        TrainSVM(X, y, out w, out b, C: 1f, lr: 0.01f, maxIter: 1000);

        // Calculate the rotation angle and offset for the decision plane visualization based on the trained SVM parameters (w, b).
        // The angle (angleZ) represents the orientation of the plane in the XY visualization space.
        // The distance represents its shift from the origin along its normal vector.
        float angleZ = Mathf.Atan2(w[1], w[0]) * Mathf.Rad2Deg; // Angle of the normal vector w, converted to degrees.
        // Distance of the plane from the origin: -b / ||w||
        float distance = -b / Mathf.Sqrt(w[0] * w[0] + w[1] * w[1]); 
        // Calculate the 2D offset vector for the plane's position (projection of origin onto the plane, then negated).
        Vector2 offset = new Vector2(w[0], w[1]).normalized * distance; 

        // Find the GameObject in the scene that represents the decision plane for Level 4.
        GameObject decisionPlane = GameObject.Find("DecisionPlaneLevel4");
        if (decisionPlane == null) {
            Debug.LogError("Could not find DecisionPlaneLevel4 GameObject.");
            return;
        }

        // Hardcode the transform of the decision plane. 
        // This section seems to override any calculation based on the SVM result for the plane's position, 
        // effectively setting it to a fixed state. This might be for a specific visual target or a bug.
        // The SVM training and parameter calculation (angleZ, offset) might be intended for a different visualization approach or are being overridden here.
        decisionPlane.transform.localPosition = new Vector3(9.01f, -0.392f, -38.77f);
        decisionPlane.transform.localRotation = Quaternion.Euler(0f, 90f, 0f); // Fixed rotation.
        decisionPlane.transform.localScale = new Vector3(0.125f, 2.5f, 0.35f); // Fixed scale.

        // The lines below are commented out, but they would have applied the SVM-calculated transform.
        // This suggests the hardcoded transform above is intentional for the current setup.
        /*
        // Apply the calculated rotation and offset to the decision plane's transform.
        // An additional hardcoded offset (3.75f) is added to the x-position.
        Vector3 pos = decisionPlane.transform.localPosition; // This would start from its current position, not origin.
        pos.x += offset.x + (3.75f); // Apply calculated offset and hardcoded adjustment
        pos.y += offset.y;           // Apply calculated offset
        decisionPlane.transform.localPosition = pos;
        decisionPlane.transform.rotation = Quaternion.Euler(0f, 0f, angleZ); // Apply calculated rotation (around Z for 2D view)
        */
    }

    /// Trains a linear Support Vector Machine (SVM) model using a basic gradient descent algorithm.
    /// It aims to minimize the hinge loss with an L2 regularization term.
    /// - X: Input feature data (n_samples x 2 features for this implementation).
    /// - y: Target labels (+1 or -1, n_samples).
    /// - w: Output weight vector (array of 2 floats for w_x, w_y).
    /// - b: Output bias term.
    /// - C: Regularization parameter. Controls the trade-off between maximizing the margin and minimizing classification errors.
    /// - lr: Learning rate for the gradient descent updates.
    /// - maxIter: Maximum number of iterations for the gradient descent optimization.
    private void TrainSVM(float[,] X, float[] y, out float[] w, out float b, float C, float lr, int maxIter) {
        int n = y.Length; // Number of samples.
        w = new float[2]; // Initialize weights to zero (w_x = 0, w_y = 0).
        b = 0f;           // Initialize bias to zero.

        // Perform gradient descent for a fixed number of iterations.
        for (int iter = 0; iter < maxIter; iter++) {
            // Initialize gradients for weights. The L2 regularization term (lambda*w, here implicitly lambda=1) derivative is w itself.
            float[] gradW = new float[2] { w[0], w[1] }; // Gradient component from regularization: d(0.5*||w||^2)/dw_i = w_i.
            float gradB = 0f;                            // Gradient for bias (no regularization on bias typically).

            // Iterate through all data points to calculate the hinge loss and its gradient.
            for (int i = 0; i < n; i++) {
                // Calculate the current prediction score: w . x_i + b.
                float wx = w[0] * X[i, 0] + w[1] * X[i, 1] + b; 
                
                // Check the condition for hinge loss: y_i * (w . x_i + b) < 1.
                // This means the point is either misclassified or correctly classified but within the margin.
                if (y[i] * wx < 1f) {
                    // If the condition is met, add the gradient component from the hinge loss: -C * y_i * x_i for w, and -C * y_i for b.
                    gradW[0] -= C * y[i] * X[i, 0];
                    gradW[1] -= C * y[i] * X[i, 1];
                    gradB   -= C * y[i];
                }
            }

            // Update weights and bias using the calculated gradients and the learning rate.
            // w = w - lr * gradW
            // b = b - lr * gradB
            w[0] -= lr * gradW[0];
            w[1] -= lr * gradW[1];
            b     -= lr * gradB;
        }
        // After maxIter, w and b hold the parameters of the trained linear SVM.
    }
}

// Represents a single SVM instance in the game, including its hyperplane and visualized support vectors.
public class SVMInstance {
    // Predefined colors for positive (green) and negative (red) class support vectors.
    private Color positiveColor = new Color(0.2f, 0.8f, 0.2f); 
    private Color negativeColor = new Color(1f, 0.2f, 0.2f); 
    // GameObject representing the hyperplane (decision boundary) for this SVM instance.
    private GameObject hyperplane;
    // List to store the GameObjects of all visualized support vectors for this instance.
    private List<GameObject> supportVectors = new List<GameObject>();

    // Properties to store the raw SupportVectorPosition data for positive and negative classes.
    // These are used for logic like re-training or accessing the original data points.
    public List<SupportVectorPosition> PositiveVectors { get; private set; }
    public List<SupportVectorPosition> NegativeVectors { get; private set; }

    // Getter for the hyperplane GameObject, allowing external access if needed.
    public GameObject GetHyperplaneGameObject() {
        return hyperplane;
    }

    // Sets the rotation of the hyperplane GameObject.
    // The rotation is applied primarily around the Y-axis for a 2D-like visualization in 3D space.
    public void setHyperplaneRotation(float rotationY) {
        //Debug.Log("Setting Hyperplane rotation to: " + rotationY);
        // Sets rotation to (90, rotationY, 0), making it vertical and rotated by rotationY around the world Y-axis.
        hyperplane.transform.rotation = Quaternion.Euler(90, rotationY, 0); 
        //Debug.Log("New Hyperplane rotation: " + hyperplane.transform.rotation);
    }
    
    
    // Adds lists of positive and negative support vector positions to this SVM instance and creates their visual representations.
    public void AddSupportVectors(List<SupportVectorPosition> positiveVectors, List<SupportVectorPosition> negativeVectors) {
        // Temporary dictionary to hold positions categorized by color (though only two colors are used here).
        Dictionary<Color, List<SupportVectorPosition>> vectorPositions = new Dictionary<Color, List<SupportVectorPosition>>(); 
        
        // Store the provided vector position data in the instance properties.
        this.PositiveVectors = positiveVectors;
        this.NegativeVectors = negativeVectors;

        // Populate the dictionary for visualization.
        vectorPositions[positiveColor] = positiveVectors;
        vectorPositions[negativeColor] = negativeVectors;
        
        // Create the visual GameObjects for these support vectors.
        CreateSupportVectors(vectorPositions);
    }
    
    // Creates or initializes the hyperplane GameObject for this SVM instance.
    public void CreateHyperplane(string objectName, float scaleX, float scaleY, float scaleZ) {
        // Find an existing GameObject in the scene by the provided objectName.
        // This assumes the hyperplane GameObject is pre-placed in the scene.
        hyperplane = GameObject.Find(objectName);
        
        if (hyperplane == null) {
            Debug.LogError($"Could not find HyperPlane object named '{objectName}' in the scene");
            return;
        }

        //Debug.Log("Hyperplane found");

        // Set the local scale of the hyperplane GameObject.
        hyperplane.transform.localScale = new Vector3(scaleX, scaleY, scaleZ); 
        // Set the initial rotation to be vertical (90 degrees around X-axis).
        hyperplane.transform.rotation = Quaternion.Euler(90, 0, 0); 
        //Debug.Log("Hyperplane position: " + hyperplane.transform.position);
        //Debug.Log("Hyperplane rotation: " + hyperplane.transform.rotation);
        //Debug.Log("Hyperplane scale: " + hyperplane.transform.localScale);
        
        // Set material properties for transparency and color.
        Renderer renderer = hyperplane.GetComponent<Renderer>();
        if (renderer != null) {
            // Create a new Standard material instance for the hyperplane.
            renderer.material = new Material(Shader.Find("Standard"));
            // Set its color to semi-transparent green (alpha = 0 means fully transparent, but might be overridden by shader mode).
            renderer.material.color = new Color(0.2f, 0.8f, 0.2f, 0f); 
            
            // Configure the material for transparency.
            // These settings are specific to Unity's Standard Shader rendering modes.
            renderer.material.SetFloat("_Mode", 3); // Set to Transparent mode.
            renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            renderer.material.SetInt("_ZWrite", 0); // Disable writing to the depth buffer.
            renderer.material.DisableKeyword("_ALPHATEST_ON");
            renderer.material.EnableKeyword("_ALPHABLEND_ON");
            renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            renderer.material.renderQueue = 3000; // Set render queue for transparent objects.
        } else {
            Debug.LogError($"Hyperplane object '{objectName}' does not have a Renderer component.");
        }

        //Debug.Log("Hyperplane material set");
        
        // Simple animation for the hyperplane: scale it in using LeanTween.
        LeanTween.scale(hyperplane, new Vector3(scaleX, scaleY, scaleZ), 0.8f)
            .setEaseOutQuad() // Apply an ease-out quad easing function.
            .setFrom(new Vector3(0.1f, 0.01f, 0.1f)); // Start scaling from a very small size.
        
        //Debug.Log("Hyperplane animation completed");
    }
    
    // Creates the visual GameObjects (spheres) for all support vectors.
    private void CreateSupportVectors(Dictionary<Color, List<SupportVectorPosition>> vectorPositions) {
        // Clear any previously created support vector GameObjects for this instance.
        foreach (GameObject sv in supportVectors) {
            if (sv != null) {
                GameObject.Destroy(sv);
            }
        }
        supportVectors.Clear(); // Clear the list itself.
        
        // Get the hyperplane's current world position.
        // Support vector positions are defined relative to this, but then parented, so local positions are used.
        Vector3 hyperplanePos = hyperplane.transform.position;
        // Quaternion planeRotation = hyperplane.transform.rotation; // Not directly used for sphere orientation here.
        
        // Iterate through each color (class) and its list of vector positions.
        foreach(var kvp in vectorPositions) {
            Color color = kvp.Key;
            List<SupportVectorPosition> positions = kvp.Value;
            
            foreach(var posData in positions) {
                // Calculate the local position for the support vector sphere relative to the hyperplane's pivot.
                // The Z coordinate is 0, meaning they are on the plane defined by the hyperplane's local X and Y axes.
                Vector3 localSpherePosition = new Vector3(posData.x, posData.y, 0);
                // Create the sphere GameObject at this local position.
                GameObject sphere = CreateSupportVector(localSpherePosition, 0.05f, posData.thickness, color);
                // Parent the sphere to the hyperplane GameObject so it moves and rotates with the plane.
                // `true` for worldPositionStays, but since we used local position, this doesn't matter much.
                // Better to set localPosition directly after parenting if parenting with worldPositionStays = false.
                sphere.transform.SetParent(hyperplane.transform, false); // Parent and then set local properties.
                sphere.transform.localPosition = localSpherePosition;
                sphere.transform.localRotation = Quaternion.identity; // Align sphere's rotation with parent initially.
            }
        }
        
        //Debug.Log("Created " + supportVectors.Count + " support vectors");
    }
    
    // Creates a single support vector GameObject (a sphere) with specified properties.
    private GameObject CreateSupportVector(Vector3 localPosition, float width, float heightOrThickness, Color color) {
        // Create a sphere primitive GameObject.
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        // Sphere's local scale. HeightOrThickness is used for Y scale, width for X, and a fixed Z scale.
        sphere.transform.localScale = new Vector3(width, heightOrThickness, 0.08f);
        // Note: Position and rotation are set after parenting in CreateSupportVectors for clarity.
        
        // Add a SphereCollider for interaction (e.g., triggering glow).
        SphereCollider collider = sphere.GetComponent<SphereCollider>();
        if (collider == null) {
            collider = sphere.AddComponent<SphereCollider>();
        }
        collider.isTrigger = true; // Set as trigger to detect overlap without physical collision.

        // Add the SupportVectorInteraction script to handle glow effects on trigger enter/exit.
        SupportVectorInteraction interaction = sphere.AddComponent<SupportVectorInteraction>();
        
        // Set material properties for the sphere, including emission for glow.
        Renderer renderer = sphere.GetComponent<Renderer>();
        if (renderer != null) {
            Material material = new Material(Shader.Find("Standard")); // Create a new Standard material.
            material.EnableKeyword("_EMISSION"); // Enable emission property.
            material.color = color; // Set the base albedo color.
            material.SetColor("_EmissionColor", Color.black); // Start with no emission (black).
            renderer.material = material;
            
            // Pass the material and base color to the interaction script for it to control glow.
            interaction.Initialize(material, color);
        } else {
            Debug.LogError("Created support vector sphere is missing a Renderer.");
        }
        
        // Add a small pop-in animation using LeanTween when the sphere is created.
        sphere.transform.localScale = Vector3.zero; // Start from zero scale.
        LeanTween.scale(sphere, new Vector3(width, heightOrThickness,  0.08f), 0.5f)
            .setEaseOutBack() // Apply an ease-out-back easing function for a bouncy effect.
            .setDelay(UnityEngine.Random.Range(0.1f, 0.5f)); // Add a slight random delay for staggered appearance.
        
        // Add the newly created sphere to the list of support vectors for this instance.
        supportVectors.Add(sphere);
        return sphere;
    }
    
    // Destroys the hyperplane and all associated support vector GameObjects for this SVM instance.
    public void Destroy() {
        // Clean up hyperplane GameObject.
        if (hyperplane != null) {
            GameObject.Destroy(hyperplane);
            hyperplane = null; // Nullify reference.
        }
        
        // Clean up all support vector GameObjects.
        foreach (GameObject sv in supportVectors) {
            if (sv != null) {
                GameObject.Destroy(sv);
            }
        }
        supportVectors.Clear(); // Clear the list.
        // Note: PositiveVectors and NegativeVectors (the data) are not cleared here, only visuals.
    }

    // Sets a label for the SVM instance. Currently, this method is a stub and has no implementation.
    // It might be intended for displaying information like feature names on the visualization.
    public void SetLabel(string label) {
        // Implementation of SetLabel method would go here (e.g., update a Text UI element associated with this SVM).
        Debug.Log($"SVMInstance Label set to: {label}"); // Placeholder log.
    }

    // Clears (destroys) all visual support vector GameObjects for this instance.
    // Does not destroy the hyperplane itself or the underlying SupportVectorPosition data.
    public void ClearSupportVectors() {
        foreach (GameObject sv in supportVectors) {
            if (sv != null) {
                GameObject.Destroy(sv);
            }
        }
        supportVectors.Clear();
    }
}

// MonoBehaviour class to handle interaction with individual support vector GameObjects (spheres).
// Specifically, it makes the sphere glow when a collider (e.g., player's controller) enters its trigger.
public class SupportVectorInteraction : MonoBehaviour {
    private Material material; // Reference to the sphere's material to change emission.
    private Color baseColor;   // The original albedo color of the sphere.
    private bool isGlowing = false; // Flag to track if the sphere is currently glowing.
    private float glowIntensity = 2.0f; // Multiplier for the emission color to control glow strength.
    
    // Initializes the script with the sphere's material and base color.
    // Called by SVMInstance when the support vector sphere is created.
    public void Initialize(Material mat, Color color) {
        material = mat;
        baseColor = color;
    }
    
    // Called by Unity when another collider enters this GameObject's trigger collider.
    void OnTriggerEnter(Collider other) {
        // Check if it's not already glowing to avoid redundant calls.
        // No specific check for 'other' tag, so any collider entering will trigger glow.
        if (!isGlowing) {
            StartGlow();
        }
    }
    
    // Called by Unity when another collider exits this GameObject's trigger collider.
    void OnTriggerExit(Collider other) {
        // Check if it is currently glowing.
        if (isGlowing) {
            StopGlow();
        }
    }
    
    // Activates the glow effect by setting the material's emission color.
    private void StartGlow() {
        isGlowing = true;
        if (material != null) { // Null check for material safety
            material.SetColor("_EmissionColor", baseColor * glowIntensity);
        }
    }
    
    // Deactivates the glow effect by setting the material's emission color back to black.
    private void StopGlow() {
        isGlowing = false;
        if (material != null) { // Null check for material safety
            material.SetColor("_EmissionColor", Color.black);
        }
    }
}