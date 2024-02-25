using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using System.Linq;

public class BallMotionControl : MonoBehaviour
{
    private CompositeDisposable _compositeDisposable;
    private Collider2D _collider;
    private Rigidbody2D _rigidBody;

    [SerializeField] private SO_GameplayParameters _gameplayParameters;

    private void Awake()
    {
        if (_compositeDisposable != null)
        {
            _compositeDisposable.Dispose();
        }

        _compositeDisposable = new CompositeDisposable();
        _collider = GetComponent<Collider2D>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.velocity = _gameplayParameters.BallSpeed.Invoke();
    }

    private void Start()
    {
        ManageCollisions(_collider, _rigidBody, _gameplayParameters.BallSpeed.Invoke(), _gameplayParameters.BallSpeedMultiplier.Invoke())
            .AddTo(_compositeDisposable);
    }

    private IDisposable ManageCollisions(Collider2D trigger, Rigidbody2D rb, Vector2 ballSpeed, float speedMultiplier)
    {
        return trigger.OnTriggerEnter2DAsObservable()
            .Select<Collider2D, Action>(collider =>
            {
                return collider switch
                {
                    var col when col.isTrigger => () => Teleport(rb, ballSpeed)
                    ,
                    var col when !col.isTrigger && col.transform.rotation.eulerAngles.z == 0 => () => ReflectVerticalVelocity(rb, speedMultiplier)
                    ,
                    var col when !col.isTrigger && col.transform.rotation.eulerAngles.z == 90 => () => ReflectHorizontalVelocity(rb, speedMultiplier)
                    ,
                    _ => null
                    ,
                };
            })
            .Where(action => action != null)
            .Subscribe(action => action.Invoke());
    }

    private readonly Action<Rigidbody2D, Vector2> Teleport =
        (rb, ballSpeed) =>
        {
            rb.position = Vector2.zero;
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * Mathf.Abs(ballSpeed.x), Mathf.Sign(rb.velocity.y) * Mathf.Abs(ballSpeed.y));
        };

    private readonly Action<Rigidbody2D, float> ReflectVerticalVelocity =
        (rb, speedMultiplier) =>
        {
            var unitPosY = Mathf.Sign(rb.position.y);
            rb.velocity = speedMultiplier * new Vector2(rb.velocity.x, (-1) * unitPosY * Mathf.Abs(rb.velocity.y));
        };

    private readonly Action<Rigidbody2D, float> ReflectHorizontalVelocity =
        (rb, speedMultiplier) =>
        {
            var unitPosX = Mathf.Sign(rb.position.x);
            rb.velocity = speedMultiplier * new Vector2((-1) * unitPosX * Mathf.Abs(rb.velocity.x), rb.velocity.y);
        };

    private void OnDestroy()
    {
        _compositeDisposable.Dispose();
    }
}
