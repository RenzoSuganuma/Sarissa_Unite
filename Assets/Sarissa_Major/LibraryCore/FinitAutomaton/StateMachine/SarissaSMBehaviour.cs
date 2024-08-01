using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace Sarissa.FinitAutomaton
{
    /// <summary> BTのビヘイビアのベースクラス </summary>
    public class SarissaSMBehaviour
    {
        private bool _yieldBehaviourManually = false;

        public bool YieldManually
        {
            get { return _yieldBehaviourManually; }
        }

        public void SetYieldMode(bool yieldManually)
        {
            _yieldBehaviourManually = yieldManually;
        }

        public virtual void Begin()
        {
        }

        public virtual void Tick()
        {
        }

        public virtual void End()
        {
        }
    }
}