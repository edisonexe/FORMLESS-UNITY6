using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput Instance {  get; private set; }

    private PlayerInputAction _playerInputAction;

    public event EventHandler OnPlayerInputBasicAttack;
    public event EventHandler OnPlayerInputStrongAttack;

    private void Awake()
    {
        Instance = this;
        _playerInputAction = new PlayerInputAction();
        _playerInputAction.Enable();
        _playerInputAction.Fighting.BasicAttack.started += PlayerBasicAttack_started;
        _playerInputAction.Fighting.StrongAttack.started += PlayerStrongAttack_started;
    }

    private void OnDestroy()
    {
        _playerInputAction.Fighting.BasicAttack.started -= PlayerBasicAttack_started;
        _playerInputAction.Fighting.StrongAttack.started -= PlayerStrongAttack_started;
    }

    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = _playerInputAction.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }

    public Vector3 GetMousePositions()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        return mousePosition;
    }

    public void DisablePlayerInput()
    {
        _playerInputAction.Disable();
    }

    private void PlayerBasicAttack_started(InputAction.CallbackContext obj)
    {
        OnPlayerInputBasicAttack?.Invoke(this, EventArgs.Empty);
    }

    private void PlayerStrongAttack_started(InputAction.CallbackContext obj)
    {
        OnPlayerInputStrongAttack?.Invoke(this, EventArgs.Empty);
    }
}
