using UnityEngine;

public static class InputOverride
{
    private static bool _isInputEnabled = false;

    public static void EnableInput()
    {
        _isInputEnabled = true;
    }

    public static void DisableInput()
    {
        _isInputEnabled = false;
    }

    public static bool GetKeyUp(KeyCode key)
    {
        if (!_isInputEnabled)
            return false;

        return Input.GetKeyUp(key);
    }
}