using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using Minima.StateMachine;

namespace Minima.StateMachine.Editor
{
    public class StateMachineIO
    {
        #region Fields

        private Node[] nodes;

        #endregion

        #region Properties

        public StateMachine OpenedAsset { get => openedAsset; }
        public bool IsAssetOpened { get => openedAsset != null; }

        #endregion

        #region Static

        private static StateMachine openedAsset;
        public static int AssetId { get; private set; }

        [OnOpenAsset(1)]
        private static bool AssetOpened(int instanceID, int line)
        {
            if (TryOpenAsset(instanceID))
            {
                AssetId = instanceID;
                StateMachineEditor.OpenWindow();
                return true;
            }

            return false;
        }

        private static bool TryOpenAsset(int id)
        {
            var instance = EditorUtility.InstanceIDToObject(id);

            if (instance is StateMachine)
            {
                openedAsset = instance as StateMachine;
                return true;
            }

            return false;
        }

        [DidReloadScripts]
        private static void ScriptsReloaded()
        {
            openedAsset = null;
        }

        #endregion

        public bool TryOpenAssetByID(int id)
        {
            if (TryOpenAsset(id))
            {
                return true;
            }

            return false;
        }

        public Node[] GetNodes()
        {
            if (openedAsset == null)
            {
                return null;
            }

            CreateTasks();

            var nodes = new Node[openedAsset.Nodes.Length];

            for (int i = 0; i < nodes.Length; i++)
            {
                nodes[i] = GetEditorNode(openedAsset.Nodes[i]);
            }

            ConnectNodes(nodes);
            this.nodes = nodes;
            return nodes;
        }

        public void SetDirty()
        {
            EditorUtility.SetDirty(openedAsset);
        }

        public void AddNode(Node node)
        {
            if (IsAssetOpened)
            {
                openedAsset.AddNode(node.SMNode);
                SetDirty();
            }
        }

        public void RemoveNode(Node node)
        {
            if (IsAssetOpened)
            {
                openedAsset.RemoveNode(node.SMNode);
                SetDirty();
            }
        }

        private void ConnectNodes(Node[] nodes)
        {
            foreach (var n in nodes)
            {
                foreach (var connectedSMNode in n.SMNode.Connections)
                {
                    var secondNode = nodes.FirstOrDefault(nd => nd.SMNode.ID == connectedSMNode);

                    if (secondNode != null)
                    {
                        new Connection(n, secondNode);
                    }
                }
            }
        }

        private void CreateTasks()
        {
            foreach (var n in openedAsset.Nodes)
            {
                var tasks = ParseTasksForNode(n);
                n.SetTasks(tasks);
            }
        }

        private Task[] ParseTasksForNode(Minima.StateMachine.Node node)
        {
            var taskInfo = node.TaskInfo;
            var tasks = new Task[0];

            if (taskInfo.Length > 0)
            {
                foreach (var t in taskInfo)
                {
                    var task = t.CreateTaskInstance();
                    tasks = tasks.ConcatOne(task);
                }
            }

            return tasks;
        }

        private Node GetEditorNode(Minima.StateMachine.Node node)
        {
            var type = node.NodeType;

            if (type == NodeType.Task)
            {
                return new TaskNode(node);
            }

            Debug.LogError("No implementation of creating " + type.ToString());
            return null;
        }
    }
}
