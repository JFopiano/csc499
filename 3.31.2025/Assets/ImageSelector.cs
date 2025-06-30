using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ImageSelector : MonoBehaviour
{

    public TMP_Dropdown dropdown;
    public RawImage imageDisplay;
    public Button showButton;

    private List<string> imageNames = new List<string>()
    {
        "AreaMeanColor", "CompactnessMeanColor", "ConcavePointMeanColor", "ConcavityMeanColor", "FractalDimensionMeanColor", "PerimeterMeanColor", "RadiusMeanColor", "SmoothnessMeanColor", "SymmetryMeanColor", "TextureMeanColor"
    };


    // Start is called before the first frame update
    void Start()
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(imageNames);

        showButton.onClick.AddListener(DisplaySelectedImage);

    }

    void DisplaySelectedImage()
    {
        string selectedImageName = imageNames[dropdown.value];

        Texture2D tex = Resources.Load<Texture2D>("Images/" + selectedImageName);

        if (tex != null)
        {
            imageDisplay.texture = tex;
            imageDisplay.SetNativeSize();
        }
        else
        {
            Debug.LogError("Image Not Found: " + selectedImageName);
        }
    }
}
