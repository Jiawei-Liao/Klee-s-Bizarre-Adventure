using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    private static float health = 6;
    private static float maxHealth = 6;
    private static float moveSpeed = 5;
    private static float fireRate = 0.5f;
    private static float bulletSize = 0.5f;

    public static float Health { get => health; set => health = value; }
    public static float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public static float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public static float FireRate { get => fireRate; set => fireRate = value; }
    public static float BulletSize { get => bulletSize; set => bulletSize = value; }

    public List<string> artifactsCollected = new List<string>();
    private int cwfCollected = 0;
    private int vvCollected = 0;

    public Text healthText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Update()
    {
        healthText.text = "Health: " + Health;
    }

    public static void DamagePlayer(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Health = 0;
            KillPlayer();
        }
    }

    public static void HealPlayer(float heal)
    {
        Health += heal;
        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }
    }

    public static void MoveSpeedChange(float change)
    {
        MoveSpeed += change;
    }

    public static void FireRateChange(float change)
    {
        FireRate -= change;
    }

    public static void BulletSizeChange(float change)
    {
        BulletSize += change;
    }

    private static void KillPlayer()
    {
        Debug.Log("Player died!");
    }

    public void UpdateCollectedItems(CollectionController item)
    {
        artifactsCollected.Add(item.item.name);
        switch(item.item.set)
        {
            case "cwf":
                cwfCollected++;
                if (cwfCollected > 2)
                {
                    FireRateChange(0.5f);
                }
                break;
            case "vv":
                vvCollected++;
                break;
        }
    }
}
