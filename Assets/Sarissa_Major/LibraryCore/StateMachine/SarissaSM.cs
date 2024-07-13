// 管理者 菅沼

using System.Collections.Generic;
using System;

namespace Sarissa.StateMachine
{
    /// <summary> ステートマシンの機能を提供する </summary>
    public class SarissaSM
    {
        // 通常ステート
        HashSet<IStateMachineState> _states = new HashSet<IStateMachineState>();

        // Anyステートからのステート
        HashSet<IStateMachineState> _statesFromAnyState = new HashSet<IStateMachineState>();

        // トランジション
        HashSet<StateMachineTransition> _transitions = new HashSet<StateMachineTransition>();

        // Anyからのトランジション
        HashSet<StateMachineTransition> _transitionsFromAny = new HashSet<StateMachineTransition>();

        // 現在突入しているステート
        IStateMachineState _currentPlayingStateMachineState;

        // 現在突入しているトランジション名
        string _currentTransitionName;

        // ステートマシンが一時停止中かのフラグ
        bool _isPausing = true;

        // デリゲート公開部
        public event Action<string> OnEntered;
        public event Action<string> OnUpdated;
        public event Action<string> OnExited;

        #region 登録処理

        /// <summary> ステートの登録 </summary>
        public void ResistState(IStateMachineState stateMachineState)
        {
            _states.Add(stateMachineState);
            if (_currentPlayingStateMachineState == null)
            {
                _currentPlayingStateMachineState = stateMachineState;
            }
        }

        /// <summary> Anyからのステートの登録 </summary>
        public void ResistStateFromAny(IStateMachineState stateMachineState)
        {
            _statesFromAnyState.Add(stateMachineState);
        }

        /// <summary> 複数のステートを引数に渡してすべての渡されたステートを登録 </summary>
        public void ResistStates(List<IStateMachineState> states)
        {
            foreach (IStateMachineState state in states)
            {
                _states.Add(state);
                if (_currentPlayingStateMachineState == null)
                {
                    _currentPlayingStateMachineState = state;
                }
            }
        }

        /// <summary> 複数のステートを引数に渡してすべての渡されたAnyからのステートを登録 </summary>
        public void ResistStatesFromAny(List<IStateMachineState> states)
        {
            foreach (IStateMachineState state in _statesFromAnyState)
            {
                _states.Add(state);
            }
        }

        /// <summary> ステート間の遷移の登録 </summary>
        public void MakeTransition(IStateMachineState from, IStateMachineState to, string name)
        {
            var tmp = new StateMachineTransition(from, to, name);
            _transitions.Add(tmp);
        }

        /// <summary> Anyステートからの遷移の登録 </summary>
        public void MakeTransitionFromAny(IStateMachineState to, string name)
        {
            var tmp = new StateMachineTransition(new DummyStateMachineStateClass(), to, name);
            _transitionsFromAny.Add(tmp);
        }

        #endregion

        #region 更新処理

        /// <summary> 任意のステート間遷移の遷移の状況を更新する。 </summary>
        public void UpdateTransition(string name, ref bool condition2transist, bool equalsTo = true,
            bool isTrigger = false)
        {
            if (_isPausing) return; // もし一時停止中なら更新処理はしない。
            foreach (var t in _transitions)
            {
                // 遷移する場合 // * 条件を満たしているなら前トランジションを無視してしまうのでその判定処理をはさむこと *
                // もし遷移条件を満たしていて遷移名が一致するなら
                if ((condition2transist == equalsTo) && t.Name == name)
                {
                    if (t.From == _currentPlayingStateMachineState) // 現在左ステートなら
                    {
                        _currentPlayingStateMachineState.Exit(); // 右ステートへの遷移条件を満たしたので抜ける
                        if (OnExited != null)
                        {
                            OnExited(_currentTransitionName);
                        }

                        if (isTrigger) condition2transist = !equalsTo; // IsTrigger が trueなら
                        _currentPlayingStateMachineState = t.To; // 現在のステートを右ステートに更新、遷移はそのまま
                        _currentPlayingStateMachineState.Entry(); // 現在のステートの初回起動処理を呼ぶ
                        if (OnEntered != null)
                        {
                            OnEntered(_currentTransitionName);
                        }

                        _currentTransitionName = name; // 現在の遷移ネームを更新
                    }
                }
                // 遷移の条件を満たしてはいないが、遷移ネームが一致（更新されていないなら）現在のステートの更新処理を呼ぶ
                else
                {
                    _currentPlayingStateMachineState.Update();
                    if (OnUpdated != null)
                    {
                        OnUpdated(_currentTransitionName);
                    }
                }
            } // 全遷移を検索。
        }

        /// <summary> ANYステートからの遷移の条件を更新 </summary>
        public void UpdateTransitionFromAnyState(string name, ref bool condition2transist, bool equalsTo = true,
            bool isTrigger = false)
        {
            if (_isPausing) return; // もし一時停止中なら更新処理はしない。
            foreach (var t in _transitionsFromAny)
            {
                // もし遷移条件を満たしていて遷移名が一致するなら
                if ((condition2transist == equalsTo) && t.Name == name)
                {
                    _currentPlayingStateMachineState.Exit(); // 右ステートへの遷移条件を満たしたので抜ける
                    if (OnExited != null)
                    {
                        OnExited(_currentTransitionName);
                    }

                    if (isTrigger) condition2transist = !equalsTo; // 遷移条件を初期化
                    _currentPlayingStateMachineState = t.To; // 現在のステートを右ステートに更新、遷移はそのまま
                    _currentPlayingStateMachineState.Entry(); // 現在のステートの初回起動処理を呼ぶ
                    if (OnEntered != null)
                    {
                        OnEntered(_currentTransitionName);
                    }

                    _currentTransitionName = name; // 現在の遷移ネームを更新
                }
                // 遷移の条件を満たしてはいないが、遷移ネームが一致（更新されていないなら）現在のステートの更新処理を呼ぶ
                else if (t.Name == name)
                {
                    _currentPlayingStateMachineState.Update();
                    if (OnUpdated != null)
                    {
                        OnUpdated(_currentTransitionName);
                    }
                }
            } // 全遷移を検索。
        }

        #endregion

        #region 起動処理

        /// <summary> ステートマシンを起動する </summary>
        public void Start()
        {
            _isPausing = false;
            _currentPlayingStateMachineState.Entry();
        }

        #endregion

        #region 一時停止処理

        /// <summary> ステートマシンの処理を一時停止 </summary>
        public void Pause()
        {
            _isPausing = true;
        }

        #endregion
    }

    // 各トランジションは名前を割り当てている
    /// <summary> ステート間遷移の情報を格納している </summary>
    public class StateMachineTransition
    {
        IStateMachineState _from;
        public IStateMachineState From => _from;
        IStateMachineState _to;
        public IStateMachineState To => _to;
        string _name;
        public string Name => _name;

        public StateMachineTransition(IStateMachineState from, IStateMachineState to, string name)
        {
            _from = from;
            _to = to;
            _name = name;
        }
    }

    /// <summary> ステートとして登録をするクラスが継承するべきインターフェース </summary>
    public interface IStateMachineState
    {
        public void Entry();
        public void Update();
        public void Exit();
    }

    /// <summary> ダミーのステートのクラス </summary>
    class DummyStateMachineStateClass : IStateMachineState
    {
        public void Entry()
        {
        }

        public void Exit()
        {
        }

        public void Update()
        {
        }
    }

    /// <summary> ステート遷移のタイプ </summary>
    enum StateMachineTransitionType
    {
        StandardState, // 通常 
        AnyState, // 一フレームのみ遷移 
    }

    #region ステートマシン、利用部構想

    // イニシャライズ処理
    // ステートマシンインスタンス化ステートの登録
    // トランジションの登録
    // ステートマシンの更新

    // 毎フレーム処理
    // トランジションの状態の更新

    #endregion
}