using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Linq;
using TMPro;

public class NameInputReader : MonoBehaviour
{
    private CompositeDisposable _compositeDisposable;

    [Header("Leftmost name input field")]
    [SerializeField] private TMP_InputField _leftmostField;
    [Header("Rightmost name input field")]
    [SerializeField] private TMP_InputField _rightmostField;
    [Header("Player data record")]
    [SerializeField] private SessionMemory _sessionMemory;

    private void Awake()
    {
        if(_compositeDisposable != null)
        {
            _compositeDisposable.Dispose();
        }

        _compositeDisposable = new CompositeDisposable();
    }

    private void Start()
    {
        ManageNameChanges(_leftmostField, _rightmostField, _sessionMemory)
            .AddTo(_compositeDisposable);
    }

    private IObservable<(string id, string text)> TextFieldObservable(TMP_InputField textField, string identifier)
    {
        return textField.ObserveEveryValueChanged(input => input.text)
            .Where(str => str != "")
            .Select(str => (id: identifier, text: str));
    }

    private IDisposable ManageNameChanges(TMP_InputField leftmostField, TMP_InputField rightmostField, SessionMemory sessionMemory)
    {
        var leftmostObservable = TextFieldObservable(leftmostField, "L");
        var rightmostObservable = TextFieldObservable(rightmostField, "R");

        return Observable.Merge(leftmostObservable, rightmostObservable)
            .Select<(string id, string text), Action>(tuple =>
            {
                return tuple.id switch
                {
                    "L" => () => { sessionMemory.leftmostName = tuple.text; }
                    ,
                    _ => () => { sessionMemory.rightmostName = tuple.text; }
                    ,
                };
            })
            .Subscribe(action => action.Invoke());
    }

    private void OnDestroy()
    {
        _compositeDisposable.Dispose();
    }
}
