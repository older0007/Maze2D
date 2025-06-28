using UnityEngine;

namespace Generator
{
    public class MazeGenerator : MonoBehaviour
    {
        [SerializeField] private MazeRenderer mazeRenderer;
        [SerializeField] private GameConfig config;
        
        [ContextMenu("Test Generate")]
        private void Generate()
        {
            GenerateAndRenderLevel(1);
        }

        public void GenerateAndRenderLevel(int levelIndex)
        {
            var overrideData = config.GetOverride(levelIndex);
            var generator = overrideData != null && overrideData.generatorOverride != null
                ? overrideData.generatorOverride
                : config.DefaultGenerator;

            var tileSet = overrideData != null && overrideData.tileDataOverride != null
                ? overrideData.tileDataOverride
                : config.DefaultTiles;

            var maze = generator.Generate(levelIndex, tileSet, config.Seed);
            mazeRenderer.Render(maze);
        }
    }
}
