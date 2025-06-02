using System.Collections.Generic;
using UnityEngine;

public class SeparatorCollisionHandler : MonoBehaviour
{
    public Color brighterRed = new Color(1f, 0.5f, 0.5f); // Brighter red color
    public Color brighterBlue = new Color(0.5f, 0.5f, 1f); // Brighter blue color

    // Dictionary to store the original colors of spheres
    private Dictionary<GameObject, Color> originalColors = new Dictionary<GameObject, Color>();

    void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is a sphere
        if (other.CompareTag("RedSphere") || other.CompareTag("BlueSphere"))
        {
            Renderer renderer = other.GetComponent<Renderer>();
            if (renderer != null)
            {
                // Store the original color if it's not already stored
                if (!originalColors.ContainsKey(other.gameObject))
                {
                    originalColors[other.gameObject] = renderer.material.color;
                }

                // Change to brighter color based on the tag
                if (other.CompareTag("RedSphere"))
                {
                    renderer.material.color = brighterRed;
                }
                else if (other.CompareTag("BlueSphere"))
                {
                    renderer.material.color = brighterBlue;
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Revert to the original color when the separator stops colliding
        if (originalColors.ContainsKey(other.gameObject))
        {
            Renderer renderer = other.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = originalColors[other.gameObject]; // Revert to the stored original color
            }
        }
    }
}
