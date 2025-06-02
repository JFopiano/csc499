using UnityEngine;
using System.Collections.Generic;
using System.Collections; // Required for IEnumerator to run coroutines.

// Manages sequences of animated guide arrows for different game levels.
// This class is not a MonoBehaviour and relies on an external MonoBehaviour (coroutineRunner)
// to start and stop coroutines for the arrow lighting animations.
public class ArrowManager {

    // Dictionaries to keep track of running lighting sequences and their corresponding coroutines.
    // sequenceId (string) is used as a key (e.g., "level1", "level2").
    private Dictionary<string, List<GameObject>> runningArrowLightingSequences;
    private Dictionary<string, Coroutine> runningCoroutines;
    
    // Parent GameObjects for arrow sets for each level.
    // These are passed in the constructor and are expected to hold the actual arrow GameObjects as children.
    private GameObject level1ArrowsGameObjectParent;
    private List<GameObject> level1Arrows; // List of actual arrow GameObjects for Level 1.
    private GameObject level2ArrowsGameObjectParent;
    private List<GameObject> level2Arrows; // List of actual arrow GameObjects for Level 2.
    private GameObject level3ArrowsGameObjectParent;
    private List<GameObject> level3Arrows; // List of actual arrow GameObjects for Level 3.
    private GameObject level4ArrowsGameObjectParent;
    private List<GameObject> level4Arrows; // List of actual arrow GameObjects for Level 4.
    
    // private bool isLightingSequence = false; // This field seems unused and can be removed.
    
    // Time delay between each arrow in a sequence lighting up and then dimming.
    private float lightDelay = 0.25f; 
    
    // private Coroutine lightingCoroutine; // This seems to be a remnant or for a single sequence, replaced by `runningCoroutines` dictionary.
    
    // Reference to a MonoBehaviour instance (e.g., GameManager) that can run coroutines.
    private MonoBehaviour coroutineRunner;
    
    // Default and highlight colors for the arrow materials.
    private Color defaultColor = Color.white;
    private Color highlightColor = Color.green;

    // Constructor for ArrowManager.
    // Initializes collections and populates lists of arrow GameObjects from their parent GameObjects.
    public ArrowManager(MonoBehaviour runner, GameObject level1ArrowsGameObjectParent, GameObject level2ArrowsGameObjectParent, GameObject level3ArrowsGameObjectParent, GameObject level4ArrowsGameObjectParent) {
        this.coroutineRunner = runner; // Store the MonoBehaviour instance for starting coroutines.
        this.runningArrowLightingSequences = new Dictionary<string, List<GameObject>>();
        this.runningCoroutines = new Dictionary<string, Coroutine>();
        
        // Store references to parent GameObjects for arrows of each level.
        this.level1ArrowsGameObjectParent = level1ArrowsGameObjectParent;
        this.level2ArrowsGameObjectParent = level2ArrowsGameObjectParent;
        this.level3ArrowsGameObjectParent = level3ArrowsGameObjectParent;
        this.level4ArrowsGameObjectParent = level4ArrowsGameObjectParent;
        
        // Initialize lists to hold the individual arrow GameObjects.
        this.level1Arrows = new List<GameObject>();
        this.level2Arrows = new List<GameObject>();
        this.level3Arrows = new List<GameObject>();
        this.level4Arrows = new List<GameObject>();

        // Check if all parent GameObjects for arrows are assigned.
        if (level1ArrowsGameObjectParent != null && level2ArrowsGameObjectParent != null && level3ArrowsGameObjectParent != null && level4ArrowsGameObjectParent != null) {
            // Populate the list of Level 1 arrows by iterating through children of its parent GameObject.
            foreach (Transform childTransform in level1ArrowsGameObjectParent.transform) {
                level1Arrows.Add(childTransform.gameObject);
            }
            // Populate for Level 2 arrows.
            foreach (Transform childTransform in level2ArrowsGameObjectParent.transform) {
                level2Arrows.Add(childTransform.gameObject);
            }
            // Populate for Level 3 arrows.
            foreach (Transform childTransform in level3ArrowsGameObjectParent.transform) {
                level3Arrows.Add(childTransform.gameObject);
            }
            // Populate for Level 4 arrows.
            foreach (Transform childTransform in level4ArrowsGameObjectParent.transform) {
                level4Arrows.Add(childTransform.gameObject);
            }
        } else {
            // Log an error if any of the arrow parent GameObjects are missing, as this will break functionality.
            Debug.LogError("One or more arrow parent GameObjects (Level1Arrows, Level2Arrows, etc.) are not assigned in ArrowManager constructor.");
        }
    }

