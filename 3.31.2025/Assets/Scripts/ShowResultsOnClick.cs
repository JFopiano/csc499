using UnityEngine;

public class ClickButtonActivator : MonoBehaviour
{

    public GameObject dataFile;

    private void Start()
    {

        if (dataFile != null)
            dataFile.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 0 = left mouse button
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    ActivateObjects();
                }
            }
        }
    }

    private void ActivateObjects()
    {
        if (dataFile != null)
            dataFile.SetActive(true);

        Debug.Log("Object clicked – displaying data.");
    }
}