using UnityEngine;
public class JoyStickInputService :  MonoBehaviour, IInputService
{
    [SerializeField] FloatingJoyStick f;
    public Vector3 MoveDirection =>f==null?Vector3.zero:new Vector3(f.Horizontal,0,f.Vertical);
    public void SetJoyStick(FloatingJoyStick joyStick) => f= joyStick;
}