using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.LevelGeneration
{
    public class LevelGeneratorMultiroom : LevelGeneratorBase
    {
        #region Fields

        [SerializeField]
        private float roomRawOffset = 26f;

        [SerializeField]
        private int roomsCount = 5;

        [SerializeField]
        private int generatorExitsPerRoom = 2;

        private List<RoomGenerator> roomGenerators = new List<RoomGenerator>();
        private List<ExitCorner> roomsExits = new List<ExitCorner>();

        #endregion

        #region Properties

        #endregion

        protected override void GenerateLevel()
        {
            CreateGenerators(InstantiateGenerator(Vector2.zero));
            GenerateRooms();
            SnapRooms();
        }

        /// <summary>
        /// Recurcively create rooms
        /// </summary>
        protected virtual bool CreateGenerators(RoomGenerator roomGenerator)
        {
            roomGenerators.Add(roomGenerator);
            roomsExits = roomsExits.Concat(roomGenerator.RoomDraft.Exits).ToList();

            if (roomGenerators.Count >= roomsCount)
            {
                return false;
            }

            var exits = GetRandomExits(generatorExitsPerRoom);

            foreach (var e in exits)
            {
                var generator = InstantiateGenerator(GetPositionForNextRoom(e));
                bool succes = CreateGenerators(generator);

                if (!succes)
                {
                    return false;
                }
            }

            return true;
        }

        private void GenerateRooms()
        {
            foreach (var g in roomGenerators)
            {
                g.ExitsCount = Random.Range(4, 4);
                g.GenerateRoom();
            }
        }

        private void SnapRooms()
        {
            foreach (var g in roomGenerators)
            {
                var room = g.RoomDraft;

                foreach (var e in room.Exits)
                {
                    e.NextRoom.transform.position += e.position - e.NextExit.position;
                }
            }
        }

        private List<ExitCorner> GetRandomExits(int count)
        {
            int countClamp = Mathf.Clamp(count, 0, 4);

            List<ExitCorner> result = new List<ExitCorner>();
            roomsExits.Shuffle();

            foreach (var e in roomsExits)
            {
                if (e.NextRoom == null)
                {
                    result.Add(e);

                    if (result.Count >= countClamp)
                    {
                        break;
                    }
                }
            }

            return result;
        }

        private Vector2 GetPositionForNextRoom(ExitCorner exit)
        {
            var exitParent = exit.transform.parent;
            var direction = exit.position - exitParent.position;
            var newPosition = exitParent.position + direction.normalized * roomRawOffset;
            return newPosition;
        }
    }
}
