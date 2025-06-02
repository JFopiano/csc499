using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SolverButtons : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField]
    private Button uiButton;

    [SerializeField]
    private GameObject PlaneGen;


    [SerializeField]
    private GameObject decisionPlane;


     [SerializeField]
    private GameObject redPlane;

    // Add this field to store the copy
    private GameObject planeGenCopy;

    private IEnumerator ApplyPressureWithDelay(GameObject plane) {
        yield return new WaitForSeconds(0.25f);
        PlaneDeformer planeDeformerScript = plane.GetComponent<PlaneDeformer>();
        if (planeDeformerScript != null) {
            planeDeformerScript.ApplyPressure(new Vector3(-0.20f, -4.32f, 0.5f), 250f, 1.6f);
            GameObject decisionPlane3 = GameObject.Find("DecisionPlaneLevel3");
            decisionPlane3.transform.position = new Vector3(-9.285f, 1.5f, -15.07f);
            decisionPlane3.transform.rotation = Quaternion.Euler(0, 90, 0);
            if(!gameManager.level3Solved) {
                gameManager.level3Solved = true;
                gameManager.taskPopupManager.setTaskTextByState(TaskPopupManager.TaskPopupState.Level3_Complete);
                gameManager.arrowManager.stopLevel3ArrowLightingSequence();
                gameManager.arrowManager.startLevel4ArrowLightingSequence();
            }
        } else {
            Debug.LogError("PlaneDeformer component not found after delay");
        }
    }

    // Start is called before the first frame update
    void Start() {
        // Prevent it from creating multiple copies since this is attached to many buttons
        if(gameObject.name == "Level3_Solver_Button") {
            // Create a copy of PlaneGen at start
            planeGenCopy = Instantiate(PlaneGen, PlaneGen.transform.localPosition, PlaneGen.transform.rotation);
            planeGenCopy.SetActive(false);  // Hide the copy
        }

        // Get components
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uiButton = GetComponent<Button>();

        // Set up UI Button events as fallback
        if (uiButton != null) {
            uiButton.onClick.AddListener(OnUIButtonClicked);
            //Debug.LogError("UI Button events attached to " + gameObject.name);
        }

        
    }

    // This one activates on pressing the button
    void OnUIButtonClicked() {
        // Check the button name and perform actions based on it
        if (gameObject.name == "Level1_Solver_Button") {

            Debug.Log("Level 1 Solver button clicked");
            decisionPlane.transform.localPosition = new Vector3(-0.59f, -0.544f, -9.37f);
            decisionPlane.transform.rotation = Quaternion.Euler(-2.078f, 91.91f, 39.218f);
            decisionPlane.transform.localScale = new Vector3(1.3f, 2f, 0.35f);

            if(!gameManager.level1Solved) {
                gameManager.level1Solved = true;
                gameManager.taskPopupManager.setTaskTextByState(TaskPopupManager.TaskPopupState.Level1_Complete);
                gameManager.arrowManager.stopLevel1ArrowLightingSequence();
                gameManager.arrowManager.startLevel2ArrowLightingSequence();
            }

            //redPlane.transform.localPosition = new Vector3(0.125f, 2f, 0.35f);
            // Add logic for Level 1 Solver
        } else if (gameObject.name == "Level2_Solver_Button") {
            Debug.Log("Level 2 Solver button clicked");

            decisionPlane.transform.localPosition = new Vector3(24.976f, -0.544f, -9.36f);
            decisionPlane.transform.rotation = Quaternion.Euler(-1.825f, 90, 37.318f);
            decisionPlane.transform.localScale = new Vector3(1.261f, 1.5f, 0.35f);

            if(!gameManager.level2Solved) {
                gameManager.level2Solved = true;
                gameManager.taskPopupManager.setTaskTextByState(TaskPopupManager.TaskPopupState.Level2_Complete);
                gameManager.arrowManager.stopLevel2ArrowLightingSequence();
                gameManager.arrowManager.startLevel3ArrowLightingSequence();
            }

            //redPlane.transform.localPosition = new Vector3(0.125f, 2f, 0.35f);
            // Add logic for Level 2 Solver
        } else if (gameObject.name == "Level3_Solver_Button") {
            Debug.Log("Level 3 Solver button clicked");
            
            
            // Destroy the previous PlaneGen
            Destroy(PlaneGen);

            // Set the copy as the new PlaneGen
            PlaneGen = Instantiate(planeGenCopy);

            // This is the parent object that PlaneGen should be a child of
            GameObject svmRelatedGameObject = GameObject.Find("SVM Related");

            // Set the parent of PlaneGen
            PlaneGen.transform.SetParent(svmRelatedGameObject.transform);
            PlaneGen.name = "PlaneGen";
            PlaneGen.SetActive(true);

            // Start the coroutine to apply pressure after delay
            StartCoroutine(ApplyPressureWithDelay(PlaneGen));

        } else if (gameObject.name == "Level4_Solver_Button") {
            Debug.Log("Level 4 Solver button clicked");
            gameManager.svmManager.SolveLevel4SVM();
            if(!gameManager.level4Solved) {
                gameManager.level4Solved = true;
                gameManager.taskPopupManager.setTaskTextByState(TaskPopupManager.TaskPopupState.Level4_Complete);
                gameManager.arrowManager.stopLevel4ArrowLightingSequence();
            }
        } else {
            Debug.LogWarning("Unknown button clicked: " + gameObject.name);
        }
    }
}
