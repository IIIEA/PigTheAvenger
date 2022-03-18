using UnityEngine;

public class SelfDestroyer : MonoBehaviour
{
    [SerializeField] private float _delay = 3f;

    void Start()
    {
        Destroy(gameObject, _delay);
    }
}
