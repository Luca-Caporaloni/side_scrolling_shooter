using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemy : Controller_Enemy
{
    public float movementSpeed = 5f;
    public float rotationSpeed = 90f;
    public float shootCooldown = 2f;
    public GameObject projectilePrefab;

    private Transform playerTransform;
    private float lastShootTime;

    void Start()
    {
        playerTransform = Controller_Player._Player.transform;
        lastShootTime = Time.time;
    }

    void Update()
    {
        MoveTowardsPlayer();
        RotateTowardsPlayer();
        ShootPlayer();
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        transform.position += direction * movementSpeed * Time.deltaTime;
    }

    void RotateTowardsPlayer()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void ShootPlayer()
    {
        if (Time.time - lastShootTime >= shootCooldown)
        {
            Instantiate(projectilePrefab, transform.position, transform.rotation);
            lastShootTime = Time.time;
        }
    }
}