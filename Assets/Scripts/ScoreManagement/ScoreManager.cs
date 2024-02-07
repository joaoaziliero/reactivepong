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

    [SerializeField] private SO_GameplayParameters _gameplayParameters;
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
        ManageScore(_collider, _opposingTextField, _textField, _gameplayParameters.MaxPoints.Invoke())
            .AddTo(_compositeDisposable);
    }

    private IDisposable ManageScore(Collider2D trigger, TextMeshProUGUI textField, TextMeshProUGUI opposingField, int maxPoints)
    {
        return trigger
            .OnTriggerEnter2DAsObservable()
            .Where(incomingTrigger => incomingTrigger.gameObject.CompareTag("Ball"))
            .Select<Collider2D, Action>(collision =>
            {
                var score = int.Parse(textField.text);
                var opposingScore = int.Parse(opposingField.text);

                return (score, opposingScore) switch
                {
                    var tuple when tuple.score == maxPoints => () => ResetScore(textField, opposingField)
                    ,
                    var tuple when tuple.opposingScore == maxPoints => () => ResetScore(textField, opposingField)
                    ,
                    _ => () => UpdateScore(textField, score + 1)
                    ,
                };
            })
            .Subscribe(action => action.Invoke());
    }

    private readonly Action<TextMeshProUGUI, TextMeshProUGUI> ResetScore =
        (textField, opposingField) => { textField.text = 1.ToString(); opposingField.text = 0.ToString(); };

    private readonly Action<TextMeshProUGUI, int> UpdateScore =
        (textField, newScore) => { textField.text = newScore.ToString(); };

    private void OnDestroy()
    {
        _compositeDisposable.Dispose();
    }
}
