﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
using Newtonsoft.Json;
using System.IO;
using System;


[System.Serializable]
public class DialogueNode : IComparable<DialogueNode>
{
    private int _NodeID;
    public int TreeID;

    int FlagToSet = 0;
    int RequiredFlag = 0;
    int SetSpeaker = 0;

    string JsonData;

    [System.NonSerialized]
    public Rect Node;

    string FilePath = "Assets/Speakers.json";

    List<Speaker> SpeakerSelection;
    List<String> SpeakerName;

    Speaker SelectedSpeaker;

    NodeType NType;

    private Vector2 _NodeSize;

    public float PosX, PosY;

    private string _NodeTitle;

    public int NpcId;
    public int Quest;

    [System.NonSerialized]
    private GUIStyle _NodeStyle;

    private bool IsDragged;

    public DialogueMessage DNode;
    public ConnectionPoints LeftPoint;
    public ConnectionPoints RightPoint;

    [System.NonSerialized]
    public Action<DialogueNode> OnRemoveNode;

    public void CreateNode(string NodeTitle, Vector2 Position, float SizeW, float SizeH, GUIStyle NodeStyle,
    GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoints> OnClickInPoint,
    Action<ConnectionPoints> OnClickOutPoint, Action<DialogueNode> RemoveNode, ref int ID, NodeType Type)
    {
        _NodeStyle = NodeStyle;
        _NodeTitle = NodeTitle;

        _NodeID = ID;

        DNode = new DialogueMessage();
        DNode.ID = _NodeID;

        DNode.NodeT = Type;

        if (File.Exists(FilePath))
        {
            SpeakerName = new List<string>();
            string temp = File.ReadAllText(FilePath);
            SpeakerSelection = JsonConvert.DeserializeObject<List<Speaker>>(temp);

            for (int i = 0; i < SpeakerSelection.Count; i++)
            {
                SpeakerName.Add(SpeakerSelection[i].SpeakerName);
            }
        }

        LeftPoint = new ConnectionPoints(this, Link.LEFT, inPointStyle, OnClickInPoint);
        RightPoint = new ConnectionPoints(this, Link.RIGHT, outPointStyle, OnClickOutPoint);

        OnRemoveNode = RemoveNode;

        Node = new Rect(Position.x, Position.y, SizeW, SizeH);
        ID += 1;
        return;
    }


    public void CreateNode(string NodeTitle, Vector2 Position, float SizeW, float SizeH, GUIStyle NodeStyle,
        GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoints> OnClickInPoint,
        Action<ConnectionPoints> OnClickOutPoint, Action<DialogueNode> RemoveNode, ref int ID, DialogueMessage MNode, NodeType Type)
    {
        _NodeStyle = NodeStyle;
        _NodeTitle = NodeTitle;

        _NodeID = ID;

        DNode = new DialogueMessage();
        DNode = MNode;

        DNode.NodeT = Type;

        if (Type == NodeType.DIALOUGE)
        {

            if (File.Exists(FilePath))
            {
                JsonData = File.ReadAllText(FilePath);
                SpeakerSelection = JsonConvert.DeserializeObject<List<Speaker>>(JsonData);

                SpeakerName = new List<string>();

                for (int i = 0; i < SpeakerSelection.Count; i++)
                {
                    SpeakerName.Add(SpeakerSelection[i].SpeakerName);
                }
            }

        }

        PosX = Position.x;
        PosY = Position.y;

        LeftPoint = new ConnectionPoints(this, Link.LEFT, inPointStyle, OnClickInPoint);
        RightPoint = new ConnectionPoints(this, Link.RIGHT, outPointStyle, OnClickOutPoint);

        OnRemoveNode = RemoveNode;

        Node = new Rect(Position.x, Position.y, SizeW, SizeH);
        ID += 1;
        return;
    }


    public void Drag(Vector2 Drag)
    {

        PosX = Drag.x;
        PosY = Drag.y;


        // Set position based on Mouse Cordinites
        Node.position += Drag;
    }

    public void Draw(string[] Flag, List<Flags> FlagData)
    {
        LeftPoint.Draw();
        RightPoint.Draw();

        GUI.Box(Node, _NodeTitle, _NodeStyle);

        GUILayout.BeginArea(new Rect(Node.x, Node.y, 250, 450));
        GUILayout.BeginVertical();


        switch (DNode.NodeT)
        {

            case NodeType.FLAG:
                EditorGUILayout.LabelField("Flag: ");
                RequiredFlag = EditorGUILayout.Popup(RequiredFlag, Flag);
                break;

            case NodeType.DIALOUGE:
                EditorGUILayout.LabelField("Speaker ID: ");
                SetSpeaker = EditorGUILayout.Popup(SetSpeaker, SpeakerName.ToArray());
                DNode.SpeakerID = SpeakerSelection[SetSpeaker];
                DNode.Name = GUILayout.TextField(DNode.Name);
                DNode.Line = GUILayout.TextArea(DNode.Line, GUILayout.Height(EditorGUIUtility.singleLineHeight * 5));
                break;

            case NodeType.CHOICE:
                // List Options and flags they set

                DNode.Choices = new string[2];
                EditorGUILayout.LabelField("Options");
                DNode.Choices[0] = EditorGUILayout.TextField(DNode.Choices[0]);
                DNode.Choices[1] = EditorGUILayout.TextField(DNode.Choices[1]);

                break;

        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    private void OnClickRemoveNode()
    {
        if (OnRemoveNode != null)
        {
            OnRemoveNode(this);
        }
    }

    private void ProcessContextMenu()
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
        genericMenu.ShowAsContext();
    }

    public bool ProcessNodeEvents(Event e)
    {

        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0) // left clicky
                {
                    if (Node.Contains(e.mousePosition))
                    {
                        IsDragged = true;
                        GUI.changed = true;
                    }
                }
                break;

            case EventType.MouseUp:
                IsDragged = false;
                break;

            case EventType.MouseDrag:
                if (e.button == 0 && IsDragged)
                {
                    Drag(e.delta);
                    e.Use();
                    return true;
                }
                break;
        }

        if (e.button == 1 && Node.Contains(e.mousePosition))
        {
            ProcessContextMenu();
            e.Use();
        }

        return false;
    }

    public int CompareTo(DialogueNode obj)
    {
        if (this._NodeID < obj._NodeID)
        {
            return -1;
        }

        if (_NodeID > obj._NodeID)
        {
            return 1;
        }

        return 0;
    }



}
