using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Enemy : MonoBehaviour, IHitTarget
{
    public int hp = 30;
    public int baseHp = 30;
    public int atk = 8;
    public float moveSpeed = 5f;
    public int rewardExp = 1;
    public int scoreValue = 1;

    public WaveManager waveManager;

    Rigidbody2D rb;
    UnityEngine.Transform player;
    public ItemSpawner spawner;

    public AudioClip hitSfx;
    [Range(0f, 1f)]
    public float hitSfxVolume = 0.7f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        player = GameManager.I.player.transform;
    }

    void Update()
    {
        if (GameManager.I.state != GameState.Playing) { rb.linearVelocity = Vector2.zero; return; }
        if (!player) return;

        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = dir * moveSpeed;

        RotateTowardPlayer();
    }

    void RotateTowardPlayer()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    public void InitFromScale(int playerLv, int waveIdx)
    {
        hp = baseHp + (int)(playerLv * 2 + waveIdx * 3);
        atk = 8 + waveIdx;
        moveSpeed = 2.0f + 0.1f * waveIdx;
    }

    public void TakeDamage(int amount)
    {
        if (hitSfx != null)
        {
            AudioSource.PlayClipAtPoint(hitSfx, transform.position, hitSfxVolume);
        }

        hp -= amount;
        if (hp <= 0) OnDeath();
    }

    public virtual void OnDeath()
    {
        // ให้ EXP
        GameManager.I.player.AddExp(rewardExp);
        // ให้ Score
        GameManager.I.score += scoreValue;
        // ดรอปของ
        if (spawner) spawner.RollDrop(transform.position);

        waveManager.OnEnemyKilled();

        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Player"))
        {
            GameManager.I.player.TakeDamage(atk);
        }
    }
}
