using System.Collections.Generic;
using UnityEngine;

namespace Generator
{
    [CreateAssetMenu(menuName = "Maze/Game Config")]
    public class GameConfig : ScriptableObject
    {
        [SerializeField] private int globalSeed = -1;
        [SerializeField] private int maxLevelCount = 10;
        [SerializeField] private List<LevelOverride> levelOverrides;
        [SerializeField] private MazeGeneratorBase baseGenerator;
        [SerializeField] private TileData baseTileData;
    
        public int Seed => globalSeed;
        public int MaxLevels => maxLevelCount;
        public MazeGeneratorBase DefaultGenerator => baseGenerator;
        public TileData DefaultTiles => baseTileData;

        public LevelOverride GetOverride(int level)
        {
            return levelOverrides.Find(x => x.levelIndex == level);
        }

        [System.Serializable]
        public class LevelOverride
        {
            public int levelIndex;
            public TileData tileDataOverride;
            public MazeGeneratorBase generatorOverride;
        }
    }
}
