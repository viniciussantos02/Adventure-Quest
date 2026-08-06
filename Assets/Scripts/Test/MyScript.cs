using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;

public class MyScript : MonoBehaviour
{
    public float playerLife;
    private Keyboard keyboard; //Componente responsavel por escutar imputs do teclado
    public Renderer cube;
    public GameObject player;
    public List<GameObject> enemies = new List<GameObject>();
    public float playerVelicity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        keyboard = Keyboard.current;
    }

    // Update is called once per frame
    void Update()
    {
        if(keyboard != null && keyboard.cKey.wasPressedThisFrame)
        {
            Debug.Log("Tecla C apertada!");
            cube.material.color = Color.blue;
        } else if(keyboard != null && keyboard.vKey.wasPressedThisFrame)
        {
            Debug.Log("Tecla V apertada!");
            cube.material.color = Color.red;
        } else if(keyboard != null && keyboard.bKey.wasPressedThisFrame)
        {
            Debug.Log("Tecla V apertada!");
            cube.material.color = Color.green;
        }

        else if(keyboard != null && keyboard.wKey.isPressed)
        {
            player.transform.position += UnityEngine.Vector3.up * playerVelicity * Time.deltaTime;
        }

        else if(keyboard != null && keyboard.sKey.isPressed)
        {
            player.transform.position += UnityEngine.Vector3.down * playerVelicity * Time.deltaTime;
        }

        else if(keyboard != null && keyboard.dKey.isPressed)
        {
            player.transform.position += UnityEngine.Vector3.right * playerVelicity * Time.deltaTime;
        }

        else if(keyboard != null && keyboard.aKey.isPressed)
        {
            player.transform.position += UnityEngine.Vector3.left * playerVelicity * Time.deltaTime;
        }
    }
}
