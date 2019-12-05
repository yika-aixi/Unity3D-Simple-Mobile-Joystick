using CabinIcarus.Joystick.Evetns;
using UnityEngine;
using UnityEngine.UI;

namespace CabinIcarus.Joystick.Components
{
    public class CoolingButton : AdvancedButton
    {
        public float CoolingTime;

        public float CurrentCoolingTime;

        public Image CoolingMask;

        [SerializeField]
        private bool _coolingState;
        
        protected override void Awake()
        {
            if (CoolingMask)
            {
                CoolingMask.enabled = false;
            }

            if (_coolingState)
            {
                EnterCooling();
            }
        }

        public void EnterCooling()
        {
            _coolingState = true;
            CurrentCoolingTime = CoolingTime;
            if (CoolingMask)
            {
                CoolingMask.enabled = true;
            }

            interactable = false;
        }

        public NoParEvent OnCoolingComplete = new NoParEvent();

        private void FixedUpdate()
        {
            if (_coolingState)
            {
                CurrentCoolingTime -= Time.fixedDeltaTime;

                if (CoolingMask)
                {
                    CoolingMask.fillAmount = CurrentCoolingTime / CoolingTime;
                }
                
                if (CurrentCoolingTime <= 0)
                {
                    _complete();
                }
            }
        }

        private void _complete()
        {
            _coolingState = false;
            
            interactable = true;
            if (CoolingMask)
            {
                CoolingMask.enabled = false;
            }

            OnCoolingComplete.Invoke();
        }
    }
}