using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FPS
{
    public class SceneLoader : MonoBehaviour
    {
        #region Public Methods

        public void LoadScene(int index)
        {
            SceneManager.LoadScene(index);
        }
        public void ExitFromGame()
        {
            Application.Quit();
        }

        #endregion
    }
}
