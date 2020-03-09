﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

namespace Minima.StateMachine.Editor
{
    public class ColumnContent : NodeContent
    {
        #region Fields

        private NodeContent[] contents = new NodeContent[0];

        #endregion

        #region Properties

        #endregion

        public ColumnContent(IGraphObject parent) : base(parent)
        {
        }

        public void AddContent(NodeContent content)
        {
            contents = contents.ConcatOne(content);
            content.UseParentRectCenter = false;
            content.AutoUpdateSize = false;
        }

        public override void Draw()
        {
            UpdateRect();
            base.Draw();
            DrawContents();
        }

        protected override void UpdateRectSize()
        {
            if (AutoUpdateSize)
            {
                rect.size = GetRawSize();
            }
        }

        public override Vector2 GetRawSize()
        {
            Vector2 size = DefaultSize;

            if (contents.Length > 0)
            {
                size.x = contents.Max(c => c.GetRawSize().x);

                float height = 0f;

                foreach (var c in contents)
                {
                    height += c.GetRawSize().y;
                }

                size.y = height;
            }

            return size;
        }

        private void DrawContents()
        {
            float lastOffset = 0f;

            for (int i = 0; i < contents.Length; i++)
            {
                var content = contents[i];
                var position = Rect.position + new Vector2(0f, lastOffset);
                content.SetRectPosition(position);
                content.SetRectSize(new Vector2(Rect.width, content.GetRawSize().y));
                lastOffset += content.Rect.height;
                content.Draw();
            }
        }
    }
}
