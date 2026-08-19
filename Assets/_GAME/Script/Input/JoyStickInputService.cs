using UnityEngine;
public class JoyStickInputService :  MonoBehaviour, IInputService
{
    [SerializeField] FloatingJoyStick f;
    public Vector3 MoveDirection => new Vector3(f.Horizontal,0,f.Vertical); 
}