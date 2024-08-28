using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    Rigidbody2D rigidbody;
    public Text energyText;
    public static int energy = 0;
    public GameObject bullet;
    public float bulletSpeed;
    public float fireDelay;
    private float lastFire;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        fireDelay = GameController.FireRate;
        speed = GameController.MoveSpeed;
        
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        float shootHorizontal = Input.GetAxis("ShootHorizontal");
        float shootVertical = Input.GetAxis("ShootVertical");

        if ((shootHorizontal != 0 || shootVertical != 0) && Time.time > lastFire + fireDelay)
        {
            Shoot(shootHorizontal, shootVertical);
            lastFire = Time.time;
        }

        rigidbody.velocity = new Vector2(moveHorizontal, moveVertical) * speed;
        energyText.text = "Energy: " + energy;
    }

    void Shoot(float x, float y)
    {
        GameObject newBullet = Instantiate(bullet, transform.position, transform.rotation);
        newBullet.AddComponent<Rigidbody2D>().gravityScale = 0;
        newBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(
            (x < 0) ? Mathf.Floor(x) : Mathf.Ceil(x),
            (y < 0) ? Mathf.Floor(y) : Mathf.Ceil(y)
        ) * bulletSpeed;
    }
}
