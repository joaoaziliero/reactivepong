using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class ColorMemory : ScriptableObject
{
    [SerializeField] private string _leftPlayerTag;
    public Color leftPlayerColor = Color.white;
    [SerializeField] private string _rightPlayerTag;
    public Color rightPlayerColor = Color.white;

    public void SetColor(string playerTag, Color color)
    {
        if (playerTag == _leftPlayerTag)
        {
            leftPlayerColor = color;
        }
        else if (playerTag == _rightPlayerTag)
        {
            rightPlayerColor = color;
        }
    }

    public void ResetColors()
    {
        leftPlayerColor = Color.white;
        rightPlayerColor = Color.white;
    }
}
