using UnityEngine;
using UnityEditor;

public class ColorKeyControl
{
    static int size = 10;
    
    float _xMin;
    float _xMax;
    Rect _rect;
    bool _isDragged = false;
    
    public ColorKey BoundKey;
    public bool IsSelected = false;
    public bool IsMovable = true;

    public ColorKeyControl(float xMin, float xMax, float y, ColorKey key)
    {
        _xMin = xMin;
        _xMax = xMax;
        BoundKey = key;
        float positionX = Mathf.Lerp(xMin, xMax, key.Time);
        float offset = size * 0.5f;
        _rect = new Rect(positionX - offset, y + offset, size, size * 2f);
    }

    void DragAlongX(float deltaX)
    {
        _rect.x += deltaX;
        _rect.x = Mathf.Clamp(_rect.x, _xMin, _xMax);
        BoundKey.Time = Mathf.InverseLerp(_xMin, _xMax, _rect.x);
    }

    public void Draw()
    {
        EditorGUI.DrawRect(_rect, BoundKey.Color);
    }

    public void DrawSelected()
    {
        Rect rect = new Rect(_rect);
        rect.y += rect.height + 3;
        rect.height = size * 0.25f;
        EditorGUI.DrawRect(rect, Color.black);
    }

    public bool ProcessEvents(Event guiEvent)
    {
        switch (guiEvent.type)
        {
            case EventType.MouseDown:
                if (guiEvent.button == 0)
                {
                    if (_rect.Contains(guiEvent.mousePosition))
                    {
                        IsSelected = true;
                        _isDragged = true;
                        
                        return true;
                    }
                }
                break;
            case EventType.mouseDrag:
                if (guiEvent.button == 0 && _isDragged && IsMovable)
                {
                    DragAlongX(guiEvent.delta.x);
                    // guiEvent.Use();
                 
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