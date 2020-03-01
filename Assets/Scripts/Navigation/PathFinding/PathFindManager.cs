using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.Navigation
{
    public class PathFindManager : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private NavMeshBuilderBase navMeshBuilder;

        [SerializeField]
        private int maxParallel = 1;

        private Queue<PathFindTask> tasksQueue = new Queue<PathFindTask>();
        private int runningTasksCount = 0;

        #endregion

        #region Properties

        #endregion

        private void Awake()
        {
            StartCoroutine(SheduleProcessingLoop());
        }

        public void FindPath(Vector2 origin, Vector2 target, PathFoundHandler pathFoundCallback)
        {
            var task = new PathFindTask(origin, target, pathFoundCallback, new NavPathFinder(navMeshBuilder));
            SheduleTask(task);
        }

        private void RunNextTask()
        {
            runningTasksCount++;
            var newTask = tasksQueue.Dequeue();
            newTask.OnComplete += PathFound;
            newTask.Run();
        }

        private void PathFound(PathFindTask task)
        {
            task.OnComplete -= PathFound;
            runningTasksCount--;
        }

        private void SheduleTask(PathFindTask task)
        {
            tasksQueue.Enqueue(task);
        }

        private IEnumerator SheduleProcessingLoop()
        {
            while(true)
            {
                yield return new WaitUntil(() => CanRunTask());
                RunNextTask();
            }
        }

        private bool CanRunTask()
        {
            return tasksQueue.Count > 0 && runningTasksCount < maxParallel;
        }
    }
}
