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
        
        public Text CoolingText;
        
        [SerializeField]
        private bool _coolingState;

        public void SetCoolingTime(float time)
        {
            CoolingTime = time;
        }
        
        protected override void Awake()
        {
            if (CoolingMask)
            {
                CoolingMask.enabled = false;
            }
            
            if (CoolingText)
            {
                CoolingText.enabled = false;
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
            
            if (CoolingText)
            {
                CoolingText.enabled = true;
            }
            
            interactable = false;
        }

        public NoParEvent OnCoolingComplete = new NoParEvent();
        public FloatParEvent OnCoolingChange = new FloatParEvent();
        
        private void FixedUpdate()
        {
            if (_coolingState)
            {
                CurrentCoolingTime -= Time.fixedDeltaTime;

                if (CurrentCoolingTime < 0)
                {
                    CurrentCoolingTime = 0;
                }
                
                if (CoolingMask)
                {
                    CoolingMask.fillAmount = CurrentCoolingTime / CoolingTime;
                }
                
                OnCoolingChange?.Invoke(CurrentCoolingTime);
                
                CoolingText.text = CurrentCoolingTime.ToString("F");
                
                if (CurrentCoolingTime <= 0)
                {
                    _complete();
                }
            }
        }

        private void _complete()
        {
            ExitCooling();

            OnCoolingComplete.Invoke();
        }

        public void ExitCooling()
        {
            _coolingState = false;

            interactable = true;
            if (CoolingMask)
            {
                CoolingMask.enabled = false;
            }

            if (CoolingText)
            {
                CoolingText.enabled = false;
            }
        }
    }
}