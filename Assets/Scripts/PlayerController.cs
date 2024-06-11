using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set;}

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    [Header("Animator")]
    private Animator animator;

    [Header("Player")]
    private float speed = 4f;
    private float turnSpeed = 700;
    public float range = 12f;
    public Transform rangeCircle;
    public bool alive;
    public int level;

    [Header("Text")]
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;

    [Header("Weapon")]
    public GameObject weapon;
    public Transform weaponPoint;
    private float weaponTime = 1.5f;
    public bool alreadyAttack;

    [Header("Enemy")]
    public string enemyTag = "Enemy";
    public GameObject enemies;
    public List<EnemyController> allEnemies;
    public GameObject closest;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        alive = true;

        nameText.text = "Testing";

        level = 1;
    }

    // Update is called once per frame
    void Update()
    {
        rangeCircle.transform.localScale = new Vector3(range, 0.2f, range);

        levelText.text = level.ToString();

        if (alive)
        {
            if (CanvasManager.Instance.aliveCounter > 1)
            {
                Move();
                if (EnemyCount.Instance.enemies != null)
                {
                    if (animator.GetBool("IsIdle") && Vector3.Distance(ClosestEnemy().transform.position, transform.position) < range)
                    {
                        Shoot(ClosestEnemy());
                    }

                    if (Input.anyKey)
                    {
                        animator.SetBool("IsAttack", false);
                    }
                }
            }
            else
            {
                animator.SetBool("IsDance", true);
            }
            
        }
    }

    //Die on hit
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Weapon"))
        {
            animator.SetBool("IsDead", true);
            rangeCircle.gameObject.SetActive(false);
            nameText.gameObject.SetActive(false);
            levelText.gameObject.SetActive(false);
            alive = false;
        }
    }

    public void Move()
    {
        float horizontal1 = SimpleInput.GetAxis("Horizontal");
        float vertical1 = SimpleInput.GetAxis("Vertical");

        Vector3 dir = new Vector3(horizontal1, 0f, vertical1).normalized;

        transform.Translate(dir * speed * Time.deltaTime, Space.World);

        transform.Translate(Vector3.forward * Time.deltaTime * Mathf.Abs(vertical1));
        transform.Translate(Vector3.forward * Time.deltaTime * Mathf.Abs(horizontal1));

        if (dir.magnitude >= 0.1f)
        {
            Quaternion toRotation = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, turnSpeed * Time.deltaTime);

            animator.SetBool("IsIdle", false);
        }
        else
        {
            animator.SetBool("IsIdle", true);
        }
    }

    public void Shoot(EnemyController enemy)
    {
        transform.LookAt(enemy.transform.position);

        if (!alreadyAttack)
        {
            animator.SetBool("IsAttack", true);
            var wp = Instantiate(weapon, weaponPoint.transform.position, transform.rotation);
            wp.transform.localScale = Vector3.one + (new Vector3(0.2f, 0.2f, 0.2f) * (level - 1));

            alreadyAttack = true;
            Invoke(nameof(ResetAttack), weaponTime);
        }
    }

    public void ResetAttack()
    {
        alreadyAttack = false;
        animator.SetBool("IsAttack", false);
    }

    public EnemyController ClosestEnemy()
    {
        EnemyController closest = null;
        float dist = Mathf.Infinity;

        foreach (var e in EnemyCount.Instance.enemies)
        {
            if ( Vector3.Distance(e.transform.position, transform.position) < dist)
            {
                closest = e;
                dist = Vector3.Distance(e.transform.position, transform.position);
            }
        }
        return closest;
    }

    public void LevelUp()
    {
        gameObject.transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
    }
}
