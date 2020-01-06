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
        private int roomsCount = 1;

        [SerializeField]
        private GameObject roomGeneratorPrefab;

        private List<RoomGenerator> roomGenerators = new List<RoomGenerator>();

        #endregion

        #region Properties

        #endregion

        private void Awake()
        {
            GenerateLevel();
        }

        private void GenerateLevel()
        {
            CreateGenerators();

            foreach (var g in roomGenerators)
            {
                g.GenerateRoom();
            }
        }

        void CreateGenerators()
        {
            for (int i = 0; i < roomsCount; i++)
            {
                var generator = InstantiateGenerator();
                roomGenerators.Add(generator);
            }
        }

        private RoomGenerator InstantiateGenerator()
        {
            var generator = Instantiate(roomGeneratorPrefab, Vector3.zero, Quaternion.identity, roomsParent);
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