using UnityEngine;
public enum InputType{JoyStick,KeyBoard,Auto};
public class InputManager : Singleton<InputManager>
{
    [SerializeField] private InputType inputType = InputType.JoyStick;
    [SerializeField] private JoyStickInputService joyInput;
    [SerializeField] private KeyBoardInputService keyInput;

    private bool IsEnableInput=true;
    private InterfaceInputService currentInput;
    public  Vector3 MoveDirection
    {
        get
        {
            if(IsEnableInput && currentInput !=null) return currentInput.MoveDirection;
            else return Vector3.zero;
        }
    }
    protected override void Awake()
    {
        base.Awake();
        if(IsDuplicate) return;
        SetInputType(inputType);
    }

    public void EnableInput(bool ok){IsEnableInput = ok;}
    public void SetInputType(InputType type)
    {
        inputType=type;
        currentInput = ResolveInput(type);
    }
    private InterfaceInputService ResolveInput(InputType type)
    {
        switch (type)
        {
            case InputType.JoyStick: return joyInput;
            case InputType.KeyBoard: return keyInput;
            default: return null;
        }
    }
    
}