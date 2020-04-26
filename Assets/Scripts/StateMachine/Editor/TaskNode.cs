using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Minima.StateMachine.Editor
{
    public class TaskNode : Node
    {
        #region Fields

        protected TaskProvider taskProvider;

        #endregion

        #region Properties

        #endregion

        public TaskNode(Vector2 position, NodeType nodeType) : base(position, nodeType) 
        {
            NodeType = nodeType;
            AddTaskProvider();
            Content.OnContentAdded += ContentAdded;
            Content.OnContentRemoved += ContentRemoved;
        }

        public TaskNode(Minima.StateMachine.Node node) : base(node)
        {
            NodeType = node.NodeType;

            if (SMNode.TasksEditable)
            {
                AddTaskProvider();
            }
            
            CreateTasks();

            if (SMNode.TasksEditable)
            {
                Content.OnContentAdded += ContentAdded;
                Content.OnContentRemoved += ContentRemoved;
            }
        }

        public override void Draw()
        {
            base.Draw();

            if (taskProvider != null)
            {
                taskProvider.Draw();
            }
        }

        private void AddTaskProvider()
        {
            taskProvider = new TaskProvider(this, SMNode.NodeType);
            taskProvider.UseParentRectCenter = false;
            taskProvider.AutoUpdateSize = false;
            taskProvider.OnTaskSelected += TaskCreated;
            AddContent(taskProvider);
        }

        protected void CreateTasks()
        {
            foreach (var t in SMNode.Tasks)
            {
                TaskCreated(t);
            }
        }

        protected void TaskCreated(Task task)
        {
            var taskView = new TaskView(this, task);
            taskView.UseParentRectCenter = false;
            taskView.AutoUpdateSize = false;
            AddContent(taskView);
        }

        private void ContentAdded(NodeContent content)
        {
            if (content is TaskView)
            {
                var task = (content as TaskView).Task;
                SMNode.AddTask(task);
            }
        }

        private void ContentRemoved(NodeContent content)
        {
            if (content is TaskView)
            {
                var task = (content as TaskView).Task;
                SMNode.RemoveTask(task);
            }
        }
    }
}
