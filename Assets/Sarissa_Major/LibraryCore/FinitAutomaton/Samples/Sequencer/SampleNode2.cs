using Sarissa.FinitAutomaton.SequenceMachine;
using UnityEngine;

public class SampleNode2 : SarissaSequenceMachineNode
{
    public override void StartNode()
    {
        Debug.Log($"Started {nameof(SampleNode2)}");
    }

    public override void UpdateNode()
    {
        Debug.Log($"Updated {nameof(SampleNode2)} : id {_id}");
    }

    public override void EndNode()
    {
        Debug.Log($"Ended {nameof(SampleNode2)}");
    }
}
