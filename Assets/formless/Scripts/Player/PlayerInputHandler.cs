using UnityEngine;
using UnityEngine.Windows;

namespace Formless.Player
{
    public class PlayerInputHandler
    {
        private PlayerInputAction inputActions;

        public PlayerInputHandler()
        {
            inputActions = new PlayerInputAction();
            inputActions.Enable();
        }

        public void Enable()
        {
            inputActions.Player.Enable();
        }

        public void Disable()
        {
            inputActions.Player.Disable();
        }

        // Получаем данные для движения (WASD или стрелки)
        public Vector2 GetMoveInput()
        {
            Vector2 input = inputActions.Player.Moving.ReadValue<Vector2>();
            return input;
        }

        // Проверка на слабую кнопку атаки
        public bool IsBasicAttackPressed()
        {
            return inputActions.Player.BasicAttack.triggered;
        }

        // Проверка на сильную атаку
        public bool IsStrongAttackPressed()
        {
            return inputActions.Player.StrongAttack.triggered;
        }

        //public bool IsBasicAttackPerformed()
        //{
        //    return inputActions.Player.BasicAttack.WasPerformedThisFrame();
        //}

        //public bool IsStrongAttackPerformed()
        //{
        //    return inputActions.Player.StrongAttack.WasPerformedThisFrame();
        //}

        public bool IsInteractionPressed()
        {
            return inputActions.Player.Interaction.triggered;
        }
    }
}
