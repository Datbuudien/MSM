using UnityEngine;
using UnityEngine.InputSystem;
public class KeyBoardInputService : MonoBehaviour, IInputService
{
    public Vector3 MoveDirection
    {
        get
        {
            Keyboard kb = Keyboard.current;
            float x=0f;
            float z = 0f;
            if(kb.aKey.isPressed || kb.leftArrowKey.isPressed) x-=1f;
            if(kb.dKey.isPressed|| kb.rightArrowKey.isPressed) x+=1f;
            if(kb.wKey.isPressed || kb.upArrowKey.isPressed) z+=1f;
            if(kb.sKey.isPressed ||kb.downArrowKey.isPressed)z-=1f;
            return new Vector3(x,0,z).normalized;
        }
    }
}