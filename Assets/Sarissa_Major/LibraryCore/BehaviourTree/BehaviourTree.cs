// 作成者 菅沼

using System.Collections.Generic;
using System.Linq;
using System;

namespace Sarissa.BehaviourTree
{
    /// <summary> BTのビヘイビアのベースクラス </summary>
    public class BTBehaviour
    {
        private List<Action> _behaviours;
        private int _behaviourIndex;
        private bool _yieldBehaviourManually;

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

        public BTBehaviour()
        {
            _behaviours = new();
            _behaviourIndex = 0;
        }

        public BTBehaviour(params Action[] behaviour)
        {
            _behaviours = new();
            _behaviourIndex = 0;
            var b = behaviour.ToList();
            _behaviours = b;
        }

        public void Begin()
        {
            if (OnBegin is not null)
            {
                OnBegin.Invoke();
            }

            if (_behaviours.Count == 0)
            {
                throw new Exception("No State Added On This Behaviour");
            }
        }

        public void Tick()
        {
            if (OnTick is not null)
            {
                OnTick.Invoke();
            }

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
            if (OnEnd is not null)
            {
                OnEnd.Invoke();
            }
        }
    }

    /// <summary> 遷移の情報を格納している </summary>
    public class BTTransition
    {
        private BTBehaviour _from;
        private BTBehaviour _to;
        private int _id;
        public BTBehaviour From => _from;
        public BTBehaviour To => _to;
        public int Id => _id;

        public BTTransition(BTBehaviour from, BTBehaviour to, int id)
        {
            _from = from;
            _to = to;
            _id = id;
        }
    }

    /// <summary> ビヘイビアツリーの機能を提供 </summary>
    public class BehaviourTree
    {
        private HashSet<BTBehaviour> _behaviours = new();
        private HashSet<BTTransition> _transitions = new();
        private BTBehaviour _currentBtBehaviour, _yieldedBtBehaviourNow;
        private bool _isPausing;
        private bool _isYieldToEvent;

        public int CurrentBehaviourID
        {
            get { return _behaviours.ToList().IndexOf(_currentBtBehaviour); }
        }

        public int CurrentYieldedBehaviourID
        {
            get { return _behaviours.ToList().IndexOf(_yieldedBtBehaviourNow); }
        }

        public bool IsPaused
        {
            get { return _isPausing; }
        }

        public BTBehaviour CurrentBtBehaviour
        {
            get { return _currentBtBehaviour; }
        }

        public BTBehaviour CurrentYieldedBtEvent
        {
            get { return _yieldedBtBehaviourNow; }
        }

        public void ResistBehaviours(params BTBehaviour[] btBehaviours)
        {
            _behaviours = btBehaviours.ToHashSet();
            if (_currentBtBehaviour == null) _currentBtBehaviour = btBehaviours[0];
        }

        public void MakeTransition(BTBehaviour from, BTBehaviour to, int id)
        {
            _transitions.Add(new BTTransition(from, to, id));
        }

        public void UpdateTransition(int id, ref bool condition, bool equalsTo = true, bool isTrigger = false)
        {
            if (_isPausing) return;

            if (_isYieldToEvent) return;

            foreach (var transition in _transitions)
            {
                if ((condition == equalsTo) && transition.Id == id)
                {
                    if (transition.From == _currentBtBehaviour)
                    {
                        // このビヘイビアの先のビヘイビアへ遷移していないことが担保されてから遷移処理をするべき
                        _currentBtBehaviour.End();
                        if (isTrigger) condition = !equalsTo;
                        _currentBtBehaviour = transition.To;
                        _currentBtBehaviour.Begin();
                    }
                }
                else
                {
                    _currentBtBehaviour.Tick();
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
                _yieldedBtBehaviourNow.Tick();
                if (!_yieldedBtBehaviourNow.YieldManually)
                {
                    _isYieldToEvent = false;
                }
            }
        }

        public void JumpTo(BTBehaviour btBehaviour)
        {
            if (_behaviours.Contains(btBehaviour))
            {
                _currentBtBehaviour = btBehaviour;
            }
        }

        public void YieldAllBehaviourTo(BTBehaviour btBehaviour)
        {
            if (_behaviours.Contains(btBehaviour))
            {
                _isYieldToEvent = true;
                _yieldedBtBehaviourNow = btBehaviour;
                _yieldedBtBehaviourNow.Begin();
            }
        }

        public void EndYieldBehaviourFrom(BTBehaviour btBehaviour)
        {
            if (_behaviours.Contains(btBehaviour))
            {
                _isYieldToEvent = false;
                _yieldedBtBehaviourNow.End();
            }
        }

        public void Pause()
        {
            _isPausing = true;
        }

        public void Start()
        {
            _isPausing = false;
            _currentBtBehaviour.Begin();
        }
    }
}