using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StateNode : Node
{
    #region Fields

    private State state;

    #endregion

    #region Properties

    #endregion

    public StateNode(Vector2 position, float width, float height) : base(position, width, height)
    {

    }

    public override void Draw()
    {
        base.Draw();
    }
}
