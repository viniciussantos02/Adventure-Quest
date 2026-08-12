using UnityEngine;

public class EnemyDamageHitBox : MonoBehaviour
{
    public int damage;

    private static string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            other.GetComponent<PlayerHealthManager>().TakeDamage(damage);
        }
    }
}
