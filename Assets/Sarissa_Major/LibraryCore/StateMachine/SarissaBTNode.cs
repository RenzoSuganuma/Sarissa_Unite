using System;

namespace StateMachine
{
    [Serializable]
    public abstract class SarissaBTNode
    {
        protected SarissaBTNode _next;  // 次のノードがない場合にはNULLが格納される
        protected Int32 _id;

        public event Action<SarissaBTNode> OnNextNodeChanged;

        public SarissaBTNode Next   // 次のノードがない場合にはNULLが格納される
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