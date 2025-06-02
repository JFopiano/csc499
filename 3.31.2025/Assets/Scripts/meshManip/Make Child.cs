using UnityEngine;

public class SetParent : MonoBehaviour
{
    public GameObject dotPrefab;
    public GameObject plane;

    void Start()
    {
        // Set dotPrefab as a child of plane
        dotPrefab.transform.SetParent(plane.transform);
        Debug.Log("dotPrefab is now a child of the plane.");
    }
}
