using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.LevelGeneration
{
    public class LevelGeneratorMultiroom : LevelGeneratorBase
    {
        #region Fields

        [SerializeField]
        private int roomsCount = 1;

        private List<RoomGenerator> roomGenerators = new List<RoomGenerator>();

        #endregion

        #region Properties

        #endregion

        protected override void GenerateLevel()
        {
            CreateGenerators();

            foreach (var g in roomGenerators)
            {
                g.GenerateRoom();
            }
        }

        protected virtual void CreateGenerators()
        {
            for (int i = 0; i < roomsCount; i++)
            {
                var generator = InstantiateGenerator(Vector2.zero);
                roomGenerators.Add(generator);
            }
        }
    }
}
