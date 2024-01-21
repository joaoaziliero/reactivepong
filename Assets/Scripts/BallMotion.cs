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
        ManageCollisions(_collider, _rigidBody, _parameters.BallSpeed.Invoke(), _parameters.BallSM.Invoke())
            .AddTo(_compositeDisposable);
    }

    private IDisposable ManageCollisions(Collider2D col, Rigidbody2D rb, Vector2 ballSpeed, float speedMultiplier)
    {
        return col.OnTriggerEnter2DAsObservable()
            .Select(trigger => trigger.gameObject.GetComponent<BallMotionFlip>())
            .Where(component => component != null)
            .Select(ballFlip => ballFlip.VectorModifier.Invoke())
            .Select<(int, int), Action>(modifier =>
            {
                var absVel = new Vector2(Mathf.Abs(rb.velocity.x), Mathf.Abs(rb.velocity.y));
                var unitVel = new Vector2(Mathf.Sign(rb.velocity.x), Mathf.Sign(rb.velocity.y));
                var unitPos = new Vector2(Mathf.Sign(rb.position.x), Mathf.Sign(rb.position.y));

                return modifier switch
                {
                    (+1, -1) => () => { rb.velocity = speedMultiplier * new Vector2((+1) * unitVel.x * absVel.x, (-1) * unitPos.y * absVel.y); }
                    ,
                    (-1, +1) => () => { rb.velocity = speedMultiplier * new Vector2((-1) * unitPos.x * absVel.x, (+1) * unitVel.y * absVel.y); }
                    ,
                    _ => () => { rb.velocity = ballSpeed; rb.position = Vector2.zero; }
                    ,
                };
            })
            .Subscribe(action => action.Invoke());
    }

    private void OnDestroy()
    {
        _compositeDisposable.Dispose();
    }
}
