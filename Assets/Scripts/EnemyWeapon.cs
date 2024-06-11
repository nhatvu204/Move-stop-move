using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    private float speed = 8f;

    [SerializeField] private float destroyTime = 1.2f;

    private void Start()
    {
        rb.velocity = transform.forward * speed;

        Destroy(gameObject, destroyTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("Player"))
        {
            Destroy(gameObject);
        } 
    }
}
