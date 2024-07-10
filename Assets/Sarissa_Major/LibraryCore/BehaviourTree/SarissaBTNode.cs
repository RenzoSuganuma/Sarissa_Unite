using System;

namespace Sarissa.BehaviourTree
{
    public abstract class SarissaBTNode
    {
        protected SarissaBTNode _next;
        protected Int32 _id;

        public event Action<SarissaBTNode> OnNextNodeChanged;

        public SarissaBTNode Next
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

        public void SetupNode(int id, SarissaBTNode next)
        {
            _id = id;
            _next = next;
        }

        public abstract void StartNode();

        public abstract void UpdateNode();

        public abstract void EndNode();
    }
}