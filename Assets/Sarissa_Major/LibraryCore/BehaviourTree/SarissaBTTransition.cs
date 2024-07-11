using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace SgLibUnite.BehaviourTree
{
    /// <summary> 遷移の情報を格納している </summary>
    public class SarissaBTTransition
    {
        private SarissaBTBehaviour _from;
        private SarissaBTBehaviour _to;
        private string _name;
        public SarissaBTBehaviour From => _from;
        public SarissaBTBehaviour To => _to;
        public string Name => _name;

        public SarissaBTTransition(SarissaBTBehaviour from, SarissaBTBehaviour to, string name)
        {
            _from = from;
            _to = to;
            _name = name;
        }
    }
}
