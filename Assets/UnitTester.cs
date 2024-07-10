using Sarissa.CodingFramework;
using Sarissa.BehaviourTree;
using System.Collections;
using UnityEngine;
using Sarissa;
using System;

public class UnitTester : Character
{
    private SarissaBT _bt;
    private SampleNode1 node1 = new();
    private SampleNode2 node2 = new();

    private void OnEnable()
    {
        _bt = new();

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

        if (GUILayout.Button("Goto Node1"))
        {
            _bt.SetCurrentNodeAs((node1 as SarissaBTNode).Id);
            _bt.UpdateTransition(0, false);
        }
    }

    protected override void BehaviourWhenDeath()
    {
    }
}