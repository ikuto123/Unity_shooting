using UnityEngine;
using System;

public class TitleInput
{
    public event Action OnEnterPressed;
    
    public void ReadInput()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnEnterPressed?.Invoke();
        }
    }
}
