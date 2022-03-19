using UnityEngine;

[RequireComponent(typeof(InputHandler))]
public class PlayerAnimationSetter : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private InputHandler _input;

    private void Awake()
    {
        _input = GetComponent<InputHandler>();
    }

    private void Update()
    {
        _animator.SetFloat("Horizontal", _input.HorizontalDirection);
        _animator.SetFloat("Vertical", _input.VerticalDirection);
    }
}
