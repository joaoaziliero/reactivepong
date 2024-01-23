using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private GameplayParameters _parameters;
    [Header("Corresponding display field")]
    [SerializeField] private TextMeshProUGUI _textField;
    [Header("Opponent's display field")]
    [SerializeField] private TextMeshProUGUI _opposingTextField;

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
        TrackScore(_collider, _parameters.MaxPoints.Invoke(), _textField, _opposingTextField)
            .AddTo(_compositeDisposable);
    }

    private IDisposable TrackScore(Collider2D thisCollider, int maxPoints, TextMeshProUGUI textField, TextMeshProUGUI opposingField)
    {
        return thisCollider
            .OnTriggerEnter2DAsObservable()
            .Where(thatCollider => thatCollider.gameObject.CompareTag("Ball"))
            .Select<Collider2D, Action>(collision =>
            {
                var score = int.Parse(textField.text);
                var opposingScore = int.Parse(opposingField.text);

                if (score < maxPoints && opposingScore < maxPoints)
                {
                    return () =>
                    {
                        var newScore = score + 1;
                        textField.text = newScore.ToString();
                        if (newScore == maxPoints) opposingField.text = 0.ToString();
                    };
                }
                else
                {
                    return () => { textField.text = 1.ToString(); opposingField.text = 0.ToString(); };
                }
            })
            .Subscribe(action => action.Invoke());
    }

    private void OnDestroy()
    {
        _compositeDisposable.Dispose();
    }
}
