using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

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
        return thisCollider.OnTriggerEnter2DAsObservable()
            .Where(thatCollider => thatCollider.gameObject.CompareTag("Ball"))
            .Select(collision => 1)
            .Scan((id: 0, acc: 0), (tuple, increment) =>
            {
                var totalScore = tuple.acc + increment;

                return totalScore switch
                {
                    var value when value == maxPoints => (1, 0),
                    _ => (0, totalScore),
                };
            })
            .Select<(int id, int acc), Action>(tuple =>
            {
                return tuple.id switch
                {
                    1 => () =>
                    {
                        textField.text = maxPoints.ToString();
                        opposingField.text = (0).ToString();
                        StartCoroutine(ReloadGame());
                    }
                    ,
                    _ => () => { textField.text = tuple.acc.ToString(); }
                };
            })
            .Subscribe(action => action.Invoke());
    }

    private IEnumerator ReloadGame(string sceneName = "SCN_Main", float timeBeforeReload = 1)
    {
        yield return new WaitForSeconds(timeBeforeReload);
        SceneManager.LoadScene(sceneName);
    }

    private void OnDestroy()
    {
        StopCoroutine(ReloadGame());
        _compositeDisposable.Dispose();
    }
}
