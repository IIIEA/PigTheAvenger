using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _timeToChangeDirection;
    private Rigidbody _rigidbody;

    public UnityAction<Vector3> OnDirectionChanged = null;

    public Vector3 Direction { get; private set; }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        ChangeDirection();
    }

    private void OnEnable()
    {
        StartCoroutine(DirectionChanger());
    }

    private void OnDisable()
    {
        StopCoroutine(DirectionChanger());
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        _rigidbody.MovePosition(transform.position + Direction * _movementSpeed * Time.deltaTime);
    }

    private void ChangeDirection()
    {
        int direction = Random.Range(0, 3);

        switch (direction)
        {
            case 0:
                Direction = new Vector3(0, 0, 1);
                break;
            case 1:
                Direction = new Vector3(0, 0, -1);
                break;
            case 2:
                Direction = new Vector3(1, 0, 0);
                break;
            case 3:
                Direction = new Vector3(-1, 0, 0);
                break;
            default:
                break;
        }

        OnDirectionChanged?.Invoke(Direction);
    }

    private IEnumerator DirectionChanger()
    {
        while (true)
        {
            var time = new WaitForSeconds(_timeToChangeDirection);

            ChangeDirection();

            yield return time;
        }
    }
}
