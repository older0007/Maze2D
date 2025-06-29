using System;
using System.Collections.Generic;
using Events;
using Gameplay;
using Generator;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private MazeGenerator mazeGenerator;
        [SerializeField] private Player player;
        [SerializeField] private TriggerEnterEvent playerExitTrigger;
        [SerializeField] private GameConfig gameConfig;
        [SerializeField] private GamePlayMenu gamePlayMenu;
        [SerializeField] private List<Transform> vfxTransforms;

        private MazeData level;
        private Timer gameplayTimer;
        public SaveController SaveController { get; private set; }

        public void GoToMainMenu()
        {
            SceneController.LoadMeinMenu();
        }

        private void Awake()
        {
            SaveController = new SaveController();
            gameplayTimer = new Timer();
            SaveController.Load();
        }

        private void Start()
        {
           StartLevel();
        }

        private void StartLevel()
        {
            var currentLevel = SaveController.PlayerData.LevelIndex;
            level = mazeGenerator.GenerateAndRenderLevel(SaveController.PlayerData.LevelIndex, SaveController.PlayerData.Seed);

            player.gameObject.transform.position = level.StartPositionVector3;
            player.gameObject.SetActive(true);

            for (var index = 0; index < vfxTransforms.Count; index++)
            {
                var vfx = vfxTransforms[index];

                vfx.position = level.StartPositionVector3;
            }

            gamePlayMenu.Show(currentLevel, level.Seed);
        }

        private void Update()
        {
            gameplayTimer.Update();
            gamePlayMenu.UpdateTimer(gameplayTimer.ToString());
        }

        private void OnEnable()
        {
            playerExitTrigger.OnPlayerEnter += OnLevelComplete;
        }

        private void OnLevelComplete()
        {
            var nextLevel = SaveController.PlayerData.LevelIndex + 1;
            SaveController.PlayerData.LevelIndex = Math.Min(nextLevel, gameConfig.MaxLevels);
            SaveController.PlayerData.Seed = level.Seed;
            
            SaveController.Save();
            gameplayTimer.Clear();
            
            StartLevel();
        }

        private void OnDisable()
        {
            playerExitTrigger.OnPlayerEnter -= OnLevelComplete;
        }
    }
}