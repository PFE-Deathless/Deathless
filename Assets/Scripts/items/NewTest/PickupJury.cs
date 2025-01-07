using UnityEngine;

public class PickupJury : MonoBehaviour
{
    public ParticleSystem effectPickup;
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other + " collided!");
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.gameObject.GetComponent<playerInventory>().SpawnItem();
            effectPickup.Play();
            GetComponent<MeshRenderer>().enabled = false;
            Destroy(gameObject, 1f);
        }
    }
}
