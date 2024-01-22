using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using System.Linq;

public class ScoreManager : MonoBehaviour
{
    private CompositeDisposable _compositeDisposable;
    private Collider2D _collider;

    [Header("Gameplay parameters")]
    [SerializeField] GameplayParameters _parameters;

    private void Awake()
    {
        if (_compositeDisposable != null)
        {
            _compositeDisposable.Dispose();
        }

        _compositeDisposable = new CompositeDisposable();
        _collider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        TrackScore(_collider, _parameters.MaxPoints.Invoke()).AddTo(_compositeDisposable);
    }

    private IDisposable TrackScore(Collider2D thisCollider, int maxPoints)
    {
        GameObject ball = null;

        var obs = thisCollider.OnTriggerEnter2DAsObservable()
            .Where(thatCollider => thatCollider.gameObject.CompareTag("Ball"));

        obs.Subscribe(thatCollider => { ball = thatCollider.gameObject; });

        return obs.Select(collision => 1)
            .Scan(0, (acc, current) => acc + current)
            .Select<int, Action>(totalScore =>
            {
                return totalScore switch
                {
                    var value when value == maxPoints => () => { }
                    ,
                    _ => () => { }
                    ,
                };
            })
            .Subscribe();
    }

    private void OnDestroy()
    {
        _compositeDisposable.Dispose();
    }
}
