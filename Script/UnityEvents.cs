using System;
using UnityEngine.Events;

namespace CabinIcarus.Joystick.Evetns
{
   /// <summary>
   /// 无参事件
   /// </summary>
   [Serializable]
   public class NoParEvent : UnityEvent
   {
   }
   
   [Serializable]
   public class InputDownEvent:UnityEvent{}
   
   [Serializable]
   public class InputUpEvent:UnityEvent{}
   
   [Serializable]
   public class InputHold : UnityEvent<float>{}
}