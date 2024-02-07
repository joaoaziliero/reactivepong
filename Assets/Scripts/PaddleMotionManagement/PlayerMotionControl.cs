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
    [SerializeField] private Rigidbody2D _leftmostPaddleBody;
    [SerializeField] private Rigidbody2D _rightmostPaddleBody;

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
        ManageAxisInput(_leftmostPaddleBody,
            _gameplayParameters.PlayerPaddleSpeed.Invoke(),
            Input.GetAxis, _gameplayParameters.LeftmostPlayerAxisName.Invoke(),
            _compositeDisposable);
        ManageAxisInput(_rightmostPaddleBody,
            _gameplayParameters.PlayerPaddleSpeed.Invoke(),
            Input.GetAxis, _gameplayParameters.RightmostPlayerAxisName.Invoke(),
            _compositeDisposable);
    }

    private void ManageAxisInput(Rigidbody2D playerPaddle, float paddleSpeed, Func<string, float> GetAxis, string AxisName, CompositeDisposable disposables)
    {
        Observable.EveryUpdate().Subscribe(_ => GetAxis(AxisName)).AddTo(disposables);

        GetAxis.ObserveEveryValueChanged(InputFunction => InputFunction(AxisName))
            .Select<float, Action>(inputReading => () => { playerPaddle.velocity = inputReading * paddleSpeed * Vector2.up; })
            .Subscribe(action => action.Invoke()).AddTo(disposables);
    }

    private void OnDestroy()
    {
        _compositeDisposable.Dispose();
    }
}
