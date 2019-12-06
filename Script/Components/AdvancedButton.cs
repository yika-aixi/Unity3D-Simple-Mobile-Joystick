using CabinIcarus.Joystick.Evetns;
using UnityEngine;
using UnityEngine.EventSystems;
using Button = UnityEngine.UI.Button;

namespace CabinIcarus.Joystick.Components
{
    [AddComponentMenu("CabinIcarus/UI/Advanced Button", 30)]
    public class AdvancedButton : Button
    {
        public bool UseDownTrigger;
        public float HoldTriggerInterval;
        public InputInfo InputBind;

        public InputUpEvent OnDown = new InputUpEvent();
        public InputUpEvent OnUp = new InputUpEvent();
        public FloatParEvent OnHold = new FloatParEvent();
        
        protected virtual void Update()
        {
            if (!IsActive() || !IsInteractable() || InputBind == null)
            {
                return;
            }
            
            InputBind.Update();
            
            _stateTransition();

            _downHandle();
            
            _holdHandle();

            _clickEventHandle();

            _upHandle(false);
        }

        #region Event Handle

        private bool _downT;
        private float _holdTime;
        private float _lastHoldTriggerTime;
        
        private void _downHandle()
        {
            if (UseDownTrigger)
            {
                if (_downT)
                {
                    return;   
                }
                
                if (InputBind.Down || IsPressed())
                {
                    _downT = true;
                    _lastHoldTriggerTime = Time.time + HoldTriggerInterval;
                    OnDown.Invoke();
                }
            }
        }
        
        private void _upHandle(bool isPoint)
        {
            if (UseDownTrigger)
            {
                if (!_downT)
                {
                    return;
                }
                
                if (InputBind.Up && !isPoint || !IsPressed() && isPoint)
                {
                    _downT = false;
                    OnUp.Invoke();
                }
            }
            
            //reset Time
            if (InputBind.Up && !isPoint || !IsPressed() && isPoint)
            {
                _holdTime = 0;
                _lastHoldTriggerTime = 0;
            }
        }

        private void _holdHandle()
        {
            if (InputBind.Hold || IsPressed())
            {
                _holdTime += Time.deltaTime;

                if (Time.time < _lastHoldTriggerTime)
                {
                    return;
                }

                _lastHoldTriggerTime = Time.time + HoldTriggerInterval;
                
                OnHold.Invoke(_holdTime);
            }
        }
        
        private void _clickEventHandle()
        {
            if (!UseDownTrigger)
            {
                if (InputBind.Up && !IsPressed())
                {
                    onClick.Invoke();
                }
            }
        }

        #endregion

        private void _stateTransition()
        {
            if (InputBind.Down || InputBind.Hold)
            {
                DoStateTransition(SelectionState.Pressed, false);
            }

            if (InputBind.Up)
            {
                DoStateTransition(SelectionState.Normal, false);
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            _downHandle();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            _upHandle(true);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (UseDownTrigger)
            {
                return;
            }
            
            base.OnPointerClick(eventData);
        }

        public void DebugDown()
        {
            Debug.LogError("Down");
        }
        public void DebugHold(float time)
        {
            Debug.LogError($"Hold:{time}");
        }
        public void DebugUP()
        {
            Debug.LogError("UP");
        }
        public void DebugClick()
        {
            Debug.LogError("Click");
        }
    }
}