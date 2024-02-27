using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickstartMotion : MonoBehaviour
{
    [SerializeField] private SO_GameplayParameters _gameplayParameters;

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = _gameplayParameters.BallSpeed();
    }
}
