using UnityEngine;

public class SeparatorManager : MonoBehaviour
{
    public GameObject separatorCube; // Reference to the separator cube
    public Vector3 decisionPoint;   // A point on the decision boundary
    public Vector3 decisionNormal;  // Normal vector of the decision boundary
    public float separatorLength = 10.0f; // Length of the separator
    public float separatorThickness = 0.1f; // Thickness of the separator

    public void PlaceSeparator()
    {
        if (separatorCube == null)
        {
            Debug.LogError("Separator cube is not assigned.");
            return;
        }

        // Position the separator at the decision point
        separatorCube.transform.position = decisionPoint;

        // Orient the separator based on the decision normal
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, decisionNormal);
        separatorCube.transform.rotation = rotation;

        // Scale the separator to match the calculated boundary
        separatorCube.transform.localScale = new Vector3(separatorThickness, separatorLength, 1.0f);

        Debug.Log("Separator placed successfully.");
    }
}
