using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public int gridWidth = 10;
    public int gridHeight = 10;
    public float spacing = 1.1f; // Add a bit of space between tiles

    void Start()
    {

        if (tilePrefab == null)
        {
            Debug.LogError("Tile prefab is NOT assigned in the inspector!");
            return;
        }

        Debug.Log("Generating grid...");
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 position = new Vector3(x * spacing, 0, y * spacing);

                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, transform);
                tile.name = $"Tile_{x}_{y}";

                Tile tileScript = tile.GetComponent<Tile>();
                if (tileScript != null)
                {
                    tileScript.SetNumber(y); // Or whatever value you want to show
                }

                Debug.Log($"Spawning tile at: {position}");
            }
        }
    }

}
