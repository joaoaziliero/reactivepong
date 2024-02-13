using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class SO_Test : ScriptableObject
{
    public Func<int> Number { get; }
    public Action<int> SetNumber { get; set; }
    private Action<int> SetAndResetSetter() { return (num) => { _number = num; SetNumber = null; }; }

    [SerializeField] private int _number;
    
    private SO_Test()
    {
        Number = () => _number;
        SetNumber = SetAndResetSetter();
    }
}
