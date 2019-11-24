using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu]
public class InputData : ScriptableObject
{
    public InputAction MovementAction;
    public InputAction SwordAction;
    public InputAction GunAction;
    public InputAction DodgeAction;
    public InputAction UseAction;

    public InputAction PauseAction;

    /* I don't feel like writing a function to deal with composite keymapping so I'm doing just going 
    to use this placeholder to capture the effectivePath on rebind and then feed that into the composite binding. */
    public InputAction placeholderAction;

    private void OnEnable()
    {
        MovementAction.Enable();
        SwordAction.Enable();
        GunAction.Enable();
        DodgeAction.Enable();
        UseAction.Enable();
        PauseAction.Enable();
    }

    private void OnDisable()
    {
        MovementAction.Disable();
        SwordAction.Disable();
        GunAction.Disable();
        DodgeAction.Disable();
        UseAction.Disable();
        PauseAction.Enable();
    }

    public void DisableAllButtons()
    {
        MovementAction.Disable();
        SwordAction.Disable();
        GunAction.Disable();
        DodgeAction.Disable();
        UseAction.Disable();
        PauseAction.Disable();
    }

    public void EnableAllButtons()
    {
        MovementAction.Enable();
        SwordAction.Enable();
        GunAction.Enable();
        DodgeAction.Enable();
        UseAction.Enable();
        PauseAction.Enable();
    }
}