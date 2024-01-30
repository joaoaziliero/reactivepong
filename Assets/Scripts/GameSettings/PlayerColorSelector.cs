using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;
using System.Linq;

public class PlayerColorSelector : MonoBehaviour
{
    private CompositeDisposable _compositeDisposable;

    [Header("Color choice record")]
    [SerializeField] private ColorMemory _colorMemory;
    [Header("Color selection list")]
    [SerializeField] private List<Color> _colors;
    [Header("Color selector")]
    [SerializeField] private Button _button;
    [Header("Respective image to color")]
    [SerializeField] private Image _image;

    private void Awake()
    {
        if (_compositeDisposable != null)
        {
            _compositeDisposable.Dispose();
        }

        _compositeDisposable = new CompositeDisposable();
        _colorMemory.ResetColors();
    }

    private void Start()
    {
        ManageColorChoices(_button, _colors, _image, _colorMemory, gameObject.tag)
            .AddTo(_compositeDisposable);
    }

    private IDisposable ManageColorChoices(Button button, List<Color> colors, Image image, ColorMemory colorMemory, string tag)
    {
        return button.OnClickAsObservable()
            .Select(_ => 1)
            .Scan(-1, (acc, current) =>
            {
                return acc switch
                {
                    var index when index == colors.Count() - 1 => 0,
                    _ => acc + current,
                };
            })
            .Select(index => colors[index])
            .Select<Color, Action>(color => () => { image.color = color; colorMemory.SetColor(tag, color); })
            .Subscribe(action => action.Invoke());
    }

    private void OnDestroy()
    {
        _compositeDisposable.Dispose();
    }
}
