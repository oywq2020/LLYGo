using System;
using System.Collections.Generic;

namespace _core.Script.FSM
{
    public interface IState
    {
        void Enter();
        void Update();
        void FixedUpdate();
        void Exit();
    }

    public class CustomState : IState
    {
        private Action _onEnter;
        private Action _onUpdate;
        private Action _onFixedUpdate;
        private Action _onExit;

        public CustomState OnEnter(Action onEnter)
        {
            _onEnter = onEnter;
            return this;
        }

        public CustomState OnUpdate(Action onUpdate)
        {
            _onUpdate = onUpdate;
            return this;
        }

        public CustomState OnFixedUpdate(Action onFixedUpdate)
        {
            _onFixedUpdate = onFixedUpdate;
            return this;
        }

        public CustomState OnExit(Action onExit)
        {
            _onExit = onExit;
            return this;
        }

        public void Enter()
        {
            _onEnter?.Invoke();
        }

        public void Update()
        {
            _onUpdate?.Invoke();
        }

        public async void FixedUpdate()
        {
            _onFixedUpdate?.Invoke();
        }

        public void Exit()
        {
            _onExit?.Invoke();
        }
    }


    public class FSM<T>
    {
        //store customers' states 
        public Dictionary<T, IState> States = new Dictionary<T, IState>();

        public Action<T> OnStateChanged;

        public CustomState State(T t)
        {
            if (States.TryGetValue(t, out IState state))
            {
                return state as CustomState;
            }
            else
            {
                var customState = new CustomState();
                States.Add(t, customState);
                return customState;
            }
        }

        private IState _currentState;
        private T _currentStateId;

        //encapsulation
        public IState CurrentState
        {
            get=> _currentState;
            set => _currentState = value;
        }
        public T CurrentStateId
        {
            get=> _currentStateId; 
            set=> _currentStateId = value; 
        }

        public void ChangeState(T t)
        {
            if (States.TryGetValue(t, out IState state))
            {
                if (CurrentState != null)
                {
                    CurrentState.Exit();
                    CurrentState = state;
                    CurrentStateId = t;
                    CurrentState.Enter();
                    
                    //trigger event
                    OnStateChanged?.Invoke(t);
                }
            }
        }

        public void StartState(T t)
        {
            if (States.TryGetValue(t,out IState state))
            {
                CurrentState = state;
                CurrentStateId = t;
                state.Enter();
                
                //trigger event
                OnStateChanged?.Invoke(t);
            }
        }


        public void Clear()
        {
            States.Clear();
        }
        
      
        public void Update()
        {
            CurrentState?.Update();
        }
        
        public void FixedUpdate()
        {
            CurrentState?.FixedUpdate();
        }
    }
}