using System.Collections;
using System.Reflection;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using Minima.StateMachine;

public class TaskView : ColumnContent
{
    #region Fields

    private Type taskType;
    private FieldInfo[] fields;

    #endregion

    #region Properties
    
    #endregion

    public TaskView(IGraphObject parent, Type taskType) : base(parent)
    {
        if (typeof(Task).IsAssignableFrom(taskType))
        {
            this.taskType = taskType;
            Construct();
        }
        else
        {
            Debug.LogError(taskType.ToString() + " is not a Task");
        }
    }

    private void Construct()
    {
        var title = new TextContent(this, taskType.Name);
        AddContent(title);

        fields = taskType.GetFields()
            .Where(f => f.GetCustomAttribute<NodeEditable>() != null)
            .ToArray();

        foreach (var f in fields)
        {
            AddFieldContent(f);
        }
    }

    private void AddFieldContent(FieldInfo fieldInfo)
    {
        if (fieldInfo.FieldType == typeof(bool))
        {
            var boolean = new BoolFieldContent(this, fieldInfo);
            AddContent(boolean);
        }
        else if (fieldInfo.FieldType == typeof(int))
        {
            AddInputField(fieldInfo);
        }
        else if (fieldInfo.FieldType == typeof(float))
        {
            AddInputField(fieldInfo);
        }
        else if (fieldInfo.FieldType == typeof(string))
        {
            AddInputField(fieldInfo);
        }
    }

    private void AddInputField(FieldInfo fieldInfo)
    {
        var inputField = new InputFieldContent(this, fieldInfo);
        AddContent(inputField);
    }
}
