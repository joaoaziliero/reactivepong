using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;
using System.Linq;

public class BallMotion : MonoBehaviour
{
    private Collider2D _collider;
    private Rigidbody2D _rigidBody;

    [SerializeField]
    [Header("Gameplay Parameters")]
    private GameplayParameters _parameters;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _rigidBody = GetComponent<Rigidbody2D>();

        _rigidBody.velocity = _parameters.BallSpeed.Invoke();
    }

    private void Start()
    {
    }
}
