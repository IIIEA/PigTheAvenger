using UnityEngine;

public class ZIndex : MonoBehaviour
{
    [SerializeField] private float _paralax;

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y + _paralax);    
    }
}
