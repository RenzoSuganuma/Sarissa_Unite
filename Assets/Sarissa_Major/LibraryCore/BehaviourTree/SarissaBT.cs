// 作成者 菅沼

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace SgLibUnite.BehaviourTree
{
    /// <summary> ビヘイビアツリーの機能を提供 </summary>
    public class SarissaBT
    {
        private HashSet<SarissaBTBehaviour> _btBehaviours = new();
        private HashSet<SarissaBTTransition> _btTransitions = new();
        private SarissaBTBehaviour _currentBehaviour, _yieldedBehaviourNow;
        private bool _isPausing;
        private bool _isYieldToEvent;

        public int CurrentBehaviourID
        {
            get { return _btBehaviours.ToList().IndexOf(_currentBehaviour); }
        }

        public int CurrentYieldedBehaviourID
        {
            get { return _btBehaviours.ToList().IndexOf(_yieldedBehaviourNow); }
        }

        public bool IsPaused
        {
            get { return _isPausing; }
        }

        public SarissaBTBehaviour CurrentBehaviour
        {
            get { return _currentBehaviour; }
        }

        public SarissaBTBehaviour CurrentYieldedEvent
        {
            get { return _yieldedBehaviourNow; }
        }

        public void ResistBehaviours(params SarissaBTBehaviour[] btBehaviours)
        {
            _btBehaviours = btBehaviours.ToHashSet();
            if (_currentBehaviour == null) _currentBehaviour = btBehaviours[0];
        }

        public void MakeTransition(SarissaBTBehaviour from, SarissaBTBehaviour to, string name)
        {
            _btTransitions.Add(new SarissaBTTransition(from, to, name));
        }

        public void UpdateTransition(string name, ref bool condition, bool equalsTo = true, bool isTrigger = false)
        {
            if (_isPausing)
            {
                return;
            }

            if (_isYieldToEvent) return;

            foreach (var transition in _btTransitions)
            {
                if ((condition == equalsTo) && transition.Name == name)
                {
                    if (transition.From == _currentBehaviour)
                    {
                        // このビヘイビアの先のビヘイビアへ遷移していないことが担保されてから遷移処理をするべき
                        _currentBehaviour.End();
                        if (isTrigger) condition = !equalsTo;
                        _currentBehaviour = transition.To;
                        _currentBehaviour.Begin();
                    }
                }
                else
                {
                    _currentBehaviour.Tick();
                }
            }
        }

        public void UpdateEventsYield()
        {
            if (_isPausing)
            {
                return;
            }
            else if (_isYieldToEvent)
            {
                _yieldedBehaviourNow.Tick();
                if (!_yieldedBehaviourNow.YieldManually)
                {
                    _isYieldToEvent = false;
                }
            }
        }

        public void JumpTo(SarissaBTBehaviour behaviour)
        {
            if (_btBehaviours.Contains(behaviour))
            {
                _currentBehaviour = behaviour;
            }
        }

        public void YieldAllBehaviourTo(SarissaBTBehaviour behaviour)
        {
            if (_btBehaviours.Contains(behaviour))
            {
                _isYieldToEvent = true;
                _yieldedBehaviourNow = behaviour;
                _yieldedBehaviourNow.Begin();
            }
        }

        public void EndYieldBehaviourFrom(SarissaBTBehaviour behaviour)
        {
            if (_btBehaviours.Contains(behaviour))
            {
                _isYieldToEvent = false;
                _yieldedBehaviourNow.End();
            }
        }

        public void PauseBT()
        {
            _isPausing = true;
        }

        public void StartBT()
        {
            _isPausing = false;
            _currentBehaviour.Begin();
        }
    }
}
