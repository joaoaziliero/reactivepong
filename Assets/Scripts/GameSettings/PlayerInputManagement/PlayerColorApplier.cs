using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColorApplier : MonoBehaviour
{
    [Header("Color choice record")]
    [SerializeField] private ColorMemory _colorMemory;
    [Header("Left player sprite reference")]
    [SerializeField] private SpriteRenderer _leftSprite;
    [Header("Right player sprite reference")]
    [SerializeField] private SpriteRenderer _rightSprite;

    private void Start()
    {
        if (_colorMemory.leftPlayerColor != Color.clear)
        {
            _leftSprite.color = _colorMemory.leftPlayerColor;
        }
        else
        {
            _leftSprite.color = Color.white;
        }

        if (_colorMemory.rightPlayerColor != Color.clear)
        {
            _rightSprite.color = _colorMemory.rightPlayerColor;
        }
        else
        {
            _rightSprite.color = Color.white;
        }
    }
}
