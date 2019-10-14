using System;
using UnityEngine;

namespace CabinIcarus.Joystick
{
    [Serializable]
    public class KeyInfo
    {
        public KeyCode PrimaryKey;
        public KeyCode Secondary;
        
        public bool Enable;

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
        }
    }
}