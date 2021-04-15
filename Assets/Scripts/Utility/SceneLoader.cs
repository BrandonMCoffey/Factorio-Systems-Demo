using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Utility {
    public class SceneLoader {
        public static void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public static void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public static void NextScene()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            if (currentScene.buildIndex + 1 < SceneManager.sceneCountInBuildSettings) {
                SceneManager.LoadScene(currentScene.buildIndex + 1);
            } else {
                Debug.Log("Warning: No scene found after " + currentScene.name + " in build settings.");
                SceneManager.LoadScene(0);
            }
        }
    }
}