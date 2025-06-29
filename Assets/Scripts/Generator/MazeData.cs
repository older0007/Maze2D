using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Generator
{
    public class MazeData
    {
        public enum TileType { Wall, Road, Start, Exit }
        public Vector2Int StartPosition { get; }
        public Vector3 StartPositionVector3 { get; }
        public TileType[,] TileTypes { get; }
        public TileBase[,] Tiles { get; }
        public List<Vector2Int> ExitPositions { get; }
        public List<Vector2Int> ShortestPath { get; }

        public int Seed { get; }
        //player visual size
        private Vector3 offset = new Vector3(0.35f, 0.35f, 0.35f);
        
        public MazeData(Vector2Int startPos, TileType[,] types, TileBase[,] tiles, List<Vector2Int> path, List<Vector2Int> exits, int seed, Tilemap tilemap)
        {
            StartPosition = startPos;
            TileTypes = types;
            Tiles = tiles;
            ShortestPath = path;
            ExitPositions = exits;

            Vector3Int position = (Vector3Int)startPos;
            StartPositionVector3 = tilemap.LocalToWorld(position) + tilemap.cellSize - offset;

            Seed = seed;
        }
    }
}