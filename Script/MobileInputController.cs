//

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
[RequireComponent(typeof(UnityEngine.UI.AspectRatioFitter))]
public class MobileInputController : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler,IPointerDownHandler,IPointerUpHandler
{
    [Header("Syn Horizontal and Vertical Axis Input")]
    public bool SynAxis = true;

    [Serializable]
    public class MoveStartEvent:UnityEvent {}
    [Serializable]
    public class MoveIngEvent:UnityEvent<Vector2> {}
    [Serializable]
    public class MoveEndEvent:UnityEvent {}
    
    public RectTransform Background;
    public RectTransform Knob;
    [Header("Input Values")]
    public float Horizontal = 0;
    public float Vertical = 0;

    public MoveStartEvent OnStart;
    public MoveIngEvent OnMove;
    public MoveEndEvent OnEnd;


    public float offset;
    Vector2 PointPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    /// <summary>
    /// 计算公式来源:http://www.theappguruz.com/blog/beginners-guide-learn-to-make-simple-virtual-joystick-in-unity
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle
        (Background, 
            eventData.position,
            eventData.pressEventCamera,
            out var position);
        
        position.x = (position.x/Background.sizeDelta.x);
        position.y = (position.y/Background.sizeDelta.y);
        
        float x = (Knob.pivot.x == 1f) ? position.x *2 + 1 : position.x *2 - 1;
        float y = (Knob.pivot.y == 1f) ? position.y *2 + 1 : position.y *2 - 1;
            
        PointPosition = new Vector3 (x,y);
        PointPosition = (PointPosition.magnitude > 1) ? PointPosition.normalized : PointPosition;

        OnMove.Invoke(PointPosition);
    }

    private void _updateKnobOps()
    {
        Knob.anchoredPosition = new Vector3(PointPosition.x * (Background.sizeDelta.x / (1 + offset))
            , PointPosition.y * (Background.sizeDelta.y) / (1 + offset));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        PointPosition = new Vector2(0f,0f);
        Knob.anchoredPosition = Vector2.zero;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
        OnStart.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData) {
        OnEndDrag(eventData);
        OnEnd.Invoke();
    }
   
	
	// Update is called once per frame
	void Update ()
    {
        _inputAxis();
        Horizontal = PointPosition.x;
        Vertical = PointPosition.y;
        _updateKnobOps();
    }

    private bool _start = false;
    private void _inputAxis()
    {
        if (SynAxis)
        {
            if ((int) Input.GetAxisRaw("Horizontal") == 0 && (int) Input.GetAxisRaw("Vertical") == 0)
            {
                if (_start)
                {
                    OnEnd.Invoke();

                    _start = false;
                }

                return;
            }

            if (!_start)
            {
                OnStart.Invoke();
                _start = true;
            }

            PointPosition = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
            if ((int) Input.GetAxisRaw("Horizontal") != 0 || (int) Input.GetAxisRaw("Vertical") != 0)
            {
                OnMove.Invoke(PointPosition);   
            }
        }
    }

    public Vector2 Coordinate()
    {
	return new Vector2(Horizontal,Vertical);
    }
}
