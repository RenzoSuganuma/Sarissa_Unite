using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace Sarissa.FinitAutomaton
{
    /// <summary> 遷移の情報を格納している </summary>
    public class SarissaSMTransition
    {
        private SarissaSMBehaviour _from;
        private SarissaSMBehaviour _to;
        private int _id;
        public SarissaSMBehaviour From => _from;
        public SarissaSMBehaviour To => _to;
        public int Id => _id;

        public SarissaSMTransition(SarissaSMBehaviour from, SarissaSMBehaviour to, int id)
        {
            _from = from;
            _to = to;
            _id = id;
        }
    }
}
