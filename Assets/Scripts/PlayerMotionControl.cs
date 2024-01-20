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
        ManageAxisInput(transform, _parameters.PlayerPaddleSpeed.Invoke()).AddTo(_compositeDisposable);
    }

    private IDisposable ManageAxisInput(Transform playerPaddle, float playerPaddleSpeed)
    {
        return Observable
            .EveryUpdate()
            .Subscribe(_ =>
            {
                var updatedPosition =
                    playerPaddle.position
                    + Input.GetAxis("Vertical") * playerPaddleSpeed * Time.deltaTime * Vector3.up;
                updatedPosition.y = Mathf.Clamp(updatedPosition.y, -4.5f, +4.5f);

                playerPaddle.position = updatedPosition;
            });
    }

    private void OnDestroy()
    {
        _compositeDisposable.Dispose();
    }
}
