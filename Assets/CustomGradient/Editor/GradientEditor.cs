using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CustomGradient))]
public class GradientEditor : EditorWindow
{
    static int borderSize = 10;
    static int gradientTexHeight = 40;
    // static float keyControlSize = 10f;

    CustomGradient _gradient;
    Rect _gradientPreviewTexRect;
    List<ColorKeyControl> _keyControls = new List<ColorKeyControl>() {};
    ColorKeyControl _currentlySelected;

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
            Debug.Log(_gradient.ColorKeys.Count);
            _keyControls[0].IsMovable = false;
            _keyControls[_keyControls.Count - 1].IsMovable = false;
        }
    }

    void OnEnable() 
    {
        titleContent.text = "Gradient Editor";
    }

    void OnDisable() 
    {
        _keyControls.Clear();
    }

    void OnGUI()
    {
        Event guiEvent = Event.current;
 
        DrawColorKeyControls();
        DrawGradientTexturePreview();

        ProcessGradientTexturePreviewEvents(guiEvent);
        ProcessKeyControlsEvents(guiEvent);
        ProcessKeybardEvents(guiEvent);

        if (GUI.changed) { Repaint(); }
    }

    void AddColorKey(Vector2 mousePosition)
    {
        float time = Mathf.InverseLerp(_gradientPreviewTexRect.xMin, _gradientPreviewTexRect.xMax, mousePosition.x);
        Color color = Random.ColorHSV();
        
        ColorKey newKey = _gradient.ColorKeys.Add(color, time);
        AddColorKeyControl(newKey);
    }

    void AddColorKeyControl(ColorKey key)
    {
        float xMin = _gradientPreviewTexRect.xMin;
        float xMax = _gradientPreviewTexRect.xMax;
        float y = _gradientPreviewTexRect.yMax;
        ColorKeyControl keyControl = new ColorKeyControl(xMin, xMax, y, key);
        
        _keyControls.Add(keyControl);
        MarkAsCurrentlySelected(keyControl);
    }

    void DrawGradientTexturePreview()
    {
        _gradientPreviewTexRect = new Rect(borderSize, borderSize, position.width - 2 * borderSize, gradientTexHeight);
        GUI.DrawTexture(_gradientPreviewTexRect, _gradient.GetTexture((int)_gradientPreviewTexRect.width));
    }

    void DrawColorKeyControls()
    {
        foreach (ColorKeyControl keyControl in _keyControls)
        {
            keyControl.Draw();
        }
        if (_currentlySelected != null)
        {
            _currentlySelected.DrawSelected();
        }
    }

    void ProcessKeyControlsEvents(Event guiEvent)
    {
        if (_keyControls != null)
        {
            foreach (var keyControl in _keyControls)
            {
                bool guiChanged = keyControl.ProcessEvents(guiEvent);

                if (guiChanged) 
                { 
                    if (keyControl.IsSelected)
                    {
                        MarkAsCurrentlySelected(keyControl);
                    }
                    int i = _gradient.ColorKeys.GetKeyIndex(keyControl.BoundKey);
                    
                    if (i > 0 && keyControl.IsMovable) { SwapIfNeeded(i); }

                    GUI.changed = true;
                }
            }
        }
    }

    void ProcessGradientTexturePreviewEvents(Event guiEvent)
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

    void ProcessKeybardEvents(Event guiEvent)
    {
        if (guiEvent.type == EventType.keyDown && guiEvent.keyCode == KeyCode.Backspace)
        {
            if (_currentlySelected.IsMovable)
            {
            _gradient.ColorKeys.Remove(_currentlySelected.BoundKey);
            _keyControls.Remove(_currentlySelected);
            _currentlySelected = null;
            GUI.changed = true;
            }
        }
    }

    void SwapIfNeeded(int i)
    {
        if (_gradient.ColorKeys[i].Time < _gradient.ColorKeys[i - 1].Time)
        {
            _gradient.ColorKeys.SwapKeys(i - 1, i);
        }
        if (_gradient.ColorKeys[i].Time > _gradient.ColorKeys[i + 1].Time)
        {
            _gradient.ColorKeys.SwapKeys(i + 1, i);
        }
    }

    void MarkAsCurrentlySelected(ColorKeyControl keyControl)
    {
        if (_currentlySelected == null)
        {
            _currentlySelected = keyControl;
        }
        else 
        {
            if (_currentlySelected != keyControl)
            {
                _currentlySelected.IsSelected = false;
                _currentlySelected = keyControl;
            }
        }
    }
}