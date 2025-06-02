using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SVMButton : MonoBehaviour
{
    private Button button;
    public SVMInterface svmInterface;
    public TextMeshProUGUI accuracyText;

    void Awake()
    {
        // Get the Button component
        button = GetComponent<Button>();
        
        if (button == null)
        {
            Debug.LogError("No Button component found on " + gameObject.name);
            return;
        }

        // Remove any existing listeners
        button.onClick.RemoveAllListeners();
        
        // Add our click handler
        button.onClick.AddListener(() => {
            Debug.Log("SVM Button clicked!");
            if (svmInterface != null)
            {
                svmInterface.SolveCurrentLevel();
            }
            else
            {
                Debug.LogError("SVM Interface reference is missing!");
            }
        });

        Debug.Log("SVM Button initialized");
    }

    void OnEnable()
    {
        // Verify references
        if (svmInterface == null)
        {
            Debug.LogWarning("SVM Interface reference is missing on " + gameObject.name);
        }
    }

    void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    void Start()
    {
        Debug.Log($"Button position: {transform.position}");
        Debug.Log($"Button size: {GetComponent<RectTransform>().sizeDelta}");
        Debug.Log($"Button scale: {transform.localScale}");
    }
}