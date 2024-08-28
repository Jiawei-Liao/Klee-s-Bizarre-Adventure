using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Wander,
    Follow,
    Die,
    Attack
};

public enum EnemyType
{
    Melee,
    Ranged
};

public class EnemyController : MonoBehaviour
{
    GameObject player;

    public EnemyState currentState = EnemyState.Wander;
    public GameObject bulletPrefab;

    public EnemyType enemyType;

    public float agroRange;
    public float speed;

    private bool chooseDirection = false;
    private bool dead = false;
    private Vector3 randomDirection;

    public bool notInRoom = false;

    private float minWanderTime = 1f;
    private float maxWanderTime = 2f;
    private float minIdleTime = 1f;
    private float maxIdleTime = 2f;

    public float attackRange = 1f;
    public float attackCooldown;
    private bool attackInCooldown = false;

    

    void Start()
    {  
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Wander:
                Wander();
                break;
            case EnemyState.Follow:
                Follow();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Die:
                break;
        }

        if (IsPlayerInRange() && currentState != EnemyState.Die)
        {
            currentState = EnemyState.Follow;
        }
        else if (!IsPlayerInRange() && currentState != EnemyState.Die)
        {
            currentState = EnemyState.Wander;
        }

        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
        {
            currentState = EnemyState.Attack;
        }
        
    }

    private bool IsPlayerInRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) <= agroRange;
    }

    private IEnumerator ChooseDirection()
    {
        chooseDirection = true;
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        randomDirection = randomRotation * Vector3.right;
        randomDirection.Normalize();
        yield return new WaitForSeconds(Random.Range(minWanderTime, maxWanderTime));
        randomDirection = Vector3.zero;
        yield return new WaitForSeconds(Random.Range(minIdleTime, maxIdleTime));
        chooseDirection = false;
    }

    void Wander()
    {
        if (!chooseDirection)
        {
            StartCoroutine(ChooseDirection());
        }

        transform.position += (randomDirection * speed * Time.deltaTime);

        if (IsPlayerInRange())
        {
            currentState = EnemyState.Follow;
        }
    }

    void Follow()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    IEnumerator AttackCooldown()
    {
        attackInCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        attackInCooldown = false;
    }

    void Attack()
    {
        if (!attackInCooldown)
        {
            switch (enemyType)
            {
                case (EnemyType.Melee):
                    GameController.DamagePlayer(1);
                    StartCoroutine(AttackCooldown());
                    break;
                case (EnemyType.Ranged):
                    GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
                    bullet.GetComponent<BulletController>().GetPlayer(player.transform);
                    bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
                    bullet.GetComponent<BulletController>().isEnemyBullet = true;
                    StartCoroutine(AttackCooldown());
                    break;
            }
            
        }
    }

    public void Death()
    {
        Destroy(gameObject);
    }
}
