using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletLife;
    public bool isEnemyBullet = false;

    private Vector2 lastPos;
    private Vector2 currentPos;
    private Vector2 playerPos;

    void Start()
    {
        StartCoroutine(BulletDecay());
        if(!isEnemyBullet)
        {
            transform.localScale = new Vector2(GameController.BulletSize, GameController.BulletSize);
        }
    }

    void Update()
    {
        if (isEnemyBullet)
        {
            currentPos = transform.position;
            transform.position = Vector2.MoveTowards(transform.position, playerPos, 5f * Time.deltaTime);
            if (Vector2.Equals(currentPos, lastPos))
            {
                Destroy(gameObject);
            }
            lastPos = currentPos;
        }
    }

    public void GetPlayer(Transform player)
    {
        playerPos = player.position;
    }

    IEnumerator BulletDecay()
    {
        yield return new WaitForSeconds(bulletLife);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && !isEnemyBullet)
        {
            collision.gameObject.GetComponent<EnemyController>().Death();
            Destroy(gameObject);
        }

        if (collision.tag == "Player" && isEnemyBullet)
        {
            GameController.DamagePlayer(1);
            Destroy(gameObject);
        }
    }
}