    // Starts the arrow lighting sequence for Level 1.
    public void startLevel1ArrowLightingSequence() {
        if (level1ArrowsGameObjectParent == null) return; // Guard clause if parent is null.
        this.level1ArrowsGameObjectParent.SetActive(true); // Make the parent (and thus all Level 1 arrows) visible.
        Debug.Log("Starting level 1 arrow lighting sequence");
        runLightingSequenceOn(level1Arrows, "level1"); // Start the actual lighting animation.
    }

    // Stops the arrow lighting sequence for Level 1.
    public void stopLevel1ArrowLightingSequence() {
        if (level1ArrowsGameObjectParent == null) return;
        stopLightingSequence("level1"); // Stop the animation.
        this.level1ArrowsGameObjectParent.SetActive(false); // Hide the parent (and all Level 1 arrows).
    }

    // Starts the arrow lighting sequence for Level 2.
    public void startLevel2ArrowLightingSequence() {
        if (level2ArrowsGameObjectParent == null) return;
        this.level2ArrowsGameObjectParent.SetActive(true);
        Debug.Log("Starting level 2 arrow lighting sequence");
        runLightingSequenceOn(level2Arrows, "level2");
    }   

    // Stops the arrow lighting sequence for Level 2.
    public void stopLevel2ArrowLightingSequence() {
        if (level2ArrowsGameObjectParent == null) return;
        stopLightingSequence("level2");
        this.level2ArrowsGameObjectParent.SetActive(false);
    }

    // Starts the arrow lighting sequence for Level 3.
    public void startLevel3ArrowLightingSequence() {
        if (level3ArrowsGameObjectParent == null) return;
        this.level3ArrowsGameObjectParent.SetActive(true);
        Debug.Log("Starting level 3 arrow lighting sequence");
        runLightingSequenceOn(level3Arrows, "level3");
    }   

    // Stops the arrow lighting sequence for Level 3.
    public void stopLevel3ArrowLightingSequence() {
        if (level3ArrowsGameObjectParent == null) return;
        stopLightingSequence("level3");
        this.level3ArrowsGameObjectParent.SetActive(false);
    }

    // Starts the arrow lighting sequence for Level 4.
    public void startLevel4ArrowLightingSequence() {
        if (level4ArrowsGameObjectParent == null) return;
        this.level4ArrowsGameObjectParent.SetActive(true);
        Debug.Log("Starting level 4 arrow lighting sequence");
        runLightingSequenceOn(level4Arrows, "level4");
    }   

    // Stops the arrow lighting sequence for Level 4.
    public void stopLevel4ArrowLightingSequence() {
        if (level4ArrowsGameObjectParent == null) return;
        stopLightingSequence("level4");
        this.level4ArrowsGameObjectParent.SetActive(false);
    }
    
    // Internal method to start a lighting sequence for a given list of arrows and a unique sequence ID.
    private void runLightingSequenceOn(List<GameObject> levelArrows, string sequenceId) {
        // If a sequence with this ID is already running, log a warning and do nothing.
        if (runningArrowLightingSequences.ContainsKey(sequenceId)) {
            Debug.LogWarning($"Lighting sequence {sequenceId} is already running. Stop it first.");
            return;
        }
        // If the list of arrows is null or empty, do nothing.
        if (levelArrows == null || levelArrows.Count == 0) {
            Debug.LogWarning($"Attempted to run lighting sequence {sequenceId} with no arrows.");
            return;
        }

        // Store the list of arrows and start the coroutine for the animation.
        runningArrowLightingSequences[sequenceId] = levelArrows;
        runningCoroutines[sequenceId] = coroutineRunner.StartCoroutine(LightArrowsSequence(levelArrows, sequenceId));
    }

