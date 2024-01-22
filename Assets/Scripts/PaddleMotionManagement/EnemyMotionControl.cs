using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Linq;

public class EnemyMotionControl : MonoBehaviour
{
    private CompositeDisposable _compositeDisposable;

    [Header("Gameplay parameters")]
    [SerializeField] private GameplayParameters _parameters;
    [Header("Ball object reference")]
    [SerializeField] private Transform _ballTransform;

    private void Awake()
    {
        if (_compositeDisposable != null)
        {
            _compositeDisposable.Dispose();
        }

        _compositeDisposable = new CompositeDisposable();
    }

    private void Start()
    {
        ManageEnemyPosition(_ballTransform, transform, _parameters.EnemyPaddleSpeed.Invoke())
            .AddTo(_compositeDisposable);
    }

    private IDisposable ManageEnemyPosition(Transform ballTransform, Transform thisTransform, float paddleSpeed)
    {
        return Observable
            .EveryUpdate()
            .Select<long, Action>(_ =>
            {
                var currentPos = thisTransform.position;
                var y = Mathf.Clamp(ballTransform.position.y, -4.5f, +4.5f);
                var target = new Vector2(currentPos.x, y);
                return () => { thisTransform.position = Vector2.MoveTowards(currentPos, target, paddleSpeed * Time.deltaTime); };
            })
            .Subscribe(action => action.Invoke());
    }
}
