using UnityEngine;
using UnityEngine.Tilemaps;

namespace Generator
{
    public abstract class MazeGeneratorBase : ScriptableObject
    {
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private int deadEndCount;
        [SerializeField] private int startRoomSize;
        [SerializeField] private RoomShape startRoomShape;
        [SerializeField] private int exitCount = 1;
        [SerializeField] private int minDistToExit = 3;

        public abstract MazeData Generate(int levelIndex, TileData tileData, Tilemap map, int seed = -1);

        public enum RoomShape { Square = 1, Circle = 2 }

        protected int ResolveSeed(int levelIndex, int seed)
        {
            return seed == -1 ? (int)System.DateTime.Now.Ticks + levelIndex : seed + levelIndex;
        }

        public int Width => width;
        public int Height => height;
        public int StartRoomSize => startRoomSize;
        public RoomShape StartShape => startRoomShape;
        public int DeadEnds => deadEndCount;
        public int ExitCount => exitCount;
        public int MinDistToExit => minDistToExit;
    }
}