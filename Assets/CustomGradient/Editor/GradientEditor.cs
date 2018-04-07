using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CustomGradient))]
public class GradientEditor : EditorWindow
{
    static int borderSize = 10;
    static int gradientTexHeight = 40;

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
            
            _keyControls.Clear();
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

    void OnDisable() 
    {
        _keyControls.Clear();
    }

    void OnGUI()
    {
        Event guiEvent = Event.current;
 
        DrawColorKeyControls();
        DrawGradientTexturePreview();
        DrawSettingsBlock();

        ProcessGradientTexturePreviewEvents(guiEvent);
        ProcessKeyControlsEvents(guiEvent);
        ProcessKeybardEvents(guiEvent);

        if (GUI.changed) 
        {
            Repaint();
        }
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
        Texture2D gradientTexture = _gradient.GetTexture();
        GUI.DrawTexture(_gradientPreviewTexRect, gradientTexture);
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

    void DrawSettingsBlock()
    {
        Rect rect = new Rect(borderSize, gradientTexHeight * 2 + borderSize, _gradientPreviewTexRect.width, _gradientPreviewTexRect.height * 2);
        
        GUILayout.BeginArea(rect);
        
        EditorGUI.BeginChangeCheck();
        Color selectedKeyColor = (_currentlySelected == null) ? Color.white : _currentlySelected.BoundKey.Color;
        Color color = EditorGUILayout.ColorField(selectedKeyColor);
        if (EditorGUI.EndChangeCheck())
        {
            _currentlySelected.BoundKey.Color = color;
            GUI.changed = true;
        }

        EditorGUI.BeginChangeCheck();
        InterpolationTypes selectedType = (InterpolationTypes)EditorGUILayout.EnumPopup(_gradient.InterpolationType);
        if (EditorGUI.EndChangeCheck())
        {
            _gradient.InterpolationType = selectedType;
            GUI.changed = true;
        }

        if (GUILayout.Button("Save as PNG"))
        {
            Texture2D texture = _gradient.GetTexture(64);
            byte[] bytes = texture.EncodeToPNG();
            string fileName = string.Format("Gradient{0}x{1}_{2}.png", texture.width, texture.height, _gradient.InterpolationType.ToString());
            string filePath = string.Format(@"{0}/{1}/", Application.dataPath, "Textures");
            
            if (!Directory.Exists(filePath)) { Directory.CreateDirectory(filePath); }
            File.WriteAllBytes(filePath + fileName, bytes);
        }
        
        GUILayout.EndArea();
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
        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0)
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
        if (guiEvent.type == EventType.KeyDown && guiEvent.keyCode == KeyCode.Backspace)
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