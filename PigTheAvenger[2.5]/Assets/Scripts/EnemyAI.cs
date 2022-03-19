using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _timeToChangeDirection;
    private Rigidbody _rigidbody;
    [SerializeField] private bool _isDisabled;

    public UnityAction<Vector3> OnDirectionChanged = null;
    public UnityAction<float> OnDisabled;

    public Vector3 Direction { get; private set; }
    public bool IsDisabled => _isDisabled;

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

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<SelfDestroyer>(out SelfDestroyer selfDestroyer))
        {
            StartCoroutine(Disabled());
        }
    }

    private void Move()
    {
        if (_isDisabled == false)
        {
            _rigidbody.MovePosition(transform.position + Direction * _movementSpeed * Time.deltaTime);
        }
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

    private IEnumerator Disabled()
    {
        float time = 2f;
        var timeReturn = new WaitForSeconds(time);

        _isDisabled = true;

        _rigidbody.velocity = Vector3.zero;

        OnDisabled?.Invoke(time);

        yield return timeReturn;

        _isDisabled = false;
    }
}
