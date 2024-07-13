namespace Sarissa.StateMachine
{
    /// <summary> ステートとして登録をするクラスが継承するべきインターフェース </summary>
    public interface IStateMachineState
    {
        public void Entry();
        public void Update();
        public void Exit();
    }
}