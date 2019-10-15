using System;
using UnityEngine.Events;

namespace CabinIcarus.Joystick.Evetns
{
   [Serializable]
   public class InputDownEvent:UnityEvent{}
   
   [Serializable]
   public class InputUpEvent:UnityEvent{}
   
   [Serializable]
   public class InputHold : UnityEvent<float>{}
}