using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class FeatureSelector : MonoBehaviour
{
    public TMP_Dropdown xAxisDropdown;
    public TMP_Dropdown yAxisDropdown;
    public TMP_Dropdown species1Dropdown;
    public TMP_Dropdown species2Dropdown;
    public IrisVisualizer irisVisualizer;

    private string[] features = { "Sepal Length", "Sepal Width", "Petal Length", "Petal Width" };
    private string[] species = { "Iris-setosa", "Iris-versicolor", "Iris-virginica" };

    void Start()
    {
        if (xAxisDropdown != null && yAxisDropdown != null && 
            species1Dropdown != null && species2Dropdown != null)
        {
            PopulateDropdowns();
        }
        else
        {
            Debug.LogError("One or more dropdowns are missing. Please assign them in the Inspector.");
        }
    }

    void PopulateDropdowns()
    {
        // Clear all dropdowns
        xAxisDropdown.ClearOptions();
        yAxisDropdown.ClearOptions();
        species1Dropdown.ClearOptions();
        species2Dropdown.ClearOptions();

        // Add feature options
        List<TMP_Dropdown.OptionData> featureOptions = new List<TMP_Dropdown.OptionData>();
        foreach (string feature in features)
        {
            featureOptions.Add(new TMP_Dropdown.OptionData(feature));
        }

        // Add species options
        List<TMP_Dropdown.OptionData> speciesOptions = new List<TMP_Dropdown.OptionData>();
        foreach (string speciesName in species)
        {
            speciesOptions.Add(new TMP_Dropdown.OptionData(speciesName));
        }

        // Populate dropdowns
        xAxisDropdown.AddOptions(featureOptions);
        yAxisDropdown.AddOptions(featureOptions);
        species1Dropdown.AddOptions(speciesOptions);
        species2Dropdown.AddOptions(speciesOptions);

        // Add listeners
        xAxisDropdown.onValueChanged.AddListener(delegate { UpdateVisualization(); });
        yAxisDropdown.onValueChanged.AddListener(delegate { UpdateVisualization(); });
        species1Dropdown.onValueChanged.AddListener(delegate { UpdateVisualization(); });
        species2Dropdown.onValueChanged.AddListener(delegate { UpdateVisualization(); });

        // Set default values
        xAxisDropdown.value = 2; // Petal Length
        yAxisDropdown.value = 3; // Petal Width
        species1Dropdown.value = 0; // First species
        species2Dropdown.value = 1; // Second species

        UpdateVisualization();
    }

    void UpdateVisualization()
    {
        if (irisVisualizer != null)
        {
            irisVisualizer.UpdateVisualization(
                xAxisDropdown.value,
                yAxisDropdown.value,
                species[species1Dropdown.value],
                species[species2Dropdown.value]
            );
        }
        else
        {
            Debug.LogError("IrisVisualizer reference is missing");
        }
    }
}