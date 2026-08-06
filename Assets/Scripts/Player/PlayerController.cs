using UnityEngine;
using UnityEngine.InputSystem; // 1. Importante: Incluir o namespace do novo Input System

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed;

    public int attackAnimationNumberMin;
    public int attackAnimationNumberMax;

    public ParticleSystem slashFx;

    private CharacterController controller;
    private Animator animator;

    private Vector2 movementInput; // Guarda o valor bruto (X, Y) do novo Input System
    private Vector3 inputDirection;
    private bool isAttacking;
    private float attackCoolDown = 0.53f;

    public static PlayerController Instance;

    //É chamado 1x antes do metodo Start()
    //Criando um singleton global para o jogo (Guardando uma referencia do player para usar em outros codigos)
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        UpdateAttackState();
    }

    // 2. Método chamado automaticamente pelo componente "Player Input" (Modo Send Messages)
    public void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    public void OnAttack(InputValue value)
    {
        if(value.isPressed && !isAttacking)
        {
            int animationNumber = GetRandomNumber(attackAnimationNumberMin, attackAnimationNumberMax);

            animator.SetTrigger("Attack");
            animator.SetInteger("AttackAnimation", animationNumber);
            isAttacking = true;
            
            attackCoolDown = 0.53f;

            PlaySlashAnimation(animationNumber);
        }
    }

    void Move()
    {
        // Converte o Vector2 do Input (X, Y) para o espaço 3D (X, Z)
        inputDirection = new Vector3(movementInput.x, 0f, movementInput.y);

        // Só processa a rotação e o movimento se houver algum input (botão pressionado)
        if (inputDirection != Vector3.zero && !isAttacking)
        {
            // Cálculo de para onde o personagem deve olhar
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection);

            // Rotaciona de maneira suave
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Aplica o movimento no CharacterController
            controller.Move(inputDirection * moveSpeed * Time.deltaTime);
        }

        animator.SetFloat("Speed", inputDirection.magnitude);
    }

    void UpdateAttackState()
    {
        if (isAttacking)
        {
            attackCoolDown -= Time.deltaTime;

            if (attackCoolDown <= 0f)
            {
                isAttacking = false;
            }
        }
    }

    static int GetRandomNumber(int min, int max)
    {
        return UnityEngine.Random.Range(min, max + 1);
    }

    void PlaySlashAnimation(int attackAnimationNumber)
    {
        if(attackAnimationNumber == 1)
        {
            slashFx.Play();
        }
    }
}