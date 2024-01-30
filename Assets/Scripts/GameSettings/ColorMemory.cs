using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class ColorMemory : ScriptableObject
{
    [SerializeField] private string _leftPlayerTag;
    public Color _leftPlayerColor = Color.white;
    [SerializeField] private string _rightPlayerTag;
    public Color _rightPlayerColor = Color.white;

    public void SetColor(string playerTag, Color color)
    {
        if (playerTag == _leftPlayerTag)
        {
            _leftPlayerColor = color;
        }
        else if (playerTag == _rightPlayerTag)
        {
            _rightPlayerColor = color;
        }
    }

    public void ResetColors()
    {
        _leftPlayerColor = Color.white;
        _rightPlayerColor = Color.white;
    }
}
