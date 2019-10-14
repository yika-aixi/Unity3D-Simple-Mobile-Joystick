using UnityEngine;
using UnityEngine.UI;

namespace CabinIcarus.Joystick.Components
{
    [AddComponentMenu("CabinIcarus/UI/Advanced Button", 30)]
    public class AdvancedButton : Button
    {
        public bool IsDownTrigger;
        public KeyInfo keys;

        private void Update()
        {
            if (!IsActive() || !IsInteractable() || keys == null)
            {
                return;
            }
            
            keys.Update();
            
            if (keys.Down || keys.Hold)
            {
                DoStateTransition(SelectionState.Pressed, false);
            }

            if (keys.Up)
            {
                DoStateTransition(SelectionState.Normal, false);
            }

            if (!IsDownTrigger)
            {
                if (keys.Up)
                {
                    onClick.Invoke();
                }
            }
            else
            {
                if (keys.Down)
                {
                    onClick.Invoke();
                }
            }
        }

        public void Log()
        {
            Debug.LogError("12");
        }
    }
}