using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AI;

public class GuardState : IState
{
    private EnemyBrain brain;
    private AIAgression agression;
    private Vector3 startPosition;
    private float rotationSpeed;

    public GuardState(EnemyBrain brain, AIAgression agression, Vector3 startPosition)
    {
        this.brain = brain;
        this.agression = agression;
        this.startPosition = startPosition;
    }

    public void OnEnter()
    {
        brain.CharacterInfo.agent.SetDestination(startPosition);
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        
    }

    public void SetField(object obj, string fieldName, object value)
    {
        var type = obj.GetType();
        FieldInfo field = null;

        while (field == null && type != null)
        {
            field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            type = type.BaseType;
        }

        if (field != null)
        {
            field.SetValue(obj, value);
        }
        else
        {
            Debug.LogError($"Object: {obj} does not have a field named {fieldName}");
        }
    }
    public void GetField(object obj, string fieldName, object value)
    {
        var type = obj.GetType();
        var field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        if (field != null)
        {
            field.SetValue(value, obj);
        }
    }
}
