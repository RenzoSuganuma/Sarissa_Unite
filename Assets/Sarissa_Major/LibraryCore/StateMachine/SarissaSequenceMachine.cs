// 作成者 菅沼

using System.Collections.Generic;
using System.Linq;
using System;

namespace Sarissa.SequenceMachine
{
    /// <summary> ステートマシンの機能を提供 </summary>
    public class SarissaSequenceMachine
    {
        // id <-> node
        private Dictionary<Int32, SarissaSequenceMachineNode> _nodes = new();

        // id <-> condition
        private Dictionary<Int32, Boolean> _transitions = new();

        // idでカレントノードを取得
        private Int32 _currentNodeId;

        // 一時停止していないときにはtrue
        private Boolean _isRunning;

        public void ResistNode<T>(ref T node)
        {
            Int32 id = _nodes.Count;
            if (node as SarissaSequenceMachineNode == null)
            {
                throw new UpCastFailedException($"{nameof(node)} failed cast to {nameof(SarissaSequenceMachineNode)}");
            }

            (node as SarissaSequenceMachineNode).Id = id;
            _nodes.Add(id, node as SarissaSequenceMachineNode);
        }

        public void ApplyTransition<T1, T2>(T1 node1, T2 node2)
        {
            if (node1 as SarissaSequenceMachineNode == null)
            {
                throw new UpCastFailedException($"{nameof(node1)} failed cast to {nameof(SarissaSequenceMachineNode)}");
            }

            if (node2 as SarissaSequenceMachineNode == null)
            {
                throw new UpCastFailedException($"{nameof(node2)} failed cast to {nameof(SarissaSequenceMachineNode)}");
            }

            (node1 as SarissaSequenceMachineNode).Next = (node2 as SarissaSequenceMachineNode);
            (node2 as SarissaSequenceMachineNode).Next = null; // node1 の 次をあくまでもここでは指定しているのでnode2の次のノードの値はNULLに初期化しておく
            _transitions.Add((node1 as SarissaSequenceMachineNode).Id, false);
        }

        public void UpdateTransition(Int32 nodeId, Boolean condition)
        {
            _transitions[nodeId] = condition;
        }

        public void SetCurrentNodeAs(Int32 id)
        {
            _currentNodeId = id;
        }
        
        public void SetCurrentNodeAs<T>(T node1)
        {
            if (node1 as SarissaSequenceMachineNode == null)
            {
                throw new UpCastFailedException($"{nameof(node1)} failed cast to {nameof(SarissaSequenceMachineNode)}");
            }

            _currentNodeId = (node1 as SarissaSequenceMachineNode).Id;
        }

        public void StartBT()
        {
            _isRunning = true;

            _currentNodeId = 0;
            _nodes[_currentNodeId].StartNode();
        }

        public void UpdateBT()
        {
            if (!_isRunning) return;

            _nodes[_currentNodeId].UpdateNode();

            foreach (var nodesKey in _nodes.Keys) // for each id
            {
                // 一番端っこのノードでない 【次のいくステートがある】、 トランジション可能 
                if ( /* 1 */_transitions.ContainsKey(nodesKey) && _transitions[nodesKey] /* 1 */
                                                               && /* 2 */_nodes[nodesKey].Next is not null &&
                                                               _nodes[_currentNodeId].Next is not null /* 2 */
                                                               && /* 3 */_nodes[_currentNodeId].Next.Id ==
                                                               _nodes[nodesKey].Next.Id) /* 3 */
                {
                    _nodes[_currentNodeId].EndNode();

                    _currentNodeId = _nodes[_currentNodeId].Next.Id;

                    _nodes[_currentNodeId].StartNode();
                }
            }
        }

        public void EndBT()
        {
            _isRunning = false;

            _nodes[_currentNodeId].EndNode();
        }
    }
}
