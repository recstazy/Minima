using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Minima.StateMachine
{
    public class StateMachineController : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private StateMachine stateMachine;

        private Node currentState;
        private Node[] currentConditions;
        private Node[] nextStates;
        private List<Node> stateNodes;

        #endregion

        #region Properties

        #endregion

        private void Awake()
        {
            stateNodes = stateMachine.Nodes.Where(n => n.NodeType == NodeType.State).ToList();
            currentState = stateNodes.FirstOrDefault(n => n is EntryNode);

            if (currentState == null)
            {
                Debug.LogError("Couldn't find entry node");
                return;
            }

            CurrentStateEnter();
        }

        private void CurrentStateEnter()
        {
            var currentConnected = GetCurrentConnected();

            nextStates = currentConnected.Where(c => c.NodeType == NodeType.State).ToArray();
            currentConditions = currentConnected.Where(c => c.NodeType == NodeType.Condition).ToArray();

            foreach (var t in currentState.Tasks)
            {
                (t as StateTask).OnStateEnter();
            }
        }

        private void CurrentStateExit()
        {
            foreach (var t in currentState.Tasks)
            {
                (t as StateTask).OnStateExit();
            }
        }

        private Node[] GetCurrentConnected()
        {
            var result = currentState.Connections
                .Select(c => stateMachine.Nodes
                .FirstOrDefault(n => n.ID == c))
                .Where(n => n != default)
                .ToArray();

            return result;
        }
    }
}
