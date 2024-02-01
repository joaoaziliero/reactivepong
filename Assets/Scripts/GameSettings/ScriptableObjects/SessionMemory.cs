using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SessionMemory : ScriptableObject
{
    [Header("Leftmost player name")]
    public string leftmostName = "";
    [Header("Rightmost player name")]
    public string rightmostName = "";
    [Header("Greatest leftmost score")]
    public int leftmostMaxScore = 0;
    [Header("Greatest rightmost score")]
    public int rightmostMaxScore = 0;

    public void ResetData()
    {
        leftmostName = "";
        rightmostName = "";
        leftmostMaxScore = 0;
        rightmostMaxScore = 0;
    }
}
