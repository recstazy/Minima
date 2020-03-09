using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

namespace Minima.StateMachine.Editor
{
    public class NodeContent : IGraphObject
    {
        public event Action<NodeContent> OnRemoveContentClicked;

        #region Fields

        protected Rect rect;
        protected IGraphObject parent;
        protected GUIStyle style;
        protected GenericMenu contextMenu;

        #endregion

        #region Properties

        public Vector2 DefaultSize { get; set; } = new Vector2(200f, 50f);
        public bool UseParentRectCenter { get; set; } = true;
        public bool AutoUpdateSize { get; set; } = true;

        public Rect Rect
        {
            get
            {
                return rect;
            }
        }

        #endregion

        public NodeContent(IGraphObject parent)
        {
            this.parent = parent;

            CreateStyle();
            CreateContextMenu();
            UpdateRect();
        }

        public virtual void Draw()
        {
            UpdateRect();
            GUI.Box(Rect, "", style);
        }

        public virtual bool ProcessEvent(Event e, SMEditorEventArgs eventArgs)
        {
            bool used = false;

            if (e.type == EventType.MouseUp)
            {
                if (e.button == 1)
                {
                    if (rect.Contains(e.mousePosition))
                    {
                        if (contextMenu != null)
                        {
                            contextMenu.ShowAsContext();
                            used = true;
                        }
                    }
                }
            }

            return used;
        }

        public void SetRectPosition(Vector2 position)
        {
            if (!UseParentRectCenter)
            {
                rect.position = position;
            }
        }

        public void SetRectSize(Vector2 size)
        {
            if (!AutoUpdateSize)
            {
                rect.size = size;
            }
        }

        public virtual Vector2 GetRawSize()
        {
            return DefaultSize;
        }

        protected virtual void UpdateRect()
        {
            UpdateRectCenter();
            UpdateRectSize();
        }

        protected virtual void UpdateRectCenter()
        {
            if (UseParentRectCenter)
            {
                rect.center = parent.Rect.center;
            }
        }

        protected virtual void UpdateRectSize()
        {
            if (AutoUpdateSize)
            {
                rect.size = GetRawSize();
            }
        }

        protected void CallOnRemove()
        {
            OnRemoveContentClicked?.Invoke(this);
        }

        protected virtual void CreateStyle()
        {
            style = (GUIStyle)"flow node 0";
        }

        protected virtual void CreateContextMenu()
        {
            contextMenu = new GenericMenu();
            contextMenu.AddItem(new GUIContent("Remove" + this.ToString()), false, CallOnRemove);
        }
    }
}
