using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    [SerializeField] private SO_Test _scriptableTest;

    private void Start()
    {
        _scriptableTest.SetNumber(1);
        Invoke(nameof(LoadTestScene), 5);
    }

    private void LoadTestScene()
    {
        SceneManager.LoadScene("SCN_Test");
    }
}
