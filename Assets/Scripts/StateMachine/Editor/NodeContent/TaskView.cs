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
            var field = new TaskFieldContent(this, f);
            AddContent(field);
        }
    }

}
