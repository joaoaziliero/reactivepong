using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class PlayerMotionControl : MonoBehaviour
{
    private CompositeDisposable _compositeDisposable;

    [Header("Gameplay parameters")]
    [SerializeField] private GameplayParameters _parameters;

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
        ManageAxisInput(transform, _parameters.PlayerPaddleSpeed.Invoke(), () => Input.GetAxis("Vertical"))
            .AddTo(_compositeDisposable);
    }

    private IDisposable ManageAxisInput(Transform playerPaddle, float paddleSpeed, Func<float> AxisInput)
    {
        return Observable
            .EveryUpdate()
            .Select<long, Action>(_ =>
            {
                var currentPos = playerPaddle.position;
                var y = Mathf.Clamp(currentPos.y + AxisInput.Invoke() * paddleSpeed * Time.deltaTime, -4.5f, +4.5f);
                return () => { playerPaddle.position = new Vector3(currentPos.x, y, 0); };
            })
            .Subscribe(action => action.Invoke());
    }

    private void OnDestroy()
    {
        _compositeDisposable.Dispose();
    }
}
