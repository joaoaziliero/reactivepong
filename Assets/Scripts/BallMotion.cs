using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using System.Linq;

public class BallMotion : MonoBehaviour
{
    private CompositeDisposable _compositeDisposable;
    private Collider2D _collider;
    private Rigidbody2D _rigidBody;

    [Header("Gameplay parameters")]
    [SerializeField] private GameplayParameters _parameters;

    private void Awake()
    {
        if (_compositeDisposable != null)
        {
            _compositeDisposable.Dispose();
        }

        _compositeDisposable = new CompositeDisposable();
        _collider = GetComponent<Collider2D>();
        _rigidBody = GetComponent<Rigidbody2D>();

        _rigidBody.velocity = _parameters.BallSpeed.Invoke();
    }

    private void Start()
    {
        ManageCollisions(_collider, _rigidBody, _parameters.BallSpeed.Invoke()).AddTo(_compositeDisposable);
    }

    private IDisposable ManageCollisions(Collider2D col, Rigidbody2D rb, Vector2 ballSpeed)
    {
        return col.OnCollisionEnter2DAsObservable()
            .Select(collision => collision.gameObject.GetComponent<BallMotionFlip>())
            .Where(component => component != null)
            .Select(ballFlip => ballFlip.VectorModifier.Invoke())
            .Select(modifier =>
            {
                var unitVel = new Vector2(Mathf.Sign(rb.velocity.x), Mathf.Sign(rb.velocity.y));
                var unitPos = new Vector2(Mathf.Sign(rb.position.x), Mathf.Sign(rb.position.y));

                return modifier switch
                {
                    (+1, -1) => new Vector2((+1) * unitVel.x * ballSpeed.x, (-1) * unitPos.y * ballSpeed.y),
                    (-1, +1) => new Vector2((-1) * unitPos.x * ballSpeed.x, (+1) * unitVel.y * ballSpeed.y),
                    _ => Vector2.zero,
                };
            })
            .Subscribe(updatedVector =>
            {
                if (updatedVector != Vector2.zero)
                {
                    rb.velocity = updatedVector;
                }
                else
                {
                    rb.position = updatedVector;
                    rb.velocity = ballSpeed;
                }
            });
    }

    private void OnDestroy()
    {
        _compositeDisposable.Dispose();
    }
}
