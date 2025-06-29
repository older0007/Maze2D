using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelCompletePopup : MonoBehaviour
    {
        [SerializeField] private TMP_Text distanceText;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button exitButton;

        private Action onContinue;
        private Action onExit;
        private StringBuilder builder = new();

        private void OnEnable()
        {
            continueButton.onClick.AddListener(ContinuePlay);
            exitButton.onClick.AddListener(ExitGame);
        }

        private void OnDisable()
        {
            continueButton.onClick.RemoveListener(ContinuePlay);
            exitButton.onClick.RemoveListener(ExitGame);
        }

        public void Init(float distance, Action onContinue, Action onExit)
        {
            this.onContinue = onContinue;
            this.onExit = onExit;
            
            builder.Clear();
            builder.Append(distance);
            distanceText.SetText(builder);
        }

        private void ContinuePlay()
        {
            onContinue?.Invoke();
            
            Destroy(gameObject);
        }
        
        private void ExitGame()
        {
            onExit?.Invoke();
        }
    }
}