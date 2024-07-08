using System;
using System.Collections;
using System.Collections.Generic;
using Sarissa.BehaviourTree;
using UnityEngine;

public class BTSampleAI : MonoBehaviour
{
    private BehaviourTree _BT = new BehaviourTree();
    private BTBehaviour _btBehaviourA;
    private BTBehaviour _btBehaviourB;
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

        _btBehaviourA = new(new Action[] { _stateA1, _stateA2, _stateA3 });
        _btBehaviourB = new(new Action[] { _stateB1, _stateB2, _stateB3 });

        _BT.ResistBehaviours(new[] { _btBehaviourA, _btBehaviourB });
        _BT.MakeTransition(_btBehaviourA, _btBehaviourB, 1);
        _BT.MakeTransition(_btBehaviourB, _btBehaviourA, 2);
        _BT.Start();
    }

    private void Update()
    {
        _BT.UpdateTransition(1, ref cond);
        _BT.UpdateTransition(2, ref cond, false);
    }
}
