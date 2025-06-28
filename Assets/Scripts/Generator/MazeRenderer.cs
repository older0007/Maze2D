using UnityEngine;
using UnityEngine.Tilemaps;

namespace Generator
{
    public class MazeRenderer : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private GameConfig config;

        public void Render(MazeData data)
        {
            tilemap.ClearAllTiles();
            int width = data.TileTypes.GetLength(0);
            int height = data.TileTypes.GetLength(1);

            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                TileBase tile = data.Tiles[x, y];
                if (tile != null)
                    tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }
}