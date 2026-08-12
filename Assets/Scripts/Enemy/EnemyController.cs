using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int lifePoints;
    public int damage;
    public float hitYOffset;
    public float alertYOffset;
    [SerializeField] protected float moveSpeed = 2f;

    public GameObject hitFx;
    public Animator animator;
    public Renderer enemyRenderer;
    public Collider coll;
    public Rigidbody rb;

    public GameObject canvaAlert;
    private GameObject currentAlert;

    [SerializeField] private float hitstopDuration = 0.05f;

     protected bool isTakingDamage = false; 
    protected static float takingDamageAnimationDuration = 0.734f;

    public virtual void Move(float playerDistance)
    {
        
    }

    public virtual void Attack()
    {
        
    }

    public void OnHit(int damage)
    {
        lifePoints -= damage;

        Debug.Log("Enemy life points: " + lifePoints);

        if (lifePoints <= 0)
        {
            HitEffects();
            Death();
        }
        else
        {
            animator.SetTrigger("Hit");

            HitEffects();
        }
    }

    public virtual void Death()
    {
        
    }

    public virtual void EnemyAlert()
    {
        if (currentAlert != null) return;

        currentAlert = Instantiate(canvaAlert, transform.position + new Vector3(0, alertYOffset), transform.rotation);
        currentAlert.transform.forward = Camera.main.transform.forward;
        Destroy(currentAlert, 2f);
    }

    private void HitEffects()
    {
        GameFreezeManager.Instance.Freeze(hitstopDuration);
        PlayerController.Instance.ShakeCamera();

        //Jeito que é chamado uma co-rotina
        StartCoroutine(HitFlash());
        StartCoroutine(HitCoroutine());

        GameObject hitEffect = Instantiate(hitFx, transform.position + new Vector3(0, hitYOffset, 0f), transform.rotation);
        Destroy(hitEffect, 1f);
    }

    //Uma Co-rotina
    IEnumerator HitFlash()
    {
        enemyRenderer.material.color = Color.red;
        yield return new WaitForSeconds(.20f);
        enemyRenderer.material.color = Color.white;
        yield return new WaitForSeconds(.20f);
        enemyRenderer.material.color = Color.red;
        yield return new WaitForSeconds(.20f);
        enemyRenderer.material.color = Color.white;
    }

    IEnumerator HitCoroutine()
    {
        isTakingDamage = true;
        yield return new WaitForSeconds(takingDamageAnimationDuration);
        isTakingDamage = false;
    }
}
