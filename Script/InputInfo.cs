using System;
using UnityEngine;

namespace CabinIcarus.Joystick
{
    public enum InputState
    {
        None,
        Hold,
        Up,
    }
    [Serializable]
    public class InputInfo
    {
        public bool Enable = true;
        public KeyCode PrimaryKey;
        public KeyCode Secondary;

        public InputState State { get; private set; }
        public bool Hold { get; private set; }
        public bool Down { get; private set;}
        public bool Up { get; private set;}
        public void Update()
        {
            if (!Enable)
            {
                Hold = false;
                Down = false;
                Up = false;
                return;
            }
            
            Down = Input.GetKeyDown(PrimaryKey) | Input.GetKeyDown(Secondary);
            Hold = Input.GetKey(PrimaryKey) | Input.GetKey(Secondary);
            Up = Input.GetKeyUp(PrimaryKey) | Input.GetKeyUp(Secondary);

            if (Hold)
            {
                State = InputState.Hold;
            }else if (Up)
            {
                State = InputState.Up;
            }
            else
            {
                State = InputState.None;
            }
        }
    }
}