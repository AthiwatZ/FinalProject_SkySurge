using UnityEngine;

public enum PickupType { Exp, Heal }

public class Pickup : MonoBehaviour
{
    public PickupType type;
    public int value = 1;

    [Header("Audio")]
    public AudioClip pickupSfx;
    public float volume = 1f;

    void Awake()
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;
        var c = GetComponent<CircleCollider2D>();
        c.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var p = other.GetComponent<Player>();
        if (!p) return;

        if (pickupSfx != null)
        {
            AudioSource.PlayClipAtPoint(pickupSfx, transform.position, volume);
        }

        if (type == PickupType.Exp)
        {
            p.AddExp(value);
        }
        else if (type == PickupType.Heal)
        {
            p.Heal(value);
        }

        Destroy(gameObject);
        GameManager.I.ui.UpdateHUD(p.hp, p.maxHp ,p.lv, p.exp, p.ExpToNextLevel(), GameManager.I.waveMgr.WaveIndex, GameManager.I.score);
    }
}

