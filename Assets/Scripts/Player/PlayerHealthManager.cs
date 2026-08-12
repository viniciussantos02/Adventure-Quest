using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthManager : MonoBehaviour
{
    public int lifePoints;
    public bool isDead;

    private PlayerController player;
    public GameObject gameOverCanva;
    public Renderer playerRenderer;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
    }

    public void TakeDamage(int damage)
    {
        lifePoints -= damage;

        if (lifePoints <= 0)
        {
            isDead = true;
            Death();
        }
        else
        {
            player.Animator.SetTrigger("Hit");
            player.Animator.SetInteger("HitAnimation", PlayerController.GetRandomNumber(1, 2));
            StartCoroutine(DelayedHit());
        }

        Debug.Log("Player life: " + lifePoints);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Death()
    {
        player.Animator.SetTrigger("Die");
        player.enabled = false;
        player.PlayerInput.enabled = false;
        StartCoroutine(DelayedGameOver());
    }

    IEnumerator DelayedGameOver()
    {
        yield return new WaitForSeconds(1f);
        gameOverCanva.SetActive(true);
    }

    IEnumerator DelayedHit()
    {
        player.PlayerInput.enabled = false;
        yield return new WaitForSeconds(.5f); //Tempo da animacao
        player.PlayerInput.enabled = true;
    }
}
