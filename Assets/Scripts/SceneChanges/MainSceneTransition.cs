using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;
using System;
using System.Linq;

public class MainSceneTransition : MonoBehaviour
{
    private CompositeDisposable _compositeDisposable;

    [Header("Play button reference")]
    [SerializeField] private Button _playButton;
    [Header("Main scene name reference")]
    [SerializeField] private string _mainSceneName;

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
        ManageSceneChange(_playButton, _mainSceneName)
            .AddTo(_compositeDisposable);
    }

    private IDisposable ManageSceneChange(Button playButton, string mainSceneName)
    {
        return playButton.OnClickAsObservable()
            .Select<Unit, Action>(_ => () => { SceneManager.LoadScene(mainSceneName); })
            .Subscribe(action => action.Invoke());
    }

    private void OnDestroy()
    {
        _compositeDisposable.Dispose();
    }
}
