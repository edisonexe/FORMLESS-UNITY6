using UnityEngine;

namespace Formless.SM
{
    public class StateMachine
    {
        private State currentState;

        public State CurrentState => currentState;

        public void ChangeState(State newState)
        {
            //Debug.Log($"Смена состояния: {currentState?.GetType().Name} -> {newState.GetType().Name}");

            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }

        public void Update()
        {
            currentState?.Update();
        }

         public void FixedUpdate()
         {
            currentState?.FixedUpdate();
         }
    }
}
