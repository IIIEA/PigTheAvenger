using System.Collections;
using UnityEngine;

[RequireComponent(typeof(InputHandler))]
public class CharacterMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private InputHandler _input;

    private float _currentSpeed;
    private float _slowDowndSpeed => _speed / 2;

    private void Awake()
    {
        _currentSpeed = _speed;
        _input = GetComponent<InputHandler>();
    }

    private void Update()
    {
        var targetVector = new Vector3(_input.InputVector.x, 0, _input.InputVector.y);

        MoveTowardTarget(targetVector);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<SelfDestroyer>(out SelfDestroyer selfDestroyer))
        {
            StartCoroutine(SlowDown());
        }
    }

    private void MoveTowardTarget(Vector3 targetVector)
    {
        var speed = _currentSpeed * Time.deltaTime;
        transform.Translate(targetVector * speed);
    }

    private IEnumerator SlowDown()
    {
        _currentSpeed = _slowDowndSpeed;

        yield return new WaitForSeconds(2f);

        _currentSpeed = _speed;
    }
}
