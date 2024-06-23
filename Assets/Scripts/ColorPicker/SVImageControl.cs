using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SVImageControl : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    [SerializeField] private Image _pickerImage;
    private RawImage _svImage;
    private ColorPickerControl _colorPickerControl;
    private RectTransform _rectTransform;
    private RectTransform _pickerTransform;

    //Unity calls Awake when an enabled script instance is being loaded.
    private void Awake()
    {
        _svImage = GetComponent<RawImage>();
        _colorPickerControl = FindObjectOfType<ColorPickerControl>();
        _rectTransform = GetComponent<RectTransform>();

        _pickerTransform = _pickerImage.GetComponent<RectTransform>();
        _pickerTransform.position = new Vector2(-(_rectTransform.sizeDelta.x * 0.5f), -(_rectTransform.sizeDelta.y * 0.5f));
    }

    /// <summary>
    /// Sends picker values over to the color controller script.
    /// </summary>
    /// <param name="eventData">Data relating to any pointer event.</param>
    private void UpdateColor(PointerEventData eventData)
    {
        Vector3 pos = _rectTransform.InverseTransformPoint(eventData.position);

        float deltaX = _rectTransform.sizeDelta.x * 0.5f;
        float deltaY = _rectTransform.sizeDelta.y * 0.5f;
        //Clamp picker inside color picker
        if (pos.x < -deltaX) pos.x = -deltaX;
        else if(pos.x > deltaX) pos.x = deltaX;
        if(pos.y < -deltaY) pos.y = -deltaY;
        else if(pos.y > deltaY) pos.y = deltaY;

        float x = pos.x + deltaX;
        float y = pos.y + deltaY;

        float xNorm = x / _rectTransform.sizeDelta.x;
        float yNorm = y / _rectTransform.sizeDelta.y;

        _pickerTransform.localPosition = pos;
        _pickerImage.color = Color.HSVToRGB(0, 0, 1 - yNorm);

        _colorPickerControl.SetSV(xNorm, yNorm);
    }

    /// <summary>
    /// Updates the color when the pointer is being dragged across the color picker.
    /// </summary>
    /// <param name="eventData">Data relating to any pointer event.</param>
    public void OnDrag(PointerEventData eventData)
    {
        UpdateColor(eventData);
    }

    /// <summary>
    /// Updates the color when the pointer clicks on the color picker.
    /// </summary>
    /// <param name="eventData">Data relating to any pointer event.</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        UpdateColor(eventData);
    }
}
