namespace Sarissa.StateMachine
{
    // 各トランジションは名前を割り当てている
    /// <summary> ステート間遷移の情報を格納している </summary>
    public class StateMachineTransition
    {
        IStateMachineState _from;
        public IStateMachineState From => _from;
        IStateMachineState _to;
        public IStateMachineState To => _to;
        int _id;
        public int ID => _id;

        public StateMachineTransition(IStateMachineState from, IStateMachineState to, int id)
        {
            _from = from;
            _to = to;
            _id = id;
        }
    }
}
