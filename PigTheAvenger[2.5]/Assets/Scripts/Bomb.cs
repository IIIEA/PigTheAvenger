using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour
{
    [SerializeField] private GameObject _explosionPrefab;

    private bool _exploded = false;

    void Start()
    {
        Invoke("Explode", 3f);
    }

    void Explode()
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

        StartCoroutine(CreateExplosions(Vector3.forward));
        StartCoroutine(CreateExplosions(Vector3.right));
        StartCoroutine(CreateExplosions(Vector3.back));
        StartCoroutine(CreateExplosions(Vector3.left));

        GetComponentInChildren<SpriteRenderer>().enabled = false;
        _exploded = true;
        Destroy(gameObject, .3f);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!_exploded && other.TryGetComponent<SelfDestroyer>(out SelfDestroyer selfDestroyer))
        {
            CancelInvoke("Explode");
            Explode();
        }
    }

    private IEnumerator CreateExplosions(Vector3 direction)
    {
        for (int i = 0; i < 3; i++)
        {
            RaycastHit hit;

            Physics.Raycast(transform.position + new Vector3(0, .5f, 0), direction, out hit, i);

            if (!hit.collider)
            {
                Instantiate(_explosionPrefab, transform.position + (i * direction), _explosionPrefab.transform.rotation);
            }

            yield return new WaitForSeconds(.05f);
        }
    }
}
