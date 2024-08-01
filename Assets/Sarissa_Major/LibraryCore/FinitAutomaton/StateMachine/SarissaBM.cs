// 作成者 菅沼

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace Sarissa.FinitAutomaton
{
    /// <summary> ステートマシーンの機能を提供 </summary>
    public class SarissaBM
    {
        private Dictionary<string, int> _transitionNameAndIdDic = new();
        private HashSet<SarissaSMBehaviour> _smBehaviours = new();
        private HashSet<SarissaSMTransition> _smTransitions = new();
        private SarissaSMBehaviour _currentBehaviour, _yieldedBehaviourNow;
        private bool _isPausing;
        private bool _isYieldToEvent;

        public int CurrentBehaviourID
        {
            get { return _smBehaviours.ToList().IndexOf(_currentBehaviour); }
        }

        public int CurrentYieldedBehaviourID
        {
            get { return _smBehaviours.ToList().IndexOf(_yieldedBehaviourNow); }
        }

        public bool IsPaused
        {
            get { return _isPausing; }
        }

        public SarissaSMBehaviour CurrentBehaviour
        {
            get { return _currentBehaviour; }
        }

        public SarissaSMBehaviour CurrentYieldedEvent
        {
            get { return _yieldedBehaviourNow; }
        }

        public void ResistBehaviours(params SarissaSMBehaviour[] btBehaviours)
        {
            _smBehaviours = btBehaviours.ToHashSet();
            if (_currentBehaviour == null) _currentBehaviour = btBehaviours[0];
        }

        public void MakeTransition(SarissaSMBehaviour from, SarissaSMBehaviour to, string name)
        {
            if (!_transitionNameAndIdDic.ContainsKey(name))
            {
                _transitionNameAndIdDic.Add(name, _transitionNameAndIdDic.Count);
            }

            _smTransitions.Add(new SarissaSMTransition(from, to, _transitionNameAndIdDic[name]));
        }

        public void UpdateTransition(string name, ref bool condition, bool equalsTo = true, bool isTrigger = false)
        {
            if (_isPausing)
            {
                return;
            }

            if (_isYieldToEvent) return;

            foreach (var transition in _smTransitions)
            {
                if ((condition == equalsTo) && transition.Id == _transitionNameAndIdDic[name])
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

            if (_isYieldToEvent)
            {
                _yieldedBehaviourNow.Tick();
                if (!_yieldedBehaviourNow.YieldManually)
                {
                    _isYieldToEvent = false;
                }
            }
        }

        public void JumpTo(SarissaSMBehaviour behaviour)
        {
            if (_smBehaviours.Contains(behaviour))
            {
                _currentBehaviour = behaviour;
            }
        }

        public void YieldAllBehaviourTo(SarissaSMBehaviour behaviour)
        {
            if (_smBehaviours.Contains(behaviour))
            {
                _isYieldToEvent = true;
                _yieldedBehaviourNow = behaviour;
                _yieldedBehaviourNow.Begin();
            }
        }

        public void EndYieldBehaviourFrom(SarissaSMBehaviour behaviour)
        {
            if (_smBehaviours.Contains(behaviour))
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