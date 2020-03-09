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

        public Node[] Nodes
        {
            get
            {
                if (nodes == null)
                {
                    GetNodes();
                }

                return nodes;
            }
        }

        #endregion

        #region Static

        private static StateMachine openedAsset;

        [OnOpenAsset(1)]
        private static bool AssetOpened(int instanceID, int line)
        {
            var instance = EditorUtility.InstanceIDToObject(instanceID);

            if (instance is StateMachine)
            {
                openedAsset = instance as StateMachine;
                StateMachineEditor.OpenWindow();
                return true;
            }

            return false;
        }

        #endregion

        public Node[] GetNodes()
        {
            if (openedAsset == null)
            {
                return null;
            }

            var nodes = new Node[openedAsset.Nodes.Length];

            for (int i = 0; i < nodes.Length; i++)
            {
                nodes[i] = GetEditorNode(openedAsset.Nodes[i]);
            }

            ConnectNodes(nodes);
            this.nodes = nodes;

            return nodes;
        }

        public void AddNode(Node node)
        {
            if (IsAssetOpened)
            {
                openedAsset.AddNode(node.SMNode);
            }
        }

        public void RemoveNode(Node node)
        {
            if (IsAssetOpened)
            {
                openedAsset.RemoveNode(node.SMNode);
            }
        }

        private void ConnectNodes(Node[] nodes)
        {
            foreach (var n in nodes)
            {
                foreach (var connectedSMNode in n.SMNode.Connections)
                {
                    var secondNode = nodes.FirstOrDefault(nd => nd.SMNode == connectedSMNode);

                    if (secondNode != null)
                    {
                        new Connection(n, secondNode);
                    }
                }
            }
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
