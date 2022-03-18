using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour
{
    public GameObject explosionPrefab;

    private bool exploded = false;

    void Start()
    {
        Invoke("Explode", 3f);
    }

    void Explode()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        StartCoroutine(CreateExplosions(Vector3.forward));
        StartCoroutine(CreateExplosions(Vector3.right));
        StartCoroutine(CreateExplosions(Vector3.back));
        StartCoroutine(CreateExplosions(Vector3.left));

        GetComponentInChildren<SpriteRenderer>().enabled = false;
        exploded = true;
        Destroy(gameObject, .3f);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!exploded && other.CompareTag("Explosion"))
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
                Instantiate(explosionPrefab, transform.position + (i * direction), explosionPrefab.transform.rotation);
            }

            yield return new WaitForSeconds(.05f);
        }
    }
}
