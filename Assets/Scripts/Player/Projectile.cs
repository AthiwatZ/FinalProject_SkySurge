using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 12f;
    public int damage = 10;
    public float lifeTime = 3f;
    public Player owner; // เผื่อกันยิงโดนตัวเอง

    Rigidbody2D rb;
    CircleCollider2D col;

    public void Init(Vector2 dir)
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
        if (!col) col = GetComponent<CircleCollider2D>();

        rb.linearVelocity = dir * speed;
        col.isTrigger = true;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // ถ้ากระสุนหันไปทางขวาเป็นด้านหน้า ? ใช้ angle เฉยๆ
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
