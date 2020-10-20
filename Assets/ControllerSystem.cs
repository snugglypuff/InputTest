using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

public class ControllerSystem : MonoBehaviour
{
    [SerializeField] private InputData inputData;
    [SerializeField] private MenuHandler mainMenu;

    /* Bind an action with a new path. */
    public void KeyBinding( InputAction _action, string _path )
    {
        _action.ApplyBindingOverride( _path );
    }

    /* Start binding operation - currently excluding mouse position/delta.
        Setup a callback to dispose of this operation to protect against a memory leak.
        Return to main menu script with the new path and the id of the button that was clicked. */
    public void KeyBindingOverride( InputAction _action, int _id )
    {
        RebindingOperation rebindOperation = _action.PerformInteractiveRebinding();
        rebindOperation.WithControlsExcluding("<Pointer>/position").WithControlsExcluding("<Pointer>/delta")

       /* rebindOperation
            .WithControlsExcluding("<Mouse>")
            .WithControlsExcluding("<Keyboard>")*/
            .OnMatchWaitForAnother( 0.1f )
            .Start().OnComplete( _callback => {
                rebindOperation.Dispose();
                mainMenu.ReturnFromMapping( _action.bindings[0].effectivePath, _id );

            } );
    }

    /* Start binding operation - currently excluding mouse position/delta.
        Setup a callback to dispose of this operation to protect against a memory leak.
        Return to main menu script with the new path and the id of the button that was clicked. 
    
        This one is a mess. Couldn't figure out a better way to map composites.
        On re bind, get our current composite path.
        Start operation with a placeholder action - composite bindings do not have a rebind operation as far as I'm aware so this is a way to get around that.
        Whatever is returned from the binding operation can now be used for the composite binding.
        Example: if the user clicks the UI button for UP then we'll rebind on the placeholder, get the new path for that and then use this new path for only UP.
            We'll then use the old paths for Down, Left and Right because we're currenly only binding UP.

        Bind this new composite and return to main meny script with new path and id of button clicked.
         */
    public void CompositeBindingOverride( InputAction _action, int _id )
    {
        string _up = inputData.MovementAction.bindings[1].effectivePath;
        string _down = inputData.MovementAction.bindings[2].effectivePath;
        string _left = inputData.MovementAction.bindings[3].effectivePath;
        string _right = inputData.MovementAction.bindings[4].effectivePath;

        RebindingOperation rebindOperation = _action.PerformInteractiveRebinding();
        rebindOperation.WithControlsExcluding( "<Pointer>/position" ).WithControlsExcluding( "<Pointer>/delta" )
            .OnMatchWaitForAnother( 0.1f )
            .Start().OnComplete( _callback => {
                rebindOperation.Dispose();

                switch(_id)
                {
                    case 0:
                        _up = _action.bindings[0].effectivePath;
                        break;
                    case 1:
                        _down = _action.bindings[0].effectivePath;
                        break;
                    case 2:
                        _left = _action.bindings[0].effectivePath;
                        break;
                    case 3:
                        _right = _action.bindings[0].effectivePath;
                        break;
                    default:
                        break;
                }

                BindComposite( _up, _down, _left, _right );
                mainMenu.ReturnFromMapping( _action.bindings[0].effectivePath, _id );
            } );
    }
    
    /* Function for binding composite actions. 
        It seems like you can't override composite bindings so we have to erase the old one and then rebind it.*/
    public void BindComposite(string _up, string _down, string _left, string _right)
    {
        inputData.MovementAction = new InputAction();
        inputData.MovementAction.AddCompositeBinding( "2DVector" )
            .With( "Up", _up )
            .With( "Down", _down )
            .With( "Left", _left )
            .With( "Right", _right );
    }
}