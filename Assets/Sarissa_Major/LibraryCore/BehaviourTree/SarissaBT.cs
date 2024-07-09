// 作成者 菅沼

using System.Collections.Generic;
using System.Linq;
using System;

namespace Sarissa.BehaviourTree
{
    /// <summary> ビヘイビアツリーの機能を提供 </summary>
    public class SarissaBT
    {
        // id <-> node
        private Dictionary<Int32, SarissaBTNode> _nodes = new();

        // id <-> condition
        private Dictionary<Int32, Boolean> _transitions = new();

        // idでカレントノードを取得
        private Int32 _currentNodeId;

        public void ResistNode(SarissaBTNode node1, SarissaBTNode node1Next)
        {
            Int32 id = _nodes.Count;
            node1.SetupNode(id, node1Next);
            _nodes.Add(id, node1);
        }

        public void UpdateTransition(Int32 nodeId, Boolean condition)
        {
            _transitions[nodeId] = condition;
        }

        public void UpdateBT()
        {
            foreach (var nodesKey in _nodes.Keys) // for each id
            {
                // id = 0 ---> 1 : OK の時
                // nodesKey = 0
                // _currentNodeId = 0
                // nodesKey,_currentNodeIdともに次のノードが一致しているとき
                if (_transitions[nodesKey]
                    && _nodes[_currentNodeId].Next.Id == _nodes[nodesKey].Next.Id)
                {
                    _nodes[_currentNodeId].EndNode();

                    _currentNodeId = _nodes[_currentNodeId].Next.Id;

                    _nodes[_currentNodeId].StartNode();
                }
            }

            _nodes[_currentNodeId].UpdateNode();
        }
    }
}