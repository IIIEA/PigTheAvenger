using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAnimationSetter : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private EnemyAI _enemy = null;

    private void OnEnable()
    {
        _enemy.OnDirectionChanged += OnSetAnimation;
        _enemy.OnDisabled += OnDisable;
    }

    private void OnDisable()
    {
        _enemy.OnDirectionChanged -= OnSetAnimation;
        _enemy.OnDisabled -= OnDisable;
    }

    private void OnSetAnimation(Vector3 direction)
    {
        _animator.SetFloat("Horizontal", direction.x);
        _animator.SetFloat("Vertical", direction.z);
    }

    private void OnDisable(float time)
    {
        StartCoroutine(Disable(time));
    }

    private IEnumerator Disable(float time)
    {
        _animator.SetBool("Disabled", true);

        yield return new WaitForSeconds(time);

        _animator.SetBool("Disabled", false);
    }
}
