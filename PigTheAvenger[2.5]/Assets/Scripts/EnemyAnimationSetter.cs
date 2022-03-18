using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAnimationSetter : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private EnemyAI _enemy;

    private void OnEnable()
    {
        _enemy.OnDirectionChanged += OnSetAnimation;
    }

    private void OnDisable()
    {
        _enemy.OnDirectionChanged -= OnSetAnimation;
    }

    private void OnSetAnimation(Vector3 direction)
    {
        _animator.SetFloat("Horizontal", direction.x);
        _animator.SetFloat("Vertical", direction.z);
    }
}
