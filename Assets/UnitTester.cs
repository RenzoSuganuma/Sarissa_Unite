using Sarissa.CodingFramework;
using StateMachine;
using System.Collections;
using UnityEngine;
using Sarissa;
using System;

public class UnitTester : Character
{
    [SerializeReference, SubclassSelector] private SarissaBTNode _node;
    
    private SarissaMinimalSM _minimalSm;
    private SampleNode1 node1 = new();
    private SampleNode2 node2 = new();

    private void OnEnable()
    {
        _minimalSm = new();

        _minimalSm.ResistNode(ref node1);
        _minimalSm.ResistNode(ref node2);
        _minimalSm.ApplyTransition(node1, node2);
    }

    private void Start()
    {
        Debug.Log($"{_node}");
        
        _minimalSm.StartBT();
    }

    private void Update()
    {
        _minimalSm.UpdateBT();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Goto Next"))
        {
            _minimalSm.UpdateTransition(0, true);
        }

        if (GUILayout.Button("End"))
        {
            _minimalSm.EndBT();
        }

        if (GUILayout.Button("Goto Node1"))
        {
            _minimalSm.SetCurrentNodeAs((node1 as SarissaBTNode).Id);
            _minimalSm.UpdateTransition(0, false);
        }
    }

    protected override void BehaviourWhenDeath()
    {
    }
}