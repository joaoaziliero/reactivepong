using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class GameplayParameters : ScriptableObject
{
    public Func<Vector2> BallSpeed { get; }
    
    [Header("Ball speed")]
    [SerializeField] private Vector2 _ballSpeed;

    private GameplayParameters()
    {
        BallSpeed = () => _ballSpeed;
    }
}
