using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

public class CreateSVMButtonScript : MonoBehaviour {

    private GameManager gameManager;
    private Button uiButton;
    private Image buttonImage;
    private Color originalColor;

    private string XAxisFeatureDropDownPickerText;
    private string YAxisFeatureDropdownPickerText;
    private string SpeciesIDropdownPickerText;
    private string SpeciesIIDropdownPickerText;
    
    // Start is called before the first frame update
    void Start() {
        // Get components
        //simpleInteractable = GetComponent<XRSimpleInteractable>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uiButton = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
        
        // Save original color
        if (buttonImage != null) {
            originalColor = buttonImage.color;
        }
        
        // Set up UI Button events as fallback
        if (uiButton != null) {
            uiButton.onClick.AddListener(OnUIButtonClicked);
             //Debug.LogError("UI Button events attached to " + gameObject.name);

        }
        //OnUIButtonClicked(); // <-- for testing, comment out when not testing
    }


    //this one activates on pressing the button
    void OnUIButtonClicked() {
        // Find the label text's of the dropdown pickers
            XAxisFeatureDropDownPickerText = GameObject.Find("XAxisFeature DropdownPicker").transform.Find("Label").GetComponent<Text>().text;
            YAxisFeatureDropdownPickerText = GameObject.Find("YAxisFeature DropdownPicker").transform.Find("Label").GetComponent<Text>().text;
            SpeciesIDropdownPickerText = GameObject.Find("SpeciesI DropdownPicker").transform.Find("Label").GetComponent<Text>().text;
            SpeciesIIDropdownPickerText = GameObject.Find("SpeciesII DropdownPicker").transform.Find("Label").GetComponent<Text>().text;

            SVMManager.FeatureName xFeature = gameManager.svmManager.getFeatureNameFromString(XAxisFeatureDropDownPickerText);
            SVMManager.FeatureName yFeature = gameManager.svmManager.getFeatureNameFromString(YAxisFeatureDropdownPickerText);
            SVMManager.SpeciesType speciesI = gameManager.svmManager.getSpeciesTypeFromString(SpeciesIDropdownPickerText);
            SVMManager.SpeciesType speciesII = gameManager.svmManager.getSpeciesTypeFromString(SpeciesIIDropdownPickerText);
            // Find the GameObject representing the decision plane.
            GameObject decisionPlane = GameObject.Find("DecisionPlaneLevel4");
            if (decisionPlane == null) {
                Debug.LogError("Could not find DecisionPlaneLevel4 GameObject.");
                return;
            }

            // Hardcode the transform based on the provided image
            decisionPlane.transform.localPosition = new Vector3(9.01f, -0.392f, -38.77f);
            decisionPlane.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
            decisionPlane.transform.localScale = new Vector3(0.125f, 2.5f, 0.35f);
            gameManager.svmManager.createLevel4SVM(gameManager.level4SVMCSVFile, xFeature, yFeature, speciesI, speciesII);
    }
    private void ChangeColor(Color color) {
        // Try changing UI Image color first
        if (buttonImage != null) {
            buttonImage.color = color;
            return;
        }
        
        // Fallback to Renderer if not a UI element
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null && renderer.material != null) {
            renderer.material.color = color;
        }
    }

    private void RestoreOriginalColor() {
        // Restore UI Image color
        if (buttonImage != null) {
            buttonImage.color = originalColor;
            return;
        }
        
        // Fallback to Renderer if not a UI element
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null && renderer.material != null) {
            renderer.material.color = originalColor;
        }
    }

    // Important: Unsubscribe from events when the object is destroyed
    private void OnDestroy() {
        if (uiButton != null) {
            uiButton.onClick.RemoveListener(OnUIButtonClicked);
        }
    }
}
