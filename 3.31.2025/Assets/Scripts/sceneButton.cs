using UnityEngine;

public class AttachCanvasToPlane : MonoBehaviour
{
    public Canvas canvas;
    public GameObject planeGameObject;

    void Start()
    {
        canvas.transform.position = planeGameObject.transform.position;
        canvas.transform.rotation = planeGameObject.transform.rotation;
    }
}
