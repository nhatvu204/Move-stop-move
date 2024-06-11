using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    NavMeshAgent agent;

    public float sightRange = 20f;
    public float attackRange = 11f;
    public int level = 1;

    private Animator animator;

    [Header("Text")]
    [SerializeField] Text nametext;
    [SerializeField] Text levelText;

    [Header("Weapon")]
    public GameObject weapon;
    public Transform weaponPoint;
    private float weaponTime = 2f;
    private bool alreadyAttack;

    [SerializeField] float cooldownTime;
    float cooldownTimer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        cooldownTimer = Random.Range(1, cooldownTime);

        levelText.text = level.ToString();

        nametext.text = "Bot test";

        transform.localScale += new Vector3(0.2f, 0.2f, 0.2f) * (level - 1);
    }

    // Update is called once per frame
    void Update()
    {
        sightRange = 20f + 0.2f * (level - 1);
        attackRange = 11f + 0.2f * (level - 1);

        if (PlayerController.Instance.alive)
        {
            if (Vector3.Distance(transform.position, ClosestTarget().transform.position) > sightRange) Wander();
            if (Vector3.Distance(transform.position, ClosestTarget().transform.position) <= sightRange) Chase(ClosestTarget());
            if (Vector3.Distance(transform.position, ClosestTarget().transform.position) <= attackRange) Shoot(ClosestTarget());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Weapon"))
        {
            EnemyCount.Instance.enemies.Remove(this);
            animator.SetBool("IsDead", true);
            CanvasManager.Instance.aliveCounter -= 1;
            CanvasManager.Instance.deadCounter += 1;
            StartCoroutine(WaitDead());
        }
    }

    IEnumerator WaitDead()
    {
        yield return new WaitForSeconds(0.77f);
        Destroy(gameObject);
    }

    void Wander()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            //Generate a random direction within a sphere with a radius of 30
            Vector3 randomDir = Random.insideUnitSphere * 30f;

            randomDir += transform.position;

            NavMeshHit hit;
            NavMesh.SamplePosition(randomDir, out hit, 10f, NavMesh.AllAreas);

            Vector3 targetPos = hit.position;

            agent.SetDestination(targetPos);

            cooldownTimer = cooldownTime;

            animator.SetBool("IsIdle", false);
        }
    }

    public void Chase(GameObject opponent)
    {
        agent.SetDestination(opponent.transform.position);
        animator.SetBool("IsIdle", false);
    }

    public void Shoot(GameObject opponent)
    {
        agent.SetDestination(transform.position);
        transform.LookAt(opponent.transform.position);

        if (!alreadyAttack)
        {
            animator.SetBool("IsAttack", true);
            var wp = Instantiate(weapon, weaponPoint.transform.position, transform.rotation);
            wp.transform.localScale = Vector3.one + (new Vector3(0.2f, 0.2f, 0.2f) * (level - 1));

            alreadyAttack = true;
            Invoke(nameof(ResetAttack), weaponTime);
        } 
    }

    private void ResetAttack()
    {
        alreadyAttack = false;
        animator.SetBool("IsAttack", false);
    }

    public GameObject ClosestTarget()
    {
        GameObject closest = null;
        float dist = Mathf.Infinity;

        foreach (var e in EnemyCount.Instance.enemies)
        {
            if (Vector3.Distance(transform.position, e.transform.position) < dist && Vector3.Distance(transform.position, e.transform.position) > 0.1f)
            {
                closest = e.gameObject;
                dist = Vector3.Distance(transform.position, e.transform.position);
            }
        }

        if (Vector3.Distance(transform.position, PlayerController.Instance.transform.position) < dist)
        {
            closest = PlayerController.Instance.gameObject;
            dist = Vector3.Distance(transform.position, PlayerController.Instance.transform.position);
        }

        return closest;
    }
}
