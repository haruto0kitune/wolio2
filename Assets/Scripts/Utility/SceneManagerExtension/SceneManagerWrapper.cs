using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace MyUtility
{
    public static class SceneManagerWrapper
    {
        public static void NextScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public static void PreviousScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
}