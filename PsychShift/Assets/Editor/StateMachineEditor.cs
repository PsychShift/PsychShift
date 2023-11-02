using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;

public abstract class Node
{
    public Rect rect;
    public string title;
    public List<Node> transitions;

    public Node(Rect rect, string title)
    {
        this.rect = rect;
        this.title = title;
        this.transitions = new List<Node>();
    }
}

public class StateNode : Node
{
    public MonoScript script;

    public StateNode(Rect rect, string title, MonoScript script)
        : base(rect, title)
    {
        this.script = script;
    }
}

public class TransitionNode : Node
{
    public List<Func<bool>> conditions;

    public TransitionNode(Rect rect, string title, List<Func<bool>> conditions)
        : base(rect, title)
    {
        this.conditions = conditions;
    }
}

public class StateMachineEditor : EditorWindow
{
    private List<Node> nodes;
    private Node selectedNode;
    private Vector3 selectedHandle;

    [MenuItem("Window/State Machine Editor")]
    public static void ShowWindow()
    {
        GetWindow<StateMachineEditor>("State Machine Editor");
    }

    private void OnEnable()
    {
        nodes = new List<Node>();
    }

    private void OnGUI()
    {
        // Draw nodes
        BeginWindows();
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].rect = GUILayout.Window(i, nodes[i].rect, DrawNodeWindow, nodes[i].title);
        }
        EndWindows();

        // Draw transitions
        foreach (var node in nodes)
        {
            foreach (var transition in node.transitions)
            {
                DrawNodeCurve(node.rect, transition.rect);
            }
        }
        

        // Draw "Create Node" button
        if (GUILayout.Button("Create State Node"))
        {
            nodes.Add(new StateNode(new Rect(10, 10, 100, 100), "State " + (nodes.Count + 1), null));
        }
        if (GUILayout.Button("Create Transition Node"))
        {
            nodes.Add(new TransitionNode(new Rect(10, 10, 100, 100), "Transition " + (nodes.Count + 1), null));
        }

    }

    void DrawNodeWindow(int id)
    {
        
        if (nodes[id] is StateNode stateNode)
        {
            stateNode.title = EditorGUILayout.TextField("Title", stateNode.title);
            stateNode.script = (MonoScript)EditorGUILayout.ObjectField("State Script", stateNode.script, typeof(MonoScript), false);
        }
        else if (nodes[id] is TransitionNode transitionNode)
        {
            transitionNode.title = EditorGUILayout.TextField("Title", transitionNode.title);
            // Draw fields for transition conditions here
        }

        if (GUILayout.Button("Select Node"))
        {
            selectedNode = nodes[id];
        }

        if (selectedNode != null && selectedNode != nodes[id] && GUILayout.Button("Add Transition"))
        {
            selectedNode.transitions.Add(nodes[id]);
            selectedNode = null;
        }
        DrawHandle(new Vector3(nodes[id].rect.xMin, nodes[id].rect.center.y, 0), id);
        DrawHandle(new Vector3(nodes[id].rect.xMax, nodes[id].rect.center.y, 0), id);
        DrawHandle(new Vector3(nodes[id].rect.center.x, nodes[id].rect.yMin, 0), id);
        DrawHandle(new Vector3(nodes[id].rect.center.x, nodes[id].rect.yMax, 0), id);

        GUI.DragWindow();
    }

    void DrawHandle(Vector3 position, int nodeId)
    {
        if (Handles.Button(position, Quaternion.identity, 10, 10, Handles.CircleHandleCap))
        {
            selectedNode = nodes[nodeId];
            selectedHandle = position;
        }
    }

    void DrawNodeCurve(Rect start, Rect end)
    {
        Vector3 startPos;
        Vector3 endPos;

        if (start.center.x < end.center.x)
        {
            startPos = new Vector3(start.xMax, start.center.y, 0);
            endPos = new Vector3(end.xMin, end.center.y, 0);
        }
        else
        {
            startPos = new Vector3(start.xMin, start.center.y, 0);
            endPos = new Vector3(end.xMax, end.center.y, 0);
        }

        if (start.center.y < end.center.y)
        {
            startPos = new Vector3(start.center.x, start.yMax, 0);
            endPos = new Vector3(end.center.x, end.yMin, 0);
        }
        else
        {
            startPos = new Vector3(start.center.x, start.yMin, 0);
            endPos = new Vector3(end.center.x, end.yMax, 0);
        }

        Handles.DrawLine(startPos, endPos);
    }
}