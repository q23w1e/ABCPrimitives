using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CustomGradient))]
public class GradientEditor : EditorWindow
{
    static int borderSize = 10;
    static int gradientTexHeight = 40;
    static float keyControlSize = 10f;

    CustomGradient _gradient;
    ColorKeys _colorKeys;
    Rect _gradientPreviewTexRect;
    List<ColorKeyControl> _keyControls = new List<ColorKeyControl>() {};

    public CustomGradient Gradient
    {
        set 
        { 
            _gradient = value;
            _gradientPreviewTexRect = new Rect(borderSize, borderSize, position.width - 2 * borderSize, gradientTexHeight);
            foreach (ColorKey key in _gradient.ColorKeys)
            {
                AddColorKeyControl(key);
            }
            _keyControls[0].IsMovable = false;
            _keyControls[_keyControls.Count - 1].IsMovable = false;
        }
    }

    void OnEnable() 
    {
        titleContent.text = "Gradient Editor";
    }

    void OnGUI()
    {
        Event guiEvent = Event.current;
 
        DrawColorKeyControls();
        DrawGradientTexturePreview();

        ProcessGradientTexturePreviewEvents(guiEvent);
        ProcessKeyControlsEvents(guiEvent);

        if (GUI.changed) { Repaint(); }
    }

    void AddColorKey(Vector2 mousePosition)
    {
        float time = Mathf.InverseLerp(_gradientPreviewTexRect.xMin, _gradientPreviewTexRect.xMax, mousePosition.x);
        Color color = Random.ColorHSV();
        ColorKey key = new ColorKey(color, time);
        _gradient.ColorKeys.Add(color, time);
        AddColorKeyControl(key);
    }

    void AddColorKeyControl(ColorKey key)
    {
        float xMin = _gradientPreviewTexRect.xMin;
        float xMax = _gradientPreviewTexRect.xMax - borderSize;
        float y = _gradientPreviewTexRect.yMax;
        ColorKeyControl keyControl = new ColorKeyControl(xMin, xMax, y, key);
        _keyControls.Add(keyControl);
    }

    void DrawGradientTexturePreview()
    {
        _gradientPreviewTexRect = new Rect(borderSize, borderSize, position.width - 2 * borderSize, gradientTexHeight);
        GUI.DrawTexture(_gradientPreviewTexRect, _gradient.GetTexture((int)_gradientPreviewTexRect.width));
    }

    void DrawColorKeyControls()
    {
        Debug.Log(_keyControls.Count);
        foreach (ColorKeyControl keyControl in _keyControls)
        {
            keyControl.Draw();
        }
    }

    void ProcessKeyControlsEvents(Event guiEvent)
    {
        if (_keyControls != null)
        {
            foreach (var key in _keyControls)
            {
                bool guiChanged = key.processEvents(guiEvent);

                if (guiChanged) 
                { 
                    _gradient.ColorKeys.SortByTime();
                    GUI.changed = true;
                }
            }
        }
    }

    void ProcessGradientTexturePreviewEvents(Event guiEvent)
    {
        if (_gradientPreviewTexRect != null)
        {
            if (guiEvent.type == EventType.mouseDown && guiEvent.button == 0)
            {
                if (_gradientPreviewTexRect.Contains(guiEvent.mousePosition))
                {
                    AddColorKey(guiEvent.mousePosition);
                    GUI.changed = true;
                }
            }
        }
    }
}

public class ColorKeyControl
{
    static int size = 10;
    
    float _xMin;
    float _xMax;
    Rect _rect;
    ColorKey _boundKey;
    bool _isDragged = false;
    
    public bool IsSelected = false;
    public bool IsMovable = true;

    public ColorKeyControl(float xMin, float xMax, float y, ColorKey key)
    {
        _xMin = xMin;
        _xMax = xMax;
        _boundKey = key;
        float positionX = Mathf.Lerp(xMin, xMax, key.Time);
        float offset = size * 0.5f;
        _rect = new Rect(positionX - offset, y + offset, size, size * 2f);
    }

    void DragAlongX(float deltaX)
    {
        _rect.x += deltaX;
        _rect.x = Mathf.Clamp(_rect.x, _xMin, _xMax);
        _boundKey.Time = Mathf.InverseLerp(_xMin, _xMax, _rect.x);
    }

    public void Draw()
    {
        EditorGUI.DrawRect(_rect, _boundKey.Color);
    }

    public bool processEvents(Event guiEvent)
    {
        switch (guiEvent.type)
        {
            case EventType.MouseDown:
                if (guiEvent.button == 0)
                {
                    if (_rect.Contains(guiEvent.mousePosition))
                    {
                        _isDragged = true;
                        GUI.changed = true;
                    }
                }
                break;
            case EventType.mouseDrag:
                if (guiEvent.button == 0 && _isDragged && IsMovable)
                {
                    DragAlongX(guiEvent.delta.x);
                    guiEvent.Use();
                 
                    return true;
                }
                break;
            case EventType.mouseUp:
                _isDragged = false;
                break;
        }

        return false;
    }

}