using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using System.Linq;

public class BallMotionControl : MonoBehaviour
{
    [SerializeField] private SO_GameplayParameters _gameplayParameters;

    private void Start()
    {
        ManageCollisions(
            trigger: GetComponent<Collider2D>(),
            rb: GetComponent<Rigidbody2D>(),
            ballSpeed: _gameplayParameters.BallSpeed(),
            speedMultiplier: _gameplayParameters.BallSpeedMultiplier());
    }

    private void ManageCollisions(Collider2D trigger, Rigidbody2D rb, Vector2 ballSpeed, float speedMultiplier)
    {        
        trigger.OnTriggerEnter2DAsObservable()
            .Where(collider => !collider.isTrigger)
            .Select(collider => SignChanger(collider.transform.rotation.eulerAngles.z))
            .Subscribe(sign => ChangeVelocity(rb, speedMultiplier, new Vector2(-sign, sign)))
            .AddTo(this);

        trigger.OnTriggerEnter2DAsObservable()
            .Where(collider => collider.isTrigger)
            .Subscribe(collider => { ResetPosition(rb); ResetVelocity(rb, ballSpeed); })
            .AddTo(this);
    }

    private readonly Func<float, float> SignChanger =
        (angle) => Mathf.Sin(angle * Mathf.Deg2Rad) - Mathf.Cos(angle * Mathf.Deg2Rad);

    private readonly Action<Rigidbody2D> ResetPosition = (rb) => { rb.position = Vector2.zero; };

    private readonly Action<Rigidbody2D, Vector2> ResetVelocity =
        (rb, ballSpeed) => { rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * Mathf.Abs(ballSpeed.x), Mathf.Sign(rb.velocity.y) * Mathf.Abs(ballSpeed.y)); };

    private readonly Action<Rigidbody2D, float, Vector2> ChangeVelocity =
        (rb, speedMultiplier, signChanger) => { rb.velocity = speedMultiplier * new Vector2(signChanger.x * rb.velocity.x, signChanger.y * rb.velocity.y); };
}
