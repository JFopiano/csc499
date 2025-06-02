using UnityEngine;

public class PlaneController : MonoBehaviour
{
    public Material planeMaterial; // Reference to the material
    private GameObject plane;
    private bool isDragging = false;
    private Vector3 offset;

    void Update()
    {
        // Create a plane when the space button is pressed
        if (Input.GetKeyDown(KeyCode.Space) && plane == null)
        {
            plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            plane.transform.position = new Vector3(-7,0,-10);
            plane.transform.rotation = Quaternion.Euler(90, 0, -810);   

            // Assign the material to the plane
            if (planeMaterial != null)
            {
                plane.GetComponent<Renderer>().material = planeMaterial;
            }
        }

        // Check for mouse input
        if (plane != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit) && hit.transform == plane.transform)
                {
                    isDragging = true;
                    offset = plane.transform.position - hit.point;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }

            if (isDragging)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 newPosition = hit.point + offset;
                    plane.transform.position = new Vector3(plane.transform.position.x, plane.transform.position.y, newPosition.z);
                }
            }
        }
    }
}
