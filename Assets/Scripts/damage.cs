using UnityEngine;

public class damage : MonoBehaviour
{
    public int _damage = 1;


    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth p = other.gameObject.GetComponent<PlayerHealth>();
        if (p != null)
            p.TakeDamage(_damage);
    }
}
