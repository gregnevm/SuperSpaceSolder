using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickInputHandler : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private RectTransform _knob;
    [SerializeField] private RectTransform _backPanel;
    public Vector2 CurrentPosition { get; private set; }
    private Vector2 _defaultKnobPosition;

    private void Start()
    {
        _defaultKnobPosition = _knob.anchoredPosition;
    }
        
    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData); 
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ResetKnobPosition();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 backPanelPosition = RectTransformUtility.WorldToScreenPoint(null, _backPanel.position);
        Vector2 direction = (eventData.position - backPanelPosition).normalized;

        float distance = Vector2.Distance(eventData.position, backPanelPosition);
        float clampedDistance = Mathf.Clamp(distance, 0, _backPanel.sizeDelta.x * 0.5f); 

        _knob.anchoredPosition = _defaultKnobPosition + direction * clampedDistance;

        CurrentPosition = direction;
    }

    private void ResetKnobPosition()
    {
        _knob.anchoredPosition = _defaultKnobPosition;
        CurrentPosition = Vector2.zero;
    }
}
