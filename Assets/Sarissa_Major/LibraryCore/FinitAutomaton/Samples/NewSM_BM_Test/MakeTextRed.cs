using Sarissa.FinitAutomaton;
using UnityEngine;
using UnityEngine.UI;

public class MakeTextRed : SarissaSMBehaviour
{
    private int cnt;
    public override void Begin()
    {
        Debug.Log("<color=red>Red begin</color>");
    }

    public override void Tick()
    {
        cnt++;
        var v = GameObject.FindFirstObjectByType<Text>(FindObjectsInactive.Include);
        v.text = $" REEEEEEEEEED! {cnt} ";
        v.color = Color.red;
    }

    public override void End()
    {
        Debug.Log("<color=red>Red end</color>");
    }
}