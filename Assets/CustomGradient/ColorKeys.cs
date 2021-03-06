using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorKeys: IEnumerable
{
    List<ColorKey> _colorKeys = new List<ColorKey>() {};

    public ColorKey this[int i]
    {
        get { return _colorKeys[i]; }
    }

    public int Count
    {
        get { return _colorKeys.Count; }
    }

    public ColorKey Get(int index)
    {
        return _colorKeys[index];
    }

    public ColorKey Add(Color color, float time)
    {
        ColorKey newKey = new ColorKey(color, time);
        bool isBoundary = true;

        for (int i = 0; i < _colorKeys.Count; i++)
        {
            if (time < _colorKeys[i].Time)
            {
                _colorKeys.Insert(i, newKey);
                isBoundary = false;
                break;
            }   
        }
        if (isBoundary) { _colorKeys.Add(newKey); }
        
        return newKey;
    }

    public void Remove(ColorKey key)
    {
        _colorKeys.RemoveAt(key.Index);
    } 

    public int GetKeyIndex(ColorKey key)
    {
        return _colorKeys.IndexOf(key);
    }

    public void SwapKeys(int indexOfA, int indexOfB)
    {
        ColorKey temp = _colorKeys[indexOfA];
        _colorKeys[indexOfA] = _colorKeys[indexOfB];
        _colorKeys[indexOfB] = temp;
    }

    public void SortByTime()
    {
        _colorKeys.Sort((a, b) => a.Time.CompareTo(b.Time));
    }

    public void Clear()
    {
        _colorKeys.Clear();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumertor();
    }    

    public IEnumerator GetEnumertor()
    {
        return _colorKeys.GetEnumerator();
    }
}

[System.Serializable]
public class ColorKey
{
    [SerializeField]
    Color _color;
    [SerializeField]
    float _time;
    
    public int Index;

    public ColorKey(Color color, float time)
    {
        _color = color;
        _time = time;
    }

    public Color Color
    {
        get { return _color; }
        set { _color = value; }
    }
    public float Time
    {
        get { return _time; }
        set { _time = Mathf.Clamp01(value); }
    }
}