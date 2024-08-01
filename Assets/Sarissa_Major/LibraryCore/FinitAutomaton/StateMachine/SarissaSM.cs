// 管理者 菅沼

using System.Collections.Generic;
using System;
using Sarissa.FinitAutomaton;

namespace Sarissa.FinitAutomaton
{
    /// <summary> ステートマシンの機能を提供する </summary>
    public class SarissaSM
    {
        // 通常ステート
        HashSet<SarissaSMBehaviour> _states = new HashSet<SarissaSMBehaviour>();

        // Anyステートからのステート
        HashSet<SarissaSMBehaviour> _statesFromAnyState = new HashSet<SarissaSMBehaviour>();

        // トランジション
        HashSet<SarissaSMTransition> _transitions = new HashSet<SarissaSMTransition>();

        // Anyからのトランジション
        HashSet<SarissaSMTransition> _transitionsFromAny = new HashSet<SarissaSMTransition>();

        // 現在突入しているステート
        SarissaSMBehaviour _currentPlayingStateMachineState;

        // 現在突入しているトランジション名
        int _currentTransitionId;

        // ステートマシンが一時停止中かのフラグ
        bool _isPausing = true;

        // デリゲート公開部
        public event Action<int> OnEntered;
        public event Action<int> OnUpdated;
        public event Action<int> OnExited;

        #region 登録処理

        /// <summary> ステートの登録 </summary>
        public void ResistState<T>(T stateMachineState) where T : SarissaSMBehaviour
        {
            _states.Add(stateMachineState);
            if (_currentPlayingStateMachineState == null)
            {
                _currentPlayingStateMachineState = stateMachineState;
            }
        }

        /// <summary> Anyからのステートの登録 </summary>
        public void ResistStateFromAny<T>(T stateMachineState) where T : SarissaSMBehaviour
        {
            _statesFromAnyState.Add(stateMachineState);
        }

        /// <summary> 複数のステートを引数に渡してすべての渡されたステートを登録 </summary>
        public void ResistStates<T>(List<T> states) where T : SarissaSMBehaviour
        {
            foreach (var state in states)
            {
                SarissaSMBehaviour casted = state as SarissaSMBehaviour;

                _states.Add(casted);
                if (_currentPlayingStateMachineState == null)
                {
                    _currentPlayingStateMachineState = casted;
                }
            }
        }

        /// <summary> 複数のステートを引数に渡してすべての渡されたAnyからのステートを登録 </summary>
        public void ResistStatesFromAny<T>(List<T> states) where T : SarissaSMBehaviour
        {
            foreach (var state in _statesFromAnyState)
            {
                SarissaSMBehaviour casted = state as SarissaSMBehaviour;

                _states.Add(casted);
            }
        }

        /// <summary> ステート間の遷移の登録 </summary>
        public void MakeTransition<T1, T2>(T1 from, T2 to, int id)
            where T1 : SarissaSMBehaviour
            where T2 : SarissaSMBehaviour
        {
            SarissaSMBehaviour t1 = from as SarissaSMBehaviour;
            SarissaSMBehaviour t2 = to as SarissaSMBehaviour;

            var tmp = new SarissaSMTransition(t1, t2, id);
            _transitions.Add(tmp);
        }

        /// <summary> Anyステートからの遷移の登録 </summary>
        public void MakeTransitionFromAny<T>(T to, int id) where T : SarissaSMBehaviour
        {
            SarissaSMBehaviour t = to as SarissaSMBehaviour;

            var tmp = new SarissaSMTransition(new DummyStateMachineStateClass(), t, id);
            _transitionsFromAny.Add(tmp);
        }

        #endregion

        #region 更新処理

        /// <summary> 任意のステート間遷移の遷移の状況を更新する。 </summary>
        public void UpdateTransition(int id, ref bool condition, bool equalsTo = true,
            bool isTrigger = false)
        {
            if (_isPausing) return; // もし一時停止中なら更新処理はしない。
            foreach (var transition in _transitions)
            {
                // 遷移する場合 // * 条件を満たしているなら前トランジションを無視してしまうのでその判定処理をはさむこと *
                // もし遷移条件を満たしていて遷移名が一致するなら
                if (
                    condition == equalsTo && transition.Id == id
                                          &&
                                          transition.From == _currentPlayingStateMachineState
                )
                {
                    _currentPlayingStateMachineState.End(); // 右ステートへの遷移条件を満たしたので抜ける
                    if (OnExited != null)
                    {
                        OnExited(_currentTransitionId);
                    }

                    if (isTrigger) condition = !equalsTo; // IsTrigger が trueなら
                    _currentPlayingStateMachineState = transition.To; // 現在のステートを右ステートに更新、遷移はそのまま
                    _currentPlayingStateMachineState.Begin(); // 現在のステートの初回起動処理を呼ぶ
                    if (OnEntered != null)
                    {
                        OnEntered(_currentTransitionId);
                    }

                    _currentTransitionId = id; // 現在の遷移ネームを更新
                }
                // 遷移の条件を満たしてはいないが、遷移ネームが一致（更新されていないなら）現在のステートの更新処理を呼ぶ
                else
                {
                    _currentPlayingStateMachineState.Tick();
                    if (OnUpdated != null)
                    {
                        OnUpdated(_currentTransitionId);
                    }
                }
            } // 全遷移を検索。
        }

        /// <summary> ANYステートからの遷移の条件を更新 </summary>
        public void UpdateTransitionFromAnyState(int id, ref bool condition, bool equalsTo = true,
            bool isTrigger = false)
        {
            if (_isPausing) return; // もし一時停止中なら更新処理はしない。
            foreach (var t in _transitionsFromAny)
            {
                // もし遷移条件を満たしていて遷移名が一致するなら
                if ((condition == equalsTo) && t.Id == id)
                {
                    _currentPlayingStateMachineState.End(); // 右ステートへの遷移条件を満たしたので抜ける
                    if (OnExited != null)
                    {
                        OnExited(_currentTransitionId);
                    }

                    if (isTrigger) condition = !equalsTo; // 遷移条件を初期化
                    _currentPlayingStateMachineState = t.To; // 現在のステートを右ステートに更新、遷移はそのまま
                    _currentPlayingStateMachineState.Begin(); // 現在のステートの初回起動処理を呼ぶ
                    if (OnEntered != null)
                    {
                        OnEntered(_currentTransitionId);
                    }

                    _currentTransitionId = id; // 現在の遷移ネームを更新
                }
                // 遷移の条件を満たしてはいないが、遷移ネームが一致（更新されていないなら）現在のステートの更新処理を呼ぶ
                else if (t.Id == id)
                {
                    _currentPlayingStateMachineState.Tick();
                    if (OnUpdated != null)
                    {
                        OnUpdated(_currentTransitionId);
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
            _currentPlayingStateMachineState.Begin();
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