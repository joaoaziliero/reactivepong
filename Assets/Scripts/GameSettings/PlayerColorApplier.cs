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

    private void Awake()
    {
        _leftSprite.color = _colorMemory.leftPlayerColor;
        _rightSprite.color = _colorMemory.rightPlayerColor;
    }
}
