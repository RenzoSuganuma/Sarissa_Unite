using Sarissa.FinitAutomaton;
using UnityEngine;
using UnityEngine.UI;

public class MakeTextBlue : SarissaSMBehaviour
{
    private int cnt;
    public override void Begin()
    {
        Debug.Log("<color=blue>Blue begin</color>");
    }

    public override void Tick()
    {
        cnt++;
        var v = GameObject.FindFirstObjectByType<Text>(FindObjectsInactive.Include);
        v.text = $" BLUUUUUUUUUE!{cnt} ";
        v.color = Color.blue;
    }

    public override void End()
    {
        Debug.Log("<color=blue>blue end</color>");
    }
}