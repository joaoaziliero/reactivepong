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
        ManageCollisions(_collider, _rigidBody)
            .AddTo(_compositeDisposable);
        ManageTeleport(_collider, _rigidBody)
            .AddTo(_compositeDisposable);
    }

    private IDisposable ManageCollisions(Collider2D trigger, Rigidbody2D rb)
    {
        return trigger.OnTriggerEnter2DAsObservable()
            .Where(collider => !collider.isTrigger)
            .Select(collider => Mathf.Abs(collider.transform.rotation.eulerAngles.z))
            .Select<float, Action>(orthogonality =>
            {
                var unitPos = new Vector2(Mathf.Sign(rb.position.x), Mathf.Sign(rb.position.y));

                return orthogonality switch
                {
                    0 => () => { rb.velocity = new Vector2(rb.velocity.x, (-1) * unitPos.y * Mathf.Abs(rb.velocity.y)); }
                    ,
                    90 => () => { rb.velocity = new Vector2((-1) * unitPos.x * Mathf.Abs(rb.velocity.x), rb.velocity.y); }
                    ,
                    _ => () => { Debug.Log("Z-axis rotation values are expected to be either 0 or 90 degrees"); }
                    ,
                };
            })
            .Subscribe(action => action.Invoke());
    }

    private IDisposable ManageTeleport(Collider2D trigger, Rigidbody2D rb)
    {
        return trigger.OnTriggerEnter2DAsObservable()
            .Where(collider => collider.isTrigger)
            .AsUnitObservable()
            .Select<Unit, Action>(_ => () => { rb.position = Vector2.zero; })
            .Subscribe(action => action.Invoke());
    }

    private void OnDestroy()
    {
        _compositeDisposable.Dispose();
    }
}
