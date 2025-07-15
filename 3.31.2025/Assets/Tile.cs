using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private TMP_Text numberText;
    private Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    public void SetNumber(int num)
    {
        if (numberText != null)
        {
            numberText.text = num.ToString();
        }
        else
        {
            Debug.LogError("numberText is not assigned!");
        }
    }

    public void SetColor(Color color)
    {
        if (rend != null)
        {
            rend.material.color = color;
        }
    }
}
