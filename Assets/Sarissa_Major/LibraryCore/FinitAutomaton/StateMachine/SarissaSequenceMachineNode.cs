using System;

namespace Sarissa.FinitAutomaton.SequenceMachine
{
    [Serializable]
    public abstract class SarissaSequenceMachineNode
    {
        protected SarissaSequenceMachineNode _next;  // 次のノードがない場合にはNULLが格納される
        protected Int32 _id;

        public event Action<SarissaSequenceMachineNode> OnNextNodeChanged;

        public SarissaSequenceMachineNode Next   // 次のノードがない場合にはNULLが格納される
        {
            get { return _next; }

            set
            {
                _next = value;

                if (OnNextNodeChanged is not null)
                {
                    OnNextNodeChanged(_next);
                }
            }
        }

        public int Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        public abstract void StartNode();

        public abstract void UpdateNode();

        public abstract void EndNode();
    }
}