using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace CabinIcarus.Joystick.Components
{
    [AddComponentMenu("CabinIcarus/UI/Advanced Button", 30)]
    public class AdvancedButton : Button
    {
        public bool IsDownTrigger;
        public bool IsHold = false;
        public float HoldTriggerTime = 0.5f;
        public KeyInfo Keys;

        private float _lastTime;
        
        private void Update()
        {
            if (!IsActive() || !IsInteractable() || Keys == null)
            {
                return;
            }
            
            Keys.Update();
            
            if (Keys.Down || Keys.Hold)
            {
                DoStateTransition(SelectionState.Pressed, false);
            }

            if (Keys.Up)
            {
                DoStateTransition(SelectionState.Normal, false);
            }

            if (IsHold)
            {
                if (Keys.Hold | IsPressed())
                {
                    if (Time.time < _lastTime)
                    {
                        return;
                    }

                    _lastTime = Time.time + HoldTriggerTime;
                    
                    onClick.Invoke();

                    return;
                }
            }
            
            if (!IsDownTrigger)
            {
                if (Keys.Up)
                {
                    onClick.Invoke();
                }
            }
            else
            {
                if (Keys.Down)
                {
                    onClick.Invoke();
                }
            }
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (IsHold)
            {
                return;
            }
            
            base.OnPointerClick(eventData);
        }
    }
}