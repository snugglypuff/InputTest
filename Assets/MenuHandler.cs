using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class MenuHandler : MonoBehaviour
{
    /* Screens */
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsPanel;

    /* UI */
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button optionsBackButton;
    [SerializeField] private CanvasGroup controlsPanel;
    [SerializeField] private Button[] keyButtons;

    /* Data */
    [SerializeField] private ControllerSystem controller;
    [SerializeField] private InputData inputData;
    private string[] keyPaths;

    private Vector2 move;

    public enum KeyList
    {
        Up,
        Down,
        Left,
        Right,
        Dodge,
        Use,
        Sword,
        Gun,

        COUNT
    }
    
    private void Start()
    {
        SetupGame();
    }

    private void Update()
    {
        TestInput();

        if (Gamepad.all.Count > 0)
        {
            Debug.Log(Gamepad.current.name);
            Gamepad.all[0].SetMotorSpeeds(0.25f, 0.75f);
        }
    }

    /* Placeholder. Break this would probably be split across another script, but for this test eveyrthing is handled in the menu handler */
    private void SetupGame()
    {
        /* Check for save folders and load. */
        SaveSystem.Init();
        SaveSystem.LoadControllerData();

        keyPaths = new string[(int)KeyList.COUNT];

        /* Get a copy of our loaded data, these are the paths for each of our actions */
        keyPaths[(int)KeyList.Up] = PersistentData.Instance.UpPath;
        keyPaths[(int)KeyList.Down] = PersistentData.Instance.DownPath;
        keyPaths[(int)KeyList.Left] = PersistentData.Instance.LeftPath;
        keyPaths[(int)KeyList.Right] = PersistentData.Instance.RightPath;
        keyPaths[(int)KeyList.Dodge] = PersistentData.Instance.DodgePath;
        keyPaths[(int)KeyList.Use] = PersistentData.Instance.UsePath;
        keyPaths[(int)KeyList.Sword] = PersistentData.Instance.SwordPath;
        keyPaths[(int)KeyList.Gun] = PersistentData.Instance.GunPath;

        /* Setup the UI buttons with the correct text from the loaded data */
        SetButtonText( keyButtons[(int)KeyList.Up], PersistentData.Instance.UpPath );
        SetButtonText( keyButtons[(int)KeyList.Down], PersistentData.Instance.DownPath );
        SetButtonText( keyButtons[(int)KeyList.Left], PersistentData.Instance.LeftPath );
        SetButtonText( keyButtons[(int)KeyList.Right], PersistentData.Instance.RightPath );
        SetButtonText( keyButtons[(int)KeyList.Dodge], PersistentData.Instance.DodgePath );
        SetButtonText( keyButtons[(int)KeyList.Use], PersistentData.Instance.UsePath );
        SetButtonText( keyButtons[(int)KeyList.Sword], PersistentData.Instance.SwordPath );
        SetButtonText( keyButtons[(int)KeyList.Gun], PersistentData.Instance.GunPath );

        /* Setup the button listeners. */
        optionsButton.onClick.AddListener(OpenOptions);
        quitButton.onClick.AddListener(QuitGame);
        optionsBackButton.onClick.AddListener(OptionsBack);
        keyButtons[(int)KeyList.Up].onClick.AddListener(() => CompositeRemapping( inputData.placeholderAction, (int)KeyList.Up) );
        keyButtons[(int)KeyList.Down].onClick.AddListener(() => CompositeRemapping( inputData.placeholderAction, (int)KeyList.Down) );
        keyButtons[(int)KeyList.Left].onClick.AddListener(() => CompositeRemapping( inputData.placeholderAction, (int)KeyList.Left) );
        keyButtons[(int)KeyList.Right].onClick.AddListener(() => CompositeRemapping( inputData.placeholderAction, (int)KeyList.Right) );
        keyButtons[(int)KeyList.Dodge].onClick.AddListener(() => KeyRemapping( inputData.DodgeAction, (int)KeyList.Dodge) );
        keyButtons[(int)KeyList.Use].onClick.AddListener(() => KeyRemapping( inputData.UseAction, (int)KeyList.Use) );
        keyButtons[(int)KeyList.Sword].onClick.AddListener(() => KeyRemapping( inputData.SwordAction, (int)KeyList.Sword) );
        keyButtons[(int)KeyList.Gun].onClick.AddListener(() => KeyRemapping( inputData.GunAction, (int)KeyList.Gun) );

        /* Bind the paths into our controller actions. */
        controller.BindComposite(
            keyPaths[(int)KeyList.Up],
            keyPaths[(int)KeyList.Down],
            keyPaths[(int)KeyList.Left],
            keyPaths[(int)KeyList.Right] );
        controller.KeyBinding( inputData.DodgeAction, keyPaths[(int)KeyList.Dodge] );
        controller.KeyBinding( inputData.UseAction, keyPaths[(int)KeyList.Use] );
        controller.KeyBinding( inputData.SwordAction, keyPaths[(int)KeyList.Sword] );
        controller.KeyBinding( inputData.GunAction, keyPaths[(int)KeyList.Gun] );

        /* Enable our buttons for use on the main menu screen. */
        inputData.EnableAllButtons();
    }

    /* Used in a button listener. 
        Shuffles menu screens and sets our controller actions to disabled. The actions need to be disabled to correctly bind them. */
    private void OpenOptions()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
        inputData.DisableAllButtons();
    }

    private void QuitGame()
    {
        Debug.Log( "Closing Game!" );
    }

    /* Used in a button listener. 
        Shuffles menu screens. Enables our controller actions so they may be used. Load our current paths back into the persistent game data and then save this. */
    private void OptionsBack()
    {
        mainMenuPanel.SetActive( true );
        optionsPanel.SetActive( false );

        inputData.EnableAllButtons();

        PersistentData.Instance.UpPath = keyPaths[(int)KeyList.Up];
        PersistentData.Instance.DownPath = keyPaths[(int)KeyList.Down];
        PersistentData.Instance.LeftPath = keyPaths[(int)KeyList.Left];
        PersistentData.Instance.RightPath = keyPaths[(int)KeyList.Right];
        PersistentData.Instance.DodgePath = keyPaths[(int)KeyList.Dodge];
        PersistentData.Instance.UsePath = keyPaths[(int)KeyList.Use];
        PersistentData.Instance.SwordPath = keyPaths[(int)KeyList.Sword];
        PersistentData.Instance.GunPath = keyPaths[(int)KeyList.Gun];

        SaveSystem.SaveControllerData();
    }

    /* Helper function */
    private void SetButtonText( Button _button, string _text )
    {
        _button.GetComponentInChildren<TMP_Text>().SetText( _text );
    }

    /* Helper function */
    private void SetButtonAnimator( Button _button, bool _bool )
    {
        _button.GetComponent<Animator>().SetBool( "mapping", _bool );
    }

    /* Used in a button listener. 
        Used for mapping normal keys. Set our remap ui to not be clickable. Set ui button to blank and start an animation. Tell the controller to start the rebind operation. */
    private void KeyRemapping( InputAction _action, int _id )
    {
        controlsPanel.interactable = false;
        SetButtonText( keyButtons[_id], "__" );
        SetButtonAnimator( keyButtons[_id], true );
        controller.KeyBindingOverride( _action, _id );
    }

    /* Used in a button listener. 
        Used for mapping composite key group. set our remap ui to not be clickable. Set ui button to blank and start an animation. Tell the controller to start the rebind operation. */
    private void CompositeRemapping( InputAction _action, int _id )
    {
        controlsPanel.interactable = false;
        SetButtonText( keyButtons[_id], "__" );
        SetButtonAnimator( keyButtons[_id], true );
        controller.CompositeBindingOverride( _action, _id );
    }

    /* **Not sure if this is the best way to go about this.
        When the user starts a binding operation on the controller the operation has a callback. The callback calls this function to return here. Basically a bootleg promise.
        Set our path to returned path, re enable UI for clicks. Set ui button to ne path text and turn off animation.
        Then check to make sure they user hasn't already used this bind.*/
    public void ReturnFromMapping( string _buttonText, int _id )
    {
        keyPaths[_id] = _buttonText;
        controlsPanel.interactable = true;
        SetButtonText( keyButtons[_id], _buttonText );
        SetButtonAnimator( keyButtons[_id], false );
        CheckForDupeBind( _buttonText, _id );
    }

    /* Loop through paths to check to see if it is already in use. */
    private void CheckForDupeBind( string _buttonText, int _id )
    {
        for(int i = 0; i < keyPaths.Length; i++)
        {
            if(keyPaths[i] == _buttonText && _id != i)
            {
                UnBindDupe( i );
            }
        }
    }

    /* If we find a dupe or dupes then we'll unbind this action. */
    private void UnBindDupe( int _id )
    {
        switch( _id )
        {
            case 0:
                UnbindComposite( _id );
                break;
            case 1:
                UnbindComposite( _id );
                break;
            case 2:
                UnbindComposite( _id );
                break;
            case 3:
                UnbindComposite( _id );
                break;
            case 4:
                UnbindKey( inputData.DodgeAction, _id );
                break;
            case 5:
                UnbindKey( inputData.UseAction, _id );
                break;
            case 6:
                UnbindKey( inputData.SwordAction, _id );
                break;
            case 7:
                UnbindKey( inputData.GunAction, _id );
                break;
            default:
                break;
        }
    }

    /* Unbind function for normal keys. */
    private void UnbindKey( InputAction _action, int _id )
    {
        controller.KeyBinding( _action, " " );
        keyPaths[_id] = " ";
        SetButtonText( keyButtons[_id], " " );
    }

    /* Unbind function for composite keys. */
    private void UnbindComposite( int _id )
    {
        string _up = keyPaths[(int)KeyList.Up];
        string _down = keyPaths[(int)KeyList.Down];
        string _left = keyPaths[(int)KeyList.Left];
        string _right = keyPaths[(int)KeyList.Right];

        switch( _id )
        {
            case 0:
                _up = " ";
                break;
            case 1:
                _down = " ";
                break;
            case 2:
                _left = " ";
                break;
            case 3:
                _right = " ";
                break;
            default:
                break;
        }

        controller.BindComposite( _up, _down, _left, _right );
        keyPaths[_id] = " ";
        SetButtonText( keyButtons[_id], " ");
    }

    /* Function just to test our keys out. This only works on the main meny panel. */
    private void TestInput()
    {
        if (inputData.DodgeAction.triggered)
        {
            Debug.Log("Dodging!");
        }

        if (inputData.UseAction.triggered)
        {
            Debug.Log("Interacting!");
        }

        if (inputData.SwordAction.triggered)
        {
            Debug.Log("Attacking!");
        }

        if (inputData.GunAction.triggered)
        {
            Debug.Log("Shooting!");
        }

        move = inputData.MovementAction.ReadValue<Vector2>();
        if (move != Vector2.zero)
        {
            Debug.Log(move);
        }
    }
}