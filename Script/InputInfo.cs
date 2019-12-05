using System;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

#endif

namespace CabinIcarus.Joystick
{
    [Serializable]
    public class InputInfo
    {
        public bool Enable = true;

#if ENABLE_INPUT_SYSTEM
        public Key PrimaryKey;
        public Key Secondary;
#else
        public KeyCode PrimaryKey;
        public KeyCode Secondary;
#endif
        
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

        #if ENABLE_INPUT_SYSTEM

            var keyboard = Keyboard.current;
            
            Down = (PrimaryKey != Key.None && keyboard[PrimaryKey].wasPressedThisFrame) | (Secondary != Key.None && keyboard[Secondary].wasPressedThisFrame);
            Hold = (PrimaryKey != Key.None && keyboard[PrimaryKey].isPressed) | (Secondary != Key.None && keyboard[Secondary].isPressed);
            Up = (PrimaryKey != Key.None && keyboard[PrimaryKey].wasReleasedThisFrame) | (Secondary != Key.None && keyboard[Secondary].wasReleasedThisFrame);
        #else
            Down = Input.GetKeyDown(PrimaryKey) | Input.GetKeyDown(Secondary);
            Hold = Input.GetKey(PrimaryKey) | Input.GetKey(Secondary);
            Up = Input.GetKeyUp(PrimaryKey) | Input.GetKeyUp(Secondary);
        #endif

        }
    }
}