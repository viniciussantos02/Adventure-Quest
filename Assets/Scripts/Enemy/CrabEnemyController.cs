using System.Collections;
using UnityEngine;

public class CrabEnemyController : EnemyController
{
    private bool isPlayerVisible;

    public float attackRange;
    public float attackCooldown;

    public float detectionRadius;
    public float rotationSpeed;

    private float lastAttackTime = 0;
    private bool isTaunting = false;
    private bool hasTauntingAnimationPlayed = false;
    private bool isAttacking = false;
    private static float simpleAttackAnimationDuration = 0.668f;

    private Vector3 moveDirection = Vector3.zero;

    public EnemyDamageHitBox clawHitbox;

    private void Start()
    {
        if (clawHitbox != null)
        {
            clawHitbox.damage = damage;
        }
    }

    private void Update()
    {
        VerifyPlayerVisibility();
        
        if (isPlayerVisible)
        {
            RotateTowardsPlayer();
        }
    }

    private void FixedUpdate()
    {
        // A física RODA AQUI, na taxa fixa do motor (independente do FPS/tamanho da tela)
        if (isPlayerVisible && moveDirection != Vector3.zero && !isAttacking)
        {
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
            animator.SetFloat("Speed", moveDirection.magnitude);
        }
    }

    private void VerifyPlayerVisibility()
    {
        float playerDistance = GetPlayerDistance();

        SetPlayerVisibility(playerDistance);

        if (isPlayerVisible)
        {
            // 1. Calcula a intenção de movimento
            if (playerDistance > attackRange && !isTaunting && !isTakingDamage)
            {
                moveDirection = (GetPlayerPosition() - transform.position).normalized;
                moveDirection.y = 0;
            }
            else
            {
                moveDirection = Vector3.zero;
            }

            // 2. Checa condições de ataque e taunt
            bool isPlayerDead = PlayerController.Instance.playerHealth.isDead;

            if (playerDistance <= attackRange && Time.time - lastAttackTime >= attackCooldown && !isPlayerDead)
            {
                Attack();
                lastAttackTime = Time.time;
            }
            else if (playerDistance > attackRange)
            {
                PlayTauntingAnimation();
            }
        }
        else
        {
            moveDirection = Vector3.zero;
            animator.SetFloat("Speed", moveDirection.magnitude);
        }
    }

    private void RotateTowardsPlayer()
    {
        Vector3 direction = (GetPlayerPosition() - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void SetPlayerVisibility(float playerDistance)
    {
        if (playerDistance <= detectionRadius)
        {
            if (!isPlayerVisible)
            {
                isPlayerVisible = true;
                EnemyAlert();
            }
        }
        else
        {
            isPlayerVisible = false;
            hasTauntingAnimationPlayed = false;
        }
    }

    private float GetPlayerDistance()
    {
        return Vector3.Distance(transform.position, GetPlayerPosition());
    }

    private Vector3 GetPlayerPosition()
    {
        return PlayerController.Instance.transform.position;
    }

    public override void Death()
    {
        base.Death();

        animator.SetTrigger("Die");
        this.enabled = false; //Desativa o próprio script de lógica/Update
        rb.useGravity = false;
        coll.enabled = false;
        
        if (clawHitbox != null)
        {
            clawHitbox.enabled = false;
        }
    }

    public override void EnemyAlert()
    {
        base.EnemyAlert();
    }

    public override void Attack()
    {
        base.Attack();
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        moveDirection = Vector3.zero; // Para imediatamente qualquer movimento
        
        animator.SetTrigger("Attack");

        // Aguarda o tempo do ataque (ajuste attackDuration no Inspector conforme o tempo da sua animação)
        yield return new WaitForSeconds(simpleAttackAnimationDuration);

        isAttacking = false;
    }

    private void PlayTauntingAnimation()
    {
        if (!hasTauntingAnimationPlayed)
        {
            isTaunting = true;
            hasTauntingAnimationPlayed = true;
            animator.SetTrigger("Taunting");
            StartCoroutine(MovingAfterTauntingAnimation());
        }
    }

    private IEnumerator MovingAfterTauntingAnimation()
    {
        yield return new WaitForSeconds(2f);
        isTaunting = false;
    }

    //Usado para mostrar dentro da cena o raio da deteccao do player quanto do range do ataque visualmente e dinamicamente.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}