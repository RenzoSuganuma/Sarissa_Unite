using Sarissa.FinitAutomaton.SequenceMachine;
using UnityEngine;

public class SampleNode1 : SarissaSequenceMachineNode
{
    public override void StartNode()
    {
        Debug.Log($"Started {nameof(SampleNode1)}");
    }

    public override void UpdateNode()
    {
        Debug.Log($"Updated {nameof(SampleNode1)} : id {_id}");
    }

    public override void EndNode()
    {
        Debug.Log($"Ended {nameof(SampleNode1)}");
    }
}
