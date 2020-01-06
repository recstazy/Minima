using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Minima.LevelGeneration
{
    public class LevelGeneratorBase : MonoBehaviour
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        void Start()
        {

        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}