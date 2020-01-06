using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Minima.LevelGeneration
{
    public class LevelGeneratorBase : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private Transform roomsParent;

        [SerializeField]
        private GameObject roomGeneratorPrefab;

        #endregion

        #region Properties

        #endregion

        private void Awake()
        {
            GenerateLevel();
        }

        protected virtual void GenerateLevel()
        {
            var generator = InstantiateGenerator(Vector2.zero);
            generator.GenerateRoom();
        }

        protected RoomGenerator InstantiateGenerator(Vector2 position)
        {
            var generator = Instantiate(roomGeneratorPrefab, position, Quaternion.identity, roomsParent);
            return generator.GetComponent<RoomGenerator>();
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