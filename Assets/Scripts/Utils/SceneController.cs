using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
    public class SceneController
    {
        private const string GamePlayScene = "GamePlay";
        private const string MainScene = "MainMenu";

        public static void LoadMeinMenu()
        {
            SceneManager.LoadScene(MainScene);
        }

        public static IEnumerator LoadGamePlayScene(float loadingDelay)
        {
            yield return new WaitForSeconds(loadingDelay);
            yield return SceneManager.LoadSceneAsync(GamePlayScene);
        }
    }
}