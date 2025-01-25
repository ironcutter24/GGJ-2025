using System;
using UnityEngine.EventSystems;
using Utilities;

public class InputManager : Singleton<InputManager>
{
    private static bool _isActionMapLocked;

    public static InputSystem_Actions Actions { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        if (Instance == this)
        {
            Actions = new InputSystem_Actions();
            SwitchActionMapToPlayer();
        }
    }

    public static UnityEngine.InputSystem.UI.InputSystemUIInputModule GetCurrentInputModule()
    {
        return EventSystem.current.currentInputModule as UnityEngine.InputSystem.UI.InputSystemUIInputModule;
    }

    #region ActionMaps Helpers

    public static bool SwitchActionMapToPlayer() => SwitchActionMap(Actions.Player.Enable);
    public static bool SwitchActionMapToUI() => SwitchActionMap(Actions.UI.Enable);

    public static bool SwitchActionMap(params Action[] enableActions)
    {
        bool result = DisableAllActionMaps();
        if (result)
        {
            foreach (var action in enableActions)
                action?.Invoke();
        }
        return result;
    }

    public static bool DisableAllActionMaps()
    {
        if (_isActionMapLocked)
        {
            //Debug.LogWarning("Action Map is locked and cannot be changed.", this);
            return false;
        }

        Actions.Disable();

#if UNITY_EDITOR
        Actions.Debug.Enable();
#endif

        return true;
    }

    public static void LockActionMap()
    {
        _isActionMapLocked = true;
    }

    public static void UnlockActionMap()
    {
        _isActionMapLocked = false;
    }

    #endregion
    
}