using System;
using System.Collections;
using System.Globalization;
using Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Utils;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private GameObject cheats;
        [SerializeField] private UnityEvent onPlayClick;
        [SerializeField] private float loadingDelay;

        [SerializeField] private TMP_InputField seedInputField;
        [SerializeField] private TMP_InputField levelInputField;
        
        private SaveController saveController;
        
        
        private void Awake()
        {
#if UNITY_EDITOR
            cheats.SetActive(true);
#else
            cheats.SetActive(false);
#endif

            saveController = new SaveController();
            saveController.Load();
        }

        public void Start()
        {
            levelText.text = saveController.PlayerData.LevelIndex.ToString();
        }

        private void UpdateLevelText()
        {
            levelText.text = saveController.PlayerData.LevelIndex.ToString();
        }

        private void OnEnable()
        {
            seedInputField.onEndEdit.AddListener(OnSeedChanged);
            levelInputField.onEndEdit.AddListener(OnLevelChanged);
        }

        private void OnDisable()
        {
            seedInputField.onEndEdit.RemoveListener(OnSeedChanged);
            levelInputField.onEndEdit.RemoveListener(OnLevelChanged);
        }

        private void OnLevelChanged(string level)
        {
            if (int.TryParse(level, out int result))
            {
                saveController.PlayerData.LevelIndex = result;
                saveController.Save();
            }
        }

        private void OnSeedChanged(string seed)
        {
            if (int.TryParse(seed, out int result))
            {
                saveController.PlayerData.Seed = result;
                saveController.Save();
            }
        }

        public void Play(bool debug)
        {
            if (!debug)
            {
                saveController.PlayerData.Seed = -1;
                saveController.Save();
            }

            StartCoroutine(LoadGameScene());
        }

        public void DeleteSave()
        {
            saveController.Clear();
            UpdateLevelText();
        }

        private IEnumerator LoadGameScene()
        {
            onPlayClick?.Invoke();
            yield return SceneController.LoadGamePlayScene(loadingDelay);
        }
    }
}