using System;
using System.Collections;
using System.Collections.Generic;
using Sarissa.BehaviourTree;
using UnityEngine;
using Behaviour = Sarissa.BehaviourTree.Behaviour;

public class BTSampleAI : MonoBehaviour
{
    private BehaviourTree _BT = new BehaviourTree();
    private Behaviour _behaviourA;
    private Behaviour _behaviourB;
    private Action _stateA1, _stateA2, _stateA3;
    private Action _stateB1, _stateB2, _stateB3;
    [SerializeField] private bool cond;

    private void Awake()
    {
        #region MakeState

        _stateA1 = () => { Debug.Log($"sA1"); };

        _stateA2 = () => { Debug.Log($"sA2"); };

        _stateA3 = () => { Debug.Log($"sA3"); };

        _stateB1 = () => { Debug.Log($"sB1"); };

        _stateB2 = () => { Debug.Log($"sB2"); };

        _stateB3 = () => { Debug.Log($"sB3"); };

        #endregion

        _behaviourA = new(new Action[] { _stateA1, _stateA2, _stateA3 });
        _behaviourB = new(new Action[] { _stateB1, _stateB2, _stateB3 });

        _BT.ResistBehaviours(new[] { _behaviourA, _behaviourB });
        _BT.MakeTransition(_behaviourA, _behaviourB, 1);
        _BT.MakeTransition(_behaviourB, _behaviourA, 2);
        _BT.Start();
    }

    private void Update()
    {
        _BT.UpdateTransition(1, ref cond);
        _BT.UpdateTransition(2, ref cond, false);
    }
}
