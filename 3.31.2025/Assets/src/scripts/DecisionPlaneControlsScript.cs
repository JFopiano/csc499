using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;



public class DecisionPlaneControlsScript : MonoBehaviour {
    
    private XRGrabInteractable grabInteractable;
    private bool isBeingHeld = false;

    public Transform decisionPlaneTransform;
     public Transform decisionRedPlaneTransform;

    // Start is called before the first frame update
    void Start() {
        grabInteractable = GetComponent<XRGrabInteractable>();
        
        if (grabInteractable != null) {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
        } else {
            Debug.LogError("XR Grab Interactable component not found on this GameObject");
        }
    }

    private void OnGrab(SelectEnterEventArgs args) {
        Debug.Log("Grabbed");
        isBeingHeld = true;
    }
    
    private void OnRelease(SelectExitEventArgs args) {
        Debug.Log("Released");
        isBeingHeld = false;    
    }

    // Check if the object is currently being held
    public bool IsBeingHeld() {
        return isBeingHeld;
    }

    void changeScale(float scale, bool isIncreasing) {
        Debug.Log("Attempting to change scale, current scale: " + decisionPlaneTransform.localScale);
        Vector3 newScale = decisionPlaneTransform.localScale;
        
        // Store the original local scale of the child before changing parent
        Vector3 originalChildLocalScale = decisionRedPlaneTransform.localScale;
        
        // Calculate the world scale of the child before parent changes
        Vector3 childWorldScale = new Vector3(
            decisionRedPlaneTransform.lossyScale.x,
            decisionRedPlaneTransform.lossyScale.y,
            decisionRedPlaneTransform.lossyScale.z
        );
        
        // Update parent's scale
        newScale.x += scale * (isIncreasing ? 0.05f : -0.05f);
        decisionPlaneTransform.localScale = newScale;
        
        // Calculate and apply the corrected local scale to maintain the child's world scale
        Vector3 newChildLocalScale = new Vector3(
            childWorldScale.x / decisionRedPlaneTransform.parent.lossyScale.x,
            childWorldScale.y / decisionRedPlaneTransform.parent.lossyScale.y,
            childWorldScale.z / decisionRedPlaneTransform.parent.lossyScale.z
        );
        
        decisionRedPlaneTransform.localScale = newChildLocalScale;
        
        Debug.Log("New parent scale: " + newScale);
        Debug.Log($"Child scale maintained at world scale");
    }

    // Update is called once per frame
    void Update() {
        if (isBeingHeld) {
            // Cache button states once per frame
            bool secondaryPressed = CheckSecondaryButtonPressed();
            bool primaryPressed = CheckPrimaryButtonPressed();
            
            // Update target scale based on button presses
            if (secondaryPressed) {
                changeScale(0.5f, true);
            }
            
            // Check for primary button press to make thinner
            if (primaryPressed) {
                changeScale(0.5f, false);
            }
        }
    }
    
    // Check if the primary button is pressed on the right controller
    private bool CheckPrimaryButtonPressed() {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, devices);
        
        foreach (var device in devices) {
            bool primaryButtonPressed = false;
            if (device.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonPressed) && primaryButtonPressed) {
                return true;
            }
        }
        return false;
    }
    
    // Check if the secondary button is pressed on the right controller
    private bool CheckSecondaryButtonPressed() {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, devices);
        
        foreach (var device in devices) {
            bool secondaryButtonPressed = false;
            if (device.TryGetFeatureValue(CommonUsages.secondaryButton, out secondaryButtonPressed) && secondaryButtonPressed) {
                return true;
            }
        }
        return false;
    }

    
}
