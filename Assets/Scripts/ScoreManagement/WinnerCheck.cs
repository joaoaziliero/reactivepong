using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinnerCheck : MonoBehaviour
{
    [SerializeField] private SessionMemory _sessionMemory;
    [SerializeField] private TextMeshProUGUI _winnerNameDisplay;

    private void Start()
    {
        if (_sessionMemory.leftmostMaxScore > _sessionMemory.rightmostMaxScore)
        {
            if (_sessionMemory.leftmostName == "")
            {
                _winnerNameDisplay.text = "Leftmost Player";
            }
            else
            {
                _winnerNameDisplay.text = _sessionMemory.leftmostName;
            }
        }
        else if (_sessionMemory.rightmostMaxScore > _sessionMemory.leftmostMaxScore)
        {
            if (_sessionMemory.rightmostName == "")
            {
                _winnerNameDisplay.text = "Rightmost Player";
            }
            else
            {
                _winnerNameDisplay.text = _sessionMemory.rightmostName;
            }
        }
    }
}
