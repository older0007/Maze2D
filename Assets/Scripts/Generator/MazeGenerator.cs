using UnityEngine;

namespace Generator
{
    public class MazeGenerator : MonoBehaviour
    {
        [SerializeField] private MazeRenderer mazeRenderer;
        [SerializeField] private GameConfig config;

#if UNITY_EDITOR
        [ContextMenu("Test Generate")]
        private void Generate()
        {
            GenerateAndRenderLevel(0);
        }

        [ContextMenu("Clear")]
        private void Clear()
        {
            mazeRenderer.Clear();
        }
#endif
       
        public MazeData GenerateAndRenderLevel(int levelIndex, int overrideSeed = -1)
        {
            var overrideData = config.GetOverride(levelIndex);
            var generator = overrideData != null && overrideData.generatorOverride != null
                ? overrideData.generatorOverride
                : config.DefaultGenerator;

            var tileSet = overrideData != null && overrideData.tileDataOverride != null
                ? overrideData.tileDataOverride
                : config.DefaultTiles;

            var seed = overrideSeed == -1 ? config.Seed : overrideSeed;
            var maze = generator.Generate(levelIndex, tileSet, seed);
            
            mazeRenderer.Render(maze);
            
            return maze;
        }
    }
}
