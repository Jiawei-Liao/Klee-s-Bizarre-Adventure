using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string name;
    public string set;
    public string description;
    public Sprite itemImage;
}

public class CollectionController : MonoBehaviour
{
    public Item item;
    public float healthChange;
    public float moveSpeedChange;
    public float fireRateChange;
    public float bulletSizeChange;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = item.itemImage;
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
        gameObject.AddComponent<PolygonCollider2D>().isTrigger = true;
    }
    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController.energy++;
            GameController.HealPlayer(healthChange);
            GameController.MoveSpeedChange(moveSpeedChange);
            GameController.FireRateChange(fireRateChange);
            GameController.BulletSizeChange(bulletSizeChange);
            GameController.instance.UpdateCollectedItems(this);
            Destroy(gameObject);
        }
    }
}
