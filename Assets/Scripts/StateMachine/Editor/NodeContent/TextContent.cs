using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextContent : NodeContent
{
    #region Fields

    protected float rectSizeMultiplier = 1.1f;
    protected int defaultFontSize = 12;

    #endregion

    #region Properties

    public string Text { get; set; } = "";
    public int FontSize { get => style.fontSize; set => style.fontSize = value; }

    #endregion

    public TextContent(IGraphObject parent, string text) : base(parent)
    {
        Text = text;
    }

    public override void Draw()
    {
        GUI.Label(rect, Text, style);
    }

    protected override void UpdateRect()
    {
        rect.center = parent.Rect.center;

        if (Text.Length > 0)
        {
            int width = 0;

            foreach (var c in Text)
            {
                CharacterInfo info;
                style.font.GetCharacterInfo(c, out info, style.fontSize);
                width += info.advance;
            }

            var size = new Vector2(width * rectSizeMultiplier, style.fontSize * rectSizeMultiplier);

            if (size.x < DefaultSize.x)
            {
                size.x = DefaultSize.x;
            }

            if (size.y < DefaultSize.y)
            {
                size.y = DefaultSize.y;
            }

            rect.size = size;
        }
        else
        {
            rect.size = DefaultSize;
        }
    }

    protected override void CreateStyle()
    {
        style = new GUIStyle();
        style.fontSize = defaultFontSize;
        style.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        style.clipping = TextClipping.Overflow;
        style.alignment = TextAnchor.MiddleCenter;
    }
}
