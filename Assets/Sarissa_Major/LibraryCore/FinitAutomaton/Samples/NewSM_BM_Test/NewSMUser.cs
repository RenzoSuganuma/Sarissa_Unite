using Sarissa.FinitAutomaton;
using UnityEngine;
using UnityEngine.UI;

public class NewSMUser : MonoBehaviour
{
    private SarissaBM _sm = new SarissaBM();
    private MakeTextBlue _blue = new MakeTextBlue();
    private MakeTextRed _red = new MakeTextRed();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _blue.SetYieldMode(true);
        _red.SetYieldMode(true);
        
        _sm.ResistBehaviours(new SarissaSMBehaviour[] { _blue, _red });

        _sm.StartBT();
    }

    // Update is called once per frame
    void Update()
    {
        var b = Input.GetKey(KeyCode.B);
        var r = Input.GetKey(KeyCode.R);
        _sm.UpdateEventsYield();
        if (b)
        {
            _sm.EndYieldBehaviourFrom(_sm.CurrentYieldedEvent);
            _sm.YieldAllBehaviourTo(_blue);
        }

        if (r)
        {
            _sm.EndYieldBehaviourFrom(_sm.CurrentYieldedEvent);
            _sm.YieldAllBehaviourTo(_red);
        }
    }
}