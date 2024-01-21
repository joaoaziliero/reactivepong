using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class GameplayParameters : ScriptableObject
{
    public Func<Vector2> BallSpeed { get; }
    public Func<float> BallSM { get; }
    public Func<float> PlayerPaddleSpeed { get; }
    
    [Header("Ball speed")]
    [SerializeField] private Vector2 _ballSpeed;
    [Header("Ball speed multiplier")]
    [SerializeField] private float _ballSM;
    [Header("Player speed")]
    [SerializeField] private float _playerPaddleSpeed;

    private GameplayParameters()
    {
        BallSpeed = () => new Vector2(Mathf.Abs(_ballSpeed.x), Mathf.Abs(_ballSpeed.y));
        BallSM = () => _ballSM;
        PlayerPaddleSpeed = () => Mathf.Abs(_playerPaddleSpeed);
    }
}
