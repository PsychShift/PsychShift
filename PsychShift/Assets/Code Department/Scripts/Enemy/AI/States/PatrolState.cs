using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PatrolState : IState
{
    private EnemyBrain brain;
    private AIAgression agression;
    private Player.CharacterInfo currentCharacterInfo;
    private List<Vector3> patrolPoints;
    private int wpIndex = 0;

    public PatrolState(EnemyBrain brain, AIAgression agression, List<Vector3> patrolPoints)
    {
        this.brain = brain;
        this.agression = agression;
        this.patrolPoints = patrolPoints;
    }

    public void OnEnter()
    {
        wpIndex = brain as BasicEnemy != null ? (brain as BasicEnemy).CurrentPatrolPointIndex : 0;
        currentCharacterInfo = brain.CharacterInfo;
        currentCharacterInfo.agent.SetDestination(patrolPoints[wpIndex]);
        currentCharacterInfo.agent.enabled = false;
    }

    public void OnExit()
    {
        if (brain as BasicEnemy != null) (brain as BasicEnemy).CurrentPatrolPointIndex = wpIndex;
        
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

    public Color GizmoColor()
    {
        return Color.white;
    }
}

