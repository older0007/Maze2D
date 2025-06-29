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
        private const float Offset = 0.5f;
        
        public MazeData(Vector2Int startPos, TileType[,] types, TileBase[,] tiles, List<Vector2Int> path, List<Vector2Int> exits, int seed)
        {
            StartPosition = startPos;
            TileTypes = types;
            Tiles = tiles;
            ShortestPath = path;
            ExitPositions = exits;

            StartPositionVector3 = new Vector3(startPos.x + Offset, startPos.y + Offset, 0);
            Seed = seed;
        }
    }
}