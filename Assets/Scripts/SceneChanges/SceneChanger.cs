using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;
using System;
using System.Linq;

public class SceneChanger : MonoBehaviour
{
    private CompositeDisposable _compositeDisposable;

    [Header("Button reference")]
    [SerializeField] private Button _button;
    [Header("Scene name reference")]
    [SerializeField] private string _sceneName;

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
        ManageSceneChange(_button, _sceneName)
            .AddTo(_compositeDisposable);
    }

    private IDisposable ManageSceneChange(Button button, string sceneName)
    {
        return button.OnClickAsObservable()
            .Select<Unit, Action>(_ => () => { SceneManager.LoadScene(sceneName); })
            .Subscribe(action => action.Invoke());
    }

    private void OnDestroy()
    {
        _compositeDisposable.Dispose();
    }
}
