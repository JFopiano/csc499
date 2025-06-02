/*using UnityEngine;

public class SVMTestCase3Solver : MonoBehaviour
{
    //public PlaneDeformer planeDeformer; // Reference to PlaneDeformer
    //public PlaneBulger planeBulger;    // Reference to PlaneBulger
    public SeparatorManager separatorManager; // Reference to SeparatorManager

    public void CalculateSVM()
    {
        if (planeDeformer == null || separatorManager == null || planeBulger == null)
        {
            Debug.LogError("PlaneDeformer, SeparatorManager, or PlaneBulger is not assigned.");
            return;
        }

       /* // Compute the average position of blue dots (class +1)
        Vector3 blueAverage = Vector3.zero;
        int blueCount = planeDeformer.dots.Count; // Access blue dots from PlaneDeformer
        foreach (var blueDot in planeDeformer.dots)
        {
            blueAverage += blueDot.transform.position;
        }
        if (blueCount > 0) blueAverage /= blueCount;

        // Compute the average position of red dots (class -1)
        Vector3 redAverage = Vector3.zero;
        int redCount = planeDeformer.dotsR.Count; // Access red dots from PlaneDeformer
        foreach (var redDot in planeDeformer.dotsR)
        {
            redAverage += redDot.transform.position;
        }
        if (redCount > 0) redAverage /= redCount;

        // Compute decision point and normal
        Vector3 decisionPoint = (redAverage + blueAverage) / 2;
        Vector3 decisionNormal = (blueAverage - redAverage).normalized;

        Debug.Log($"Refined Decision Point: {decisionPoint}, Decision Normal: {decisionNormal}");

        // Automatically apply deformation at the decision point along the z-axis
        planeDeformer.ApplyAutomaticPressure(decisionPoint, 1.0f, 1.6f); // Adjust pressureAmount and falloff as needed

        // Position and orient the separator
        separatorManager.decisionPoint = decisionPoint;
        separatorManager.decisionNormal = decisionNormal;
        separatorManager.PlaceSeparator();

        Debug.Log("SVM calculation completed and applied with refined decision boundary.");

        // Hard code the position, scale, and rotation of the separator plane
        Vector3 hardcodedPosition = new Vector3(-125, 5, -29); // Set your desired position
        Quaternion hardcodedRotation = Quaternion.Euler(0f, -90f, 0f); // Set your desired rotation
        Vector3 hardcodedScale = new Vector3(0.541f, 14, 7); // Set your desired scale

        // Apply the hardcoded values to the separator plane
        if (separatorManager != null)
        {
            if (separatorManager.transform != null)
            {
                separatorManager.transform.position = hardcodedPosition;
                separatorManager.transform.localScale = hardcodedScale;
                separatorManager.transform.rotation = hardcodedRotation;
                Debug.Log($"SeparatorManager position set to {hardcodedPosition}, scale set to {hardcodedScale}, rotation set to {hardcodedRotation}");
            }
            else
            {
                Debug.LogError("SeparatorManager does not have a Transform component.");
            }
        }
        else
        {
            Debug.LogError("SeparatorManager is not assigned.");
        }

        Debug.Log("Separator plane position, scale, and rotation have been hardcoded.");


    }


}*/
