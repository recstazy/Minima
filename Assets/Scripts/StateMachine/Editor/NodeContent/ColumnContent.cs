using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class ColumnContent : NodeContent
{
    #region Fields

    private IGraphObject[] contents = new IGraphObject[0];

    #endregion

    #region Properties
    
    #endregion

    public ColumnContent(IGraphObject parent) : base(parent)
    {

    }

    public void AddContent(IGraphObject content)
    {
        contents = contents.ConcatOne(content);
        content.UseParentRectCenter = false;
    }

    public override void Draw()
    {
        UpdateRect();
        base.Draw();
        DrawContents();
    }
        

    protected override void UpdateRectSize()
    {
        if (contents.Length > 0)
        {
            rect.width = contents.Max(c => c.Rect.width);

            float height = 0f;

            foreach (var c in contents)
            {
                height += c.Rect.height;
            }

            rect.height = height;
        }
        else
        {
            base.UpdateRectSize();
        }
    }

    private void DrawContents()
    {
        float lastOffset = 0f;

        for (int i = 0; i < contents.Length; i++)
        {
            var content = contents[i];
            content.SetRectPosition(Rect.position + new Vector2(0f, lastOffset));
            lastOffset += content.Rect.height;
            content.Draw();
        }
    }
}