    // Internal method to stop a lighting sequence identified by its ID.
    private void stopLightingSequence(string sequenceId) {
        // If no sequence with this ID is found running, log a warning and do nothing.
        if (!runningArrowLightingSequences.ContainsKey(sequenceId)) {
            Debug.LogWarning($"No lighting sequence running with ID {sequenceId}");
            return;
        }

        // Stop the associated coroutine if it exists.
        if (runningCoroutines.ContainsKey(sequenceId) && runningCoroutines[sequenceId] != null) {
            coroutineRunner.StopCoroutine(runningCoroutines[sequenceId]);
        }

        // Reset all arrows in this sequence to their default color.
        // Assumes each arrow GameObject has children named "Cube" and "Triangle" with MeshRenderer components.
        if (runningArrowLightingSequences.TryGetValue(sequenceId, out List<GameObject> arrowsInSequence)) {
            foreach (GameObject arrow in arrowsInSequence) {
                if (arrow == null) continue; // Skip null arrows in the list
                Transform cubeChildTransform = arrow.transform.Find("Cube");
                Transform triangleChildTransform = arrow.transform.Find("Triangle");
                
                if (cubeChildTransform != null && triangleChildTransform != null) {
                    MeshRenderer cubeChildMeshRenderer = cubeChildTransform.GetComponent<MeshRenderer>();
                    MeshRenderer triangleChildMeshRenderer = triangleChildTransform.GetComponent<MeshRenderer>();
                    if (cubeChildMeshRenderer != null && triangleChildMeshRenderer != null) {
                        cubeChildMeshRenderer.material.color = defaultColor;
                        triangleChildMeshRenderer.material.color = defaultColor;
                    }
                }
            }
        }

        // Remove the sequence from the tracking dictionaries.
        runningArrowLightingSequences.Remove(sequenceId);
        runningCoroutines.Remove(sequenceId);
    }

    // Coroutine that creates the sequential lighting animation for a list of arrows.
    private IEnumerator LightArrowsSequence(List<GameObject> arrows, string sequenceId) {
        // Continue looping as long as this sequence is registered as running.
        while (runningArrowLightingSequences.ContainsKey(sequenceId)) {
            // Iterate through each arrow in the list.
            foreach (GameObject arrow in arrows) {
                // If the sequence was stopped externally during iteration, break the inner loop.
                if (!runningArrowLightingSequences.ContainsKey(sequenceId)) break;
                if (arrow == null) continue; // Skip if an arrow in the list is null

                // Find the "Cube" and "Triangle" child GameObjects of the current arrow.
                // These are assumed to be the visual parts of the arrow.
                Transform cubeChildTransform = arrow.transform.Find("Cube");
                Transform triangleChildTransform = arrow.transform.Find("Triangle");
                
                if (cubeChildTransform != null && triangleChildTransform != null) {
                    MeshRenderer cubeChildMeshRenderer = cubeChildTransform.GetComponent<MeshRenderer>();
                    MeshRenderer triangleChildMeshRenderer = triangleChildTransform.GetComponent<MeshRenderer>();
                    
                    // If both parts have MeshRenderers, animate their material color.
                    if (cubeChildMeshRenderer != null && triangleChildMeshRenderer != null) {
                        // Set to highlight color.
                        cubeChildMeshRenderer.material.color = highlightColor;
                        triangleChildMeshRenderer.material.color = highlightColor;
                        // Wait for `lightDelay` seconds.
                        yield return new WaitForSeconds(lightDelay);
                        // Revert to default color (if sequence hasn't been stopped).
                        if (runningArrowLightingSequences.ContainsKey(sequenceId)) {
                           cubeChildMeshRenderer.material.color = defaultColor;
                           triangleChildMeshRenderer.material.color = defaultColor;
                        }
                    }
                } else {
                     Debug.LogWarning($"Arrow GameObject '{arrow.name}' is missing 'Cube' or 'Triangle' children for lighting sequence.");
                }
            }
            
            // Wait for a short period after completing one full pass through all arrows before starting the next pass.
            yield return new WaitForSeconds(0.5f);
        }
    }
}