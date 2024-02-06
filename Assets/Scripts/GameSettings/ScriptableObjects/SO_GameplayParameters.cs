using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class SO_GameplayParameters : ScriptableObject
{
    public Func<Vector2> BallSpeed { get; }
    public Func<float> BallSpeedMultiplier { get; }
    public Func<float> PlayerPaddleSpeed { get; }
    public Func<string> LeftmostPlayerAxisName { get; }
    public Func<string> RightmostPlayerAxisName { get; }
    public Func<int> MaxPoints { get; }
    
    [Header("Ball speed")]
    [SerializeField] private Vector2 _ballSpeed;
    [Header("Ball speed multiplier (per collision)")]
    [SerializeField] private float _ballSpeedMultiplier;
    [Header("Player speed")]
    [SerializeField] private float _playerPaddleSpeed;
    [Header("Input axis names for each player")]
    [SerializeField] private string _leftmostPlayerAxisName;
    [SerializeField] private string _rightmostPlayerAxisName;
    [Header("Max. points before reset")]
    [SerializeField] private int _maxPoints;

    private SO_GameplayParameters()
    {
        BallSpeed = () => _ballSpeed;
        BallSpeedMultiplier = () => Mathf.Abs(_ballSpeedMultiplier);
        PlayerPaddleSpeed = () => Mathf.Abs(_playerPaddleSpeed);
        LeftmostPlayerAxisName = () => _leftmostPlayerAxisName;
        RightmostPlayerAxisName = () => _rightmostPlayerAxisName;
        MaxPoints = () => Mathf.Abs(_maxPoints);
    }
}
