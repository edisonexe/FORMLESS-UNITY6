using UnityEngine;

namespace Formless.SM
{
    public class StateMachine
    {
        private State currentState;

        public void ChangeState(State newState)
        {
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
