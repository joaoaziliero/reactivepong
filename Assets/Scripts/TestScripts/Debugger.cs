using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    [SerializeField] private SO_Test _scriptableTest;

    void Start()
    {
        Debug.Log(_scriptableTest.Number.Invoke());
        Debug.Log(_scriptableTest.SetNumber == null);
    }
}
