using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGraphObject
{
    Vector2 DefaultSize { get; }
    Rect Rect { get; }
    void Draw();
}
