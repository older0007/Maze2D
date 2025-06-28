using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Generator
{
    [CreateAssetMenu(menuName = "Maze/Tile Data")]
    public class TileData : ScriptableObject
    {
        [SerializeField] private List<TileBase> wallTiles;
        [SerializeField] private List<TileBase> roadTiles;
        [SerializeField] private TileBase startRoomTile;
        [SerializeField] private TileBase exitTile;

        public TileBase GetRandomWall() => wallTiles[Random.Range(0, wallTiles.Count)];
        public TileBase GetRandomRoad() => roadTiles[Random.Range(0, roadTiles.Count)];
        public TileBase GetStartRoomTile() => startRoomTile;
        public TileBase GetExitTile() => exitTile;
    }
}