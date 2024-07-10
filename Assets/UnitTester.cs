using Sarissa.CodingFramework;
using System.Collections;
using UnityEngine;
using Sarissa;
using System;
using Sarissa.BehaviourTree;

public class UnitTester : Character
{
    private SarissaBT _bt;

    private void OnEnable()
    {
        _bt = new();
        SampleNode1 node1 = new();
        SampleNode2 node2 = new();
        
        _bt.ResistNode(ref node1);
        _bt.ResistNode(ref node2);
        _bt.ApplyTransition(node1, node2);
    }

    private void Start()
    {
        _bt.StartBT();
    }

    private void Update()
    {
        _bt.UpdateBT();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Goto Next"))
        {
            _bt.UpdateTransition(0, true);
        }

        if (GUILayout.Button("End"))
        {
            _bt.EndBT();
        }
    }

    protected override void BehaviourWhenDeath()
    {
    }
}