using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Linq;

public class PlayerMotionControl : MonoBehaviour
{
    private CompositeDisposable _compositeDisposable;

    [SerializeField] private SO_GameplayParameters _gameplayParameters;
    [SerializeField] private Transform _leftmostPlayerTransform;
    [SerializeField] private Transform _rightmostPlayerTransform;

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
        ManageAxisInput(_leftmostPlayerTransform,
            _gameplayParameters.PlayerPaddleSpeed.Invoke(),
            Input.GetAxis, _gameplayParameters.LeftmostPlayerAxisName.Invoke())
            .AddTo(_compositeDisposable);
        ManageAxisInput(_rightmostPlayerTransform,
            _gameplayParameters.PlayerPaddleSpeed.Invoke(),
            Input.GetAxis, _gameplayParameters.RightmostPlayerAxisName.Invoke())
            .AddTo(_compositeDisposable);
    }

    private IDisposable ManageAxisInput(Transform playerPaddle, float paddleSpeed, Func<string, float> GetAxis, string AxisName)
    {
        return Observable
            .EveryUpdate()
            .AsUnitObservable()
            .Select<Unit, Action>(_ =>
            {
                var currentPos = playerPaddle.position;
                var y = Mathf.Clamp(currentPos.y + paddleSpeed * Time.deltaTime * GetAxis(AxisName), -4.5f, +4.5f);
                return () => { playerPaddle.position = new Vector3(currentPos.x, y, 0); };
            })
            .Subscribe(action => action.Invoke());
    }

    private void OnDestroy()
    {
        _compositeDisposable.Dispose();
    }
}
