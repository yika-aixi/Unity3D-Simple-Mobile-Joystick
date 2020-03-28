//

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace CabinIcarus.Joystick.Components
{
    [RequireComponent(typeof(UnityEngine.UI.AspectRatioFitter))]
    public class JoystickComponent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,
        IPointerDownHandler, IPointerUpHandler
    {
        
#if ENABLE_INPUT_SYSTEM
        public InputAction MoveInput;

        private void Awake()
        {
            MoveInput.started += _inputAxis;
            MoveInput.performed += _inputAxis;
            MoveInput.canceled += _inputAxis;
        }

        private void OnEnable()
        {
            MoveInput.Enable();
        }

        private void OnDisable()
        {
            MoveInput.Disable();
        }

        private void OnDestroy()
        {
            MoveInput.Dispose();
            MoveInput = null;
        }
#endif
        
        public bool IsFixedUpdate = true;

        [Header("Syn Horizontal and Vertical Axis Input")]
        public bool SynAxis = true;

        [Header("Smooth Horizontal and Vertical Axis Input exit")]
        public bool SmoothExit = false;
        
        
        [Serializable]
        public class MoveStartEvent : UnityEvent
        {
        }

        [Serializable]
        public class MoveIngEvent : UnityEvent<Vector2>
        {
        }

        [Serializable]
        public class MoveEndEvent : UnityEvent
        {
        }

        public RectTransform Background;
        public RectTransform Knob;
        public float offset = 1;
        [Header("Input Values")] 
        public float Horizontal = 0;
        public float Vertical = 0;

        public MoveStartEvent OnStart;
        public MoveIngEvent OnMove;
        public MoveEndEvent OnEnd;

        [SerializeField]
        private MoveIngEvent OnEndVe2;


        Vector2 PointPosition;
        private bool _dragStart;

        public void OnBeginDrag(PointerEventData eventData)
        {
        }

        /// <summary>
        /// 计算公式来源:http://www.theappguruz.com/blog/beginners-guide-learn-to-make-simple-virtual-joystick-in-unity
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrag(PointerEventData eventData)
        {
            _dragStart = true;
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle
            (Background,
                eventData.position,
                eventData.pressEventCamera,
                out var position);

            position.x = (position.x / Background.sizeDelta.x);
            position.y = (position.y / Background.sizeDelta.y);

            float x = (Knob.pivot.x == 1f) ? position.x * 2 + 1 : position.x * 2 - 1;
            float y = (Knob.pivot.y == 1f) ? position.y * 2 + 1 : position.y * 2 - 1;

            PointPosition = new Vector3(x, y);
            PointPosition = (PointPosition.magnitude > 1) ? PointPosition.normalized : PointPosition;
        }

        private void _updateKnobOps()
        {
            Knob.anchoredPosition = new Vector3(PointPosition.x * (Background.sizeDelta.x / (1 + offset))
                , PointPosition.y * (Background.sizeDelta.y) / (1 + offset));
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _dragStart = false;
            _reset();
        }

        private void _reset()
        {
            PointPosition = Vector2.zero;
            Knob.anchoredPosition = Vector2.zero;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
            OnStart.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnEndDrag(eventData);
            OnEnd.Invoke();
            OnEndVe2.Invoke(Vector2.zero);
        }


        // Update is called once per frame
        void Update()
        {
            
#if !ENABLE_INPUT_SYSTEM
            _inputAxis();
#endif
            Horizontal = PointPosition.x;
            Vertical = PointPosition.y;

            if (!IsFixedUpdate)
            {
                if (PointPosition.magnitude > 0)
                {
                    OnMove.Invoke(PointPosition);
                }
            }

            _updateKnobOps();
        }

        private void FixedUpdate()
        {
            if (IsFixedUpdate)
            {
                if (PointPosition.magnitude > 0)
                {
                    OnMove.Invoke(PointPosition);
                }
            }
        }

        private bool _start = false;
        
#if !ENABLE_INPUT_SYSTEM
        private void _inputAxis()
#else
        private void _inputAxis(InputAction.CallbackContext context)
#endif
        {
            if (SynAxis && !_dragStart)
            {
#if !ENABLE_INPUT_SYSTEM
                PointPosition = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                float lenght = PointPosition.sqrMagnitude;
                if (!SmoothExit)
                {
                    var exit = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                    lenght = exit.sqrMagnitude;
                    if ((int) lenght == 0)
                    {
                        PointPosition = Vector2.zero;
                    }
                }
#else
                PointPosition = context.ReadValue<Vector2>();
#endif
                
                if ((int) lenght == 0)
                {
                    if (_start)
                    {
                        _start = false;
                        OnEnd.Invoke();
                        OnEndVe2.Invoke(Vector2.zero);
                        _reset();
                    }

                    return;
                }

                if (!_start)
                {
                    _start = true;
                    OnStart.Invoke();
                }

            }
        }

        public Vector2 Coordinate()
        {
            return new Vector2(Horizontal, Vertical);
        }
    }
}
