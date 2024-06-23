using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorPickerControl : MonoBehaviour
{

    public float CurrentHue;
    public float CurrentSaturation;
    public float CurrentValue;

    [SerializeField] private RawImage _hueImage;
    [SerializeField] private RawImage _SVImage;
    [SerializeField] private RawImage _outputImage;

    [SerializeField] private Slider _hueSlider;
    [SerializeField] private TMP_InputField _hexInputField;

    private Texture2D _hueTexture;
    private Texture2D _svTexture;
    private Texture2D _outputTexture;

    [SerializeField] private MeshRenderer _meshColorToChange;

    // Start is called before the first frame update
    private void Start()
    {
        CreateHueImage();
        CreateSVImage();
        CreateOutputImage();
        UpdateOuputImage();
    }

    /// <summary>
    /// Creates Hue Texture.
    /// </summary>
    private void CreateHueImage()
    {
        _hueTexture = new Texture2D(1, 16);
        _hueTexture.wrapMode = TextureWrapMode.Clamp;
        _hueTexture.name = "HueTexture";

        //Iterate over the pixels of the texture and base each pixel on its height.
        for(int i = 0; i < _hueTexture.height; i++)
        {
            _hueTexture.SetPixel(0, i, Color.HSVToRGB((float)i / _hueTexture.height, 1f, 1f));
        }
        _hueTexture.Apply();
        CurrentHue = 0;
        _hueImage.texture = _hueTexture;
    }

    /// <summary>
    /// Creates Saturation and Value Texture.
    /// </summary>
    private void CreateSVImage()
    {
        _svTexture = new Texture2D(16, 16);
        _svTexture.wrapMode = TextureWrapMode.Clamp;
        _svTexture.name = "SatValTexture";
        //Controls value
        for(int y = 0; y < _svTexture.height; y++)
        {
            //Controls saturation
            for(int x = 0; x < _svTexture.width; x++)
            {
                _svTexture.SetPixel(x, y, Color.HSVToRGB(CurrentHue, (float)x / _svTexture.width, (float)y / _svTexture.height));
            }
        }
        _svTexture.Apply();
        CurrentSaturation = 0;
        CurrentValue = 0;
        _SVImage.texture = _svTexture;
    }

    /// <summary>
    /// Creates Output Texture.
    /// </summary>
    private void CreateOutputImage()
    {
        _outputTexture = new Texture2D(1, 16);
        _outputTexture.wrapMode = TextureWrapMode.Clamp;
        _outputTexture.name = "OutputTexture";
        Color currentColor = Color.HSVToRGB(CurrentHue, CurrentSaturation, CurrentValue);
        //Create the pixels
        for(int i = 0; i < _outputTexture.height; i++)
        {
            _outputTexture.SetPixel(0, i, currentColor);
        }
        _outputTexture.Apply();
        _outputImage.texture = _outputTexture;
    }

    /// <summary>
    /// Updates Output Image.
    /// </summary>
    private void UpdateOuputImage()
    {
        Color currentColor = Color.HSVToRGB(CurrentHue, CurrentSaturation, CurrentValue);
        for (int i = 0; i < _outputTexture.height; i++)
        {
            _outputTexture.SetPixel(0, i, currentColor);
        }
        _outputTexture.Apply();
        _hexInputField.text = ColorUtility.ToHtmlStringRGB(currentColor);
        _meshColorToChange.material.color = currentColor;
    }

    /// <summary>
    /// Sets the saturation and value.
    /// </summary>
    public void SetSV(float S, float V)
    {
        CurrentSaturation = S;
        CurrentValue = V;
        UpdateOuputImage();
    }

    /// <summary>
    /// Updates the SV Image every time the slider value is changed.
    /// </summary>
    public void UpdateSVImage()
    {
        CurrentHue = _hueSlider.value;
        for(int y = 0; y < _svTexture.height; y++)
        {
            for(int x = 0; x < _svTexture.width; x++)
            {
                _svTexture.SetPixel(x, y, Color.HSVToRGB(CurrentHue, (float)x / _svTexture.width, (float)y / _svTexture.height));
            }
        }
        _svTexture.Apply();
        UpdateOuputImage();
    }

    /// <summary>
    /// Updates the SV Image using the hex color code.
    /// </summary>
    public void OnTextInput()
    {
        if (_hexInputField.text.Length < 6) return;
        Color newColor;
        if(ColorUtility.TryParseHtmlString("#" + _hexInputField.text, out newColor))
        {
            Color.RGBToHSV(newColor, out CurrentHue, out CurrentSaturation, out CurrentValue);
        }
        _hueSlider.value = CurrentHue;
        _hexInputField.text = "";
        UpdateOuputImage();
    }
}
