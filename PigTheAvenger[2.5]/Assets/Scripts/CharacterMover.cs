using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputHandler))]
public class CharacterMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private InputHandler _input;

    private void Awake()
    {
        _input = GetComponent<InputHandler>();
    }

    private void Update()
    {
        var targetVector = new Vector3(_input.InputVector.x, 0, _input.InputVector.y);

        MoveTowardTarget(targetVector);
    }

    private void MoveTowardTarget(Vector3 targetVector)
    {
        var speed = _speed * Time.deltaTime;
        transform.Translate(targetVector * speed);
    }
}
