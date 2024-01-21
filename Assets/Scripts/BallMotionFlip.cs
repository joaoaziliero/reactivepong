using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BallMotionFlip : MonoBehaviour
{
    public Func<(int x, int y)> VectorModifier { get; }

    [Header("Vector flip factors")]
    [SerializeField] private int xFactor;
    [SerializeField] private int yFactor;

    private BallMotionFlip()
    {
        VectorModifier = () => (xFactor, yFactor);
    }
}
