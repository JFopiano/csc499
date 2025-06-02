using UnityEngine;

public class SeparatorController : MonoBehaviour
{
    public GameObject separatorManager; // Drag the separator GameObject here in the Inspector


    public void ApplyHardcodedTransform()
    {
        if (separatorManager == null)
        {
            Debug.LogError("SeparatorManager is not assigned!");
            return;
        }

        // Hardcoded position, rotation, and scale
        Vector3 hardcodedPosition = new Vector3(-125, 1.42f, -29); // Desired position
        Quaternion hardcodedRotation = Quaternion.Euler(0f, -90f, 0f); // Desired rotation
        Vector3 hardcodedScale = new Vector3(0.541f, 14, 7); // Desired scale

        // Apply the hardcoded values
        Transform separatorTransform = separatorManager.transform;
        separatorTransform.position = hardcodedPosition;
        separatorTransform.localScale = hardcodedScale;
        separatorTransform.rotation = hardcodedRotation;

        Debug.Log($"SeparatorManager updated: Position = {hardcodedPosition}, Scale = {hardcodedScale}, Rotation = {hardcodedRotation}");
    }
}
