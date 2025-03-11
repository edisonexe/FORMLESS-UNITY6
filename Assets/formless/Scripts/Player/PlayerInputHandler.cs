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

        // �������� ������ ��� �������� (WASD ��� �������)
        public Vector2 GetMoveInput()
        {
            Vector2 input = inputActions.Player.Moving.ReadValue<Vector2>();
            return input;
        }

        // �������� �� ������ ������ �����
        public bool IsBasicAttackPressed()
        {
            return inputActions.Player.BasicAttack.triggered;
        }

        // �������� �� ������� �����
        public bool IsStrongAttackPressed()
        {
            return inputActions.Player.StrongAttack.triggered;
        }

        public bool IsInteractionPressed()
        {
            return inputActions.Player.Interaction.triggered;
        }

        public bool IsRebirthPressed()
        {
            return inputActions.Player.Rebirth.triggered;
        }

        public bool IsUseBombPressed()
        {
            return inputActions.Player.UseBomb.triggered;
        }
    }
}
