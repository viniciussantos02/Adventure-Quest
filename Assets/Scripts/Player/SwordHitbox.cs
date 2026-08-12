using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    public int damage;
    private static int layerNumber = 6; //Enemy Layer

    // OnTriggerEnter is called by the Unity Runtime.
    // OnTriggerEnter is called when the Collider other enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == layerNumber)
        {
            other.GetComponent<EnemyController>().OnHit(damage);
        }
    }
}
