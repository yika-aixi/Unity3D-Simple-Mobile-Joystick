//

using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent(typeof(UnityEngine.UI.AspectRatioFitter))]
public class MobileInputController : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler,IPointerDownHandler,IPointerUpHandler {

    public RectTransform Background;
    public RectTransform Knob;
    [Header("Input Values")]
    public float Horizontal = 0;
    public float Vertical = 0;


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
        
        Knob.anchoredPosition = new Vector3 (PointPosition.x * (Background.sizeDelta.x/ (1 + offset))
            ,PointPosition.y * (Background.sizeDelta.y)/(1 + offset));

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        PointPosition = new Vector2(0f,0f);
        Knob.anchoredPosition = Vector2.zero;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
       
    }

    public void OnPointerUp(PointerEventData eventData) {
        OnEndDrag(eventData);
    }
   
	
	// Update is called once per frame
	void Update () {
        Horizontal = PointPosition.x;
        Vertical = PointPosition.y;
    }

    public Vector2 Coordinate()
    {
	return new Vector2(Horizontal,Vertical);
    }
}
