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
            Input.GetAxis, _gameplayParameters.LeftmostPlayerAxisName.Invoke(),
            _compositeDisposable);
        ManageAxisInput(_rightmostPlayerTransform,
            _gameplayParameters.PlayerPaddleSpeed.Invoke(),
            Input.GetAxis, _gameplayParameters.RightmostPlayerAxisName.Invoke(),
            _compositeDisposable);
    }

    private void ManageAxisInput(Transform playerPaddle, float paddleSpeed, Func<string, float> GetAxis, string AxisName, CompositeDisposable disposables)
    {
        Observable.EveryUpdate().Subscribe(_ => GetAxis(AxisName)).AddTo(disposables);

        GetAxis.ObserveEveryValueChanged(InputFunction => Time.deltaTime * InputFunction(AxisName))
            .Select<float, Action>(processedInput =>
            {
                var currentPos = playerPaddle.position;
                var y = Mathf.Clamp(currentPos.y + paddleSpeed * processedInput, -4.5f, +4.5f);
                return () => { playerPaddle.position = new Vector3(currentPos.x, y, 0); };
            })
            .Subscribe(action => action.Invoke()).AddTo(disposables);
    }

    private void OnDestroy()
    {
        _compositeDisposable.Dispose();
    }
}
