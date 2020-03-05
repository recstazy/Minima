using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGraphObject
{
    Rect Rect { get; }
    void Draw();
}
