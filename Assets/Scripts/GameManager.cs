using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap baseLayer;

    [SerializeField]
    private Tilemap pathLayer;

    private List<Vector2Int> buildLocations = new List<Vector2Int>();
    public void Start()
    {
        if(baseLayer != null)
        {
            // get build spots
            var bounds = baseLayer.cellBounds;
            var cells = baseLayer.GetTilesBlock(bounds);

            Debug.Log("Bounds: ");
            Debug.Log(bounds);
            for (int x = 0; x < bounds.size.x; x++)
            {
                for (int y = 0; y < bounds.size.y; y++)
                {
                    var cell = cells[x + y * bounds.size.x];

                    if (cell is BuildTile)
                    {
                        Debug.Log("Found build tile");
                        Debug.Log(cell);

                        var position = new Vector2Int(x + bounds.xMin, y + bounds.yMin);

                        Debug.Log("Position");
                        Debug.Log(position);

                    }
                }
            }

        }
    }
}
