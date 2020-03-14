using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;
using UnityEditor;

namespace Minima.StateMachine.Editor
{
    public class TaskFieldContent : NodeContent
    {
        public event Action<Type> OnValueChanged;

        #region Fields

        protected FieldInfo field;

        protected Rect nameRect;
        protected Rect valueRect;
        protected int inputFieldWidth = 100;
        protected int fontSize = 12;
        protected object value = new object();
        protected object lastValue;
        private UnityEngine.Object objectValue;
        protected string name;
        protected Task task;

        GUIStyle inputFieldStyle;

        #endregion

        #region Properties

        public object Value { get => value; }

        #endregion

        public TaskFieldContent(IGraphObject parent, FieldInfo fieldInfo, Task task) : base(parent)
        {
            field = fieldInfo;
            name = field.Name;
            this.task = task;
            CropName();
            CreateStyle();
            InitializeValue();
            DefaultSize = new Vector2(200f, 20f);

            if (value is UnityEngine.Object)
            {
                inputFieldWidth = 150;
                DefaultSize = new Vector2(200f, 16f);
            }
        }

        public override void Draw()
        {
            UpdateRect();

            if (field != null)
            {
                GUI.Label(nameRect, name, style);
                DrawInputField();

                if (value != lastValue)
                {
                    lastValue = value;
                    ValueChanged();
                }
            }
        }

        public override bool ProcessEvent(Event e, SMEditorEventArgs eventArgs)
        {
            return false;
        }

        public override Vector2 GetRawSize()
        {
            var size = DefaultSize;

            if (field != null)
            {
                if (name.Length > 0)
                {
                    int width = 0;

                    foreach (var c in name)
                    {
                        CharacterInfo info;
                        style.font.GetCharacterInfo(c, out info, style.fontSize);
                        width += info.advance;
                    }

                    size = new Vector2(width + inputFieldWidth + 10, style.fontSize);

                    if (size.x < DefaultSize.x)
                    {
                        size.x = DefaultSize.x;
                    }

                    if (size.y < DefaultSize.y)
                    {
                        size.y = DefaultSize.y;
                    }
                }
            }

            return size;
        }

        protected override void UpdateRect()
        {
            base.UpdateRect();

            nameRect.height = rect.height;
            nameRect.width = rect.width - inputFieldWidth;
            valueRect.height = rect.height;
            valueRect.width = inputFieldWidth;

            nameRect.position = rect.position;
            valueRect.position = rect.position + new Vector2(rect.width - valueRect.width, 0f);
        }

        protected override void CreateStyle()
        {
            if (field != null)
            {
                style = new GUIStyle();
                style.fontSize = fontSize;
                style.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                style.clipping = TextClipping.Clip;
                style.alignment = TextAnchor.MiddleLeft;
                style.padding = new RectOffset(5, 5, 5, 5);

                if (field.FieldType != typeof(bool))
                {
                    inputFieldStyle = (GUIStyle)"TextFieldDropDownText";
                    style.fontSize = fontSize;
                    style.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                    style.clipping = TextClipping.Clip;
                    style.alignment = TextAnchor.MiddleLeft;
                    style.padding = new RectOffset(5, 5, 5, 5);
                }
                else
                {
                    inputFieldStyle = (GUIStyle)"OL Toggle";
                }
            }
        }

        protected void DrawInputField()
        {
            if (field.FieldType == typeof(int))
            {
                value = EditorGUI.IntField(valueRect, (int)value, inputFieldStyle);
            }
            else if (field.FieldType == typeof(float))
            {
                value = EditorGUI.FloatField(valueRect, (float)value, inputFieldStyle);
            }
            else if (field.FieldType == typeof(double))
            {
                value = EditorGUI.DoubleField(valueRect, (double)value, inputFieldStyle);
            }
            else if (field.FieldType == typeof(string))
            {
                value = EditorGUI.TextField(valueRect, (string)value, inputFieldStyle);
            }
            else if (field.FieldType == typeof(bool))
            {
                value = EditorGUI.Toggle(valueRect, (bool)value, inputFieldStyle);
            }
            else if (typeof(UnityEngine.Object).IsAssignableFrom(field.FieldType))
            {
                value = EditorGUI.ObjectField(valueRect, objectValue, field.FieldType, true);
                objectValue = value as UnityEngine.Object;
            }
        }

        protected void ValueChanged()
        {
            field.SetValue(task, value);
            task.UpdateInfo();
        }

        private void InitializeValue()
        {
            value = field.GetValue(task);

            if (value is UnityEngine.Object)
            {
                objectValue = value as UnityEngine.Object;
            }
        }

        private void CropName()
        {
            if (name.Length > 30)
            {
                name = name.Remove(30);
            }
        }
    }
}
