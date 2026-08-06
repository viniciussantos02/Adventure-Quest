using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;

public class CrabEnemyController : EnemyController
{
    bool isPlayerVisible;

    public float detectionRadius;
    public float rotationSpeed;

    private Animator animator;
    private Quaternion startRotatePosition;

    private void Start()
    {
        startRotatePosition = this.transform.rotation;
    }

    private void Update()
    {
        verifyPlayerVisibility();
    }

    private void verifyPlayerVisibility()
    {
        if(!isPlayerVisible)
        {
            SetPlayerVisibility(GetPlayerDistance());
            ResetRotation();
        }
        else
        {
            RotateTowardsPlayer();
            SetPlayerVisibility(GetPlayerDistance());
        }
    }

    private void RotateTowardsPlayer()
    {
        Vector3 direction = (GetPlayerPosition() - this.transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotaion = Quaternion.LookRotation(direction);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotaion, rotationSpeed * Time.deltaTime);
        }
    }

    private void ResetRotation()
    {
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, startRotatePosition, rotationSpeed * Time.deltaTime);
    }

    private void SetPlayerVisibility(float playerDistance)
    {
        if(playerDistance <= detectionRadius)
        {
            isPlayerVisible = true;
        }
        else
        {
            isPlayerVisible = false;
        }
    }

    private float GetPlayerDistance()
    {
        return Vector3.Distance(this.transform.position, GetPlayerPosition());
    }

    private Vector3 GetPlayerPosition()
    {
        return PlayerController.Instance.transform.position;
    }
}
