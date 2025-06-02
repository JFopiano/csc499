using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonDebug : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    void Start()
    {
        Debug.Log("ButtonDebug: Started on " + gameObject.name);
        
        // Check for required components
        Button button = GetComponent<Button>();
        if (button == null) Debug.LogError("No Button component found!");
        
        // Check if button is interactable
        if (button != null && !button.interactable)
            Debug.LogError("Button is not interactable!");
            
        // Check if there's a graphic raycaster
        var canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            var raycaster = canvas.GetComponent<GraphicRaycaster>();
            if (raycaster == null)
                Debug.LogError("No GraphicRaycaster found on Canvas!");
        }
        else
            Debug.LogError("No Canvas found in parents!");
            
        // Check for EventSystem
        if (EventSystem.current == null)
            Debug.LogError("No EventSystem found in scene!");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Button clicked!");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer entered button!");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer exited button!");
    }
}