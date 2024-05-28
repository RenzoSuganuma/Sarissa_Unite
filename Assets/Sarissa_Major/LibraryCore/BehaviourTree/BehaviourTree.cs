// 作成者 菅沼

using System.Collections.Generic;
using System;
using System.Linq;

namespace Sarissa.BehaviourTree
{
    /// <summary> BTのビヘイビアのベースクラス </summary>
    public class Behaviour
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

        public Behaviour()
        {
            _behaviours = new();
            _behaviourIndex = 0;
        }

        public Behaviour(params Action[] behaviour)
        {
            _behaviours = new();
            _behaviourIndex = 0;
            var b = behaviour.ToList();
            _behaviours = b;
        }

        public void Begin()
        {
            OnBegin?.Invoke();
            if (_behaviours.Count == 0)
            {
                throw new Exception("No State Added On This Behaviour");
            }
        }

        public void Tick()
        {
            OnTick?.Invoke();
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
            OnEnd?.Invoke();
        }
    }

    /// <summary> 遷移の情報を格納している </summary>
    public class Transition
    {
        private Behaviour _from;
        public Behaviour From => _from;
        private Behaviour _to;
        public Behaviour To => _to;
        private int _id;
        public int Id => _id;

        public Transition(Behaviour from, Behaviour to, int id)
        {
            _from = from;
            _to = to;
            _id = id;
        }
    }

    /// <summary> ビヘイビアツリーの機能を提供 </summary>
    public class BehaviourTree
    {
        private HashSet<Behaviour> _behaviours = new();
        private HashSet<Transition> _transitions = new();
        private Behaviour _currentBehaviour, _yieldedBehaviourNow;
        private bool _isPausing;
        private bool _isYieldToEvent;

        public int CurrentBehaviourID
        {
            get { return _behaviours.ToList().IndexOf(_currentBehaviour); }
        }

        public int CurrentYieldedBehaviourID
        {
            get { return _behaviours.ToList().IndexOf(_yieldedBehaviourNow); }
        }

        public bool IsPaused
        {
            get { return _isPausing; }
        }

        public Behaviour CurrentBehaviour
        {
            get { return _currentBehaviour; }
        }

        public Behaviour CurrentYieldedEvent
        {
            get { return _yieldedBehaviourNow; }
        }

        public void ResistBehaviours(params Behaviour[] btBehaviours)
        {
            _behaviours = btBehaviours.ToHashSet();
            if (_currentBehaviour == null) _currentBehaviour = btBehaviours[0];
        }

        public void MakeTransition(Behaviour from, Behaviour to, int id)
        {
            _transitions.Add(new Transition(from, to, id));
        }

        public void UpdateTransition(int id, ref bool condition, bool equalsTo = true, bool isTrigger = false)
        {
            if (_isPausing) return;

            if (_isYieldToEvent) return;

            foreach (var transition in _transitions)
            {
                if ((condition == equalsTo) && transition.Id == id)
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
            if (_isYieldToEvent)
            {
                _yieldedBehaviourNow.Tick();
                if (!_yieldedBehaviourNow.YieldManually)
                {
                    _isYieldToEvent = false;
                }
            }
        }

        public void JumpTo(Behaviour behaviour)
        {
            if (_behaviours.Contains(behaviour))
            {
                _currentBehaviour = behaviour;
            }
        }

        public void YieldAllBehaviourTo(Behaviour behaviour)
        {
            if (_behaviours.Contains(behaviour))
            {
                _isYieldToEvent = true;
                _yieldedBehaviourNow = behaviour;
                _yieldedBehaviourNow.Begin();
            }
        }

        public void EndYieldBehaviourFrom(Behaviour behaviour)
        {
            if (_behaviours.Contains(behaviour))
            {
                _isYieldToEvent = false;
                _yieldedBehaviourNow.End();
            }
        }

        public void Pause()
        {
            _isPausing = true;
        }

        public void Start()
        {
            _isPausing = false;
            _currentBehaviour.Begin();
        }
    }
}