using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
    public static class SceneChanger
    {

        private const int MainMenuSceneIndex = 1;
        private const int LevelSelectionSceneIndex = 2;
        private const int GameSceneIndex = 3;
        private const int GameResultsSceneIndex = 4;

        public static void StartMainMenuScene()
        {
            Debug.Log("Start main menu");
            SceneManager.LoadScene(MainMenuSceneIndex);
        }

        public static void StartGameResultsScene()
        {
            Debug.Log("Start lose");
            SceneManager.LoadScene(GameResultsSceneIndex);
        }
        
        
        public static void StartGameScene()
        {
            Debug.Log("Start game");
            SceneManager.LoadScene(GameSceneIndex);
        }

        public static void StartLevelSelection()
        {
            Debug.Log("Start level selection");
            SceneManager.LoadScene(LevelSelectionSceneIndex);
        }

        public static void Quit()
        {
            Debug.Log("Quit from app");
            Application.Quit();
        }

    }
}