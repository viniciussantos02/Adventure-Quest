using System.Collections;
using UnityEngine;

public class GameFreezeManager : MonoBehaviour
{
    public static GameFreezeManager Instance;

    private bool isFreezing = false;

    private void Awake()
    {
        // Singleton simples para você poder chamar de qualquer lugar
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Congela o jogo por um tempo em segundos reais.
    /// Exemplo: Freeze(0.05f) congela por 50 milissegundos.
    /// </summary>
    public void Freeze(float duration)
    {
        if (isFreezing) return;
        StartCoroutine(DoFreeze(duration));
    }

    private IEnumerator DoFreeze(float duration)
    {
        isFreezing = true;

        // Pausa o tempo do jogo
        Time.timeScale = 0f;

        // Aguarda a duração usando o RELÓGIO DA VIDA REAL (pois o Time.timeScale está em 0)
        yield return new WaitForSecondsRealtime(duration);

        // Restaura o tempo do jogo para a velocidade normal
        Time.timeScale = 1f;

        isFreezing = false;
    }
}
