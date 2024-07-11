using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace SgLibUnite.BehaviourTree
{
    /// <summary> BTのビヘイビアのベースクラス </summary>
    public class SarissaBTBehaviour
    {
        private List<Action> _behaviours;
        private int _behaviourIndex;
        private bool _yieldBehaviourManually = false;

        private Action OnBegin;
        private Action OnTick;
        private Action OnEnd;

        public event Action EBegin
        {
            add { OnBegin += value; }
            remove { OnBegin -= value; }
        }

        public event Action ETick
        {
            add { OnTick += value; }
            remove { OnTick -= value; }
        }

        public event Action EEnd
        {
            add { OnEnd += value; }
            remove { OnEnd -= value; }
        }

        public int BehaviourIndex
        {
            get { return _behaviourIndex; }
        }

        public int BehaviourLength
        {
            get { return _behaviours.Count; }
        }

        public bool YieldManually
        {
            get { return _yieldBehaviourManually; }
        }

        public void AddBehaviour(Action behaviour)
        {
            _behaviours.Add(behaviour);
        }

        public void SetYieldMode(bool yieldManually)
        {
            _yieldBehaviourManually = yieldManually;
        }

        public SarissaBTBehaviour()
        {
            _behaviours = new();
            _behaviourIndex = 0;
        }

        public SarissaBTBehaviour(params Action[] behaviour)
        {
            _behaviours = new();
            _behaviourIndex = 0;
            var b = behaviour.ToList();
            _behaviours = b;
        }

        public void Begin()
        {
            if (OnBegin != null) OnBegin.Invoke();
            if (_behaviours.Count == 0)
            {
                throw new Exception("No State Added On This Behaviour");
            }
        }

        public void Tick()
        {
            if (OnTick != null) OnTick.Invoke();
            if (_behaviourIndex + 1 < _behaviours.Count)
            {
                _behaviourIndex++;
            }
            else
            {
                _behaviourIndex = 0;
            }

            _behaviours[_behaviourIndex].Invoke();
        }

        public void End()
        {
            if (OnEnd != null) OnEnd.Invoke();
        }
    }
}