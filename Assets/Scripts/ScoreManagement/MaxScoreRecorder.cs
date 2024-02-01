using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MaxScoreRecorder : MonoBehaviour
{
    [SerializeField] private GameplayParameters _gameplayParameters;
    [SerializeField] private TextMeshProUGUI _leftmostScore;
    [SerializeField] private TextMeshProUGUI _rightmostScore;
    [SerializeField] private SessionMemory _sessionMemory;

    private int _maxPoints;

    private void Awake()
    {
        _sessionMemory.leftmostMaxScore = 0;
        _sessionMemory.rightmostMaxScore = 0;
        _maxPoints = _gameplayParameters.MaxPoints.Invoke();
    }

    private void Update()
    {
        if (int.Parse(_leftmostScore.text) > _sessionMemory.leftmostMaxScore)
        {
            _sessionMemory.leftmostMaxScore = int.Parse(_leftmostScore.text);
        }

        if (int.Parse(_rightmostScore.text) > _sessionMemory.rightmostMaxScore)
        {
            _sessionMemory.rightmostMaxScore = int.Parse(_rightmostScore.text);
        }

        if (int.Parse(_leftmostScore.text) == _maxPoints || int.Parse(_rightmostScore.text) == _maxPoints)
        {
            SceneManager.LoadScene("SCN_SessionEnd");
        }
    }
}
