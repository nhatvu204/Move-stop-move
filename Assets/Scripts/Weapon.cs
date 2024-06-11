using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Weapon : MonoBehaviour
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
        if (collision.collider.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            PlayerController.Instance.LevelUp();
            PlayerController.Instance.level += 1;
            PlayerController.Instance.range += 0.2f;
            CamController.Instance.offsetZ += 5f;
            CamController.Instance.offsetY += 5f;
        }
    }
}
