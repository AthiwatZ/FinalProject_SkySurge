using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Player : MonoBehaviour, IHitTarget
{
    [Header("Stats")]
    public int hp = 100;
    public int maxHp = 100;
    public int lv = 1;
    public int exp = 0;
    public float moveSpeed = 6f;

    [Header("Refs")]
    public Weapon weapon;
    Rigidbody2D rb;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip damageSfx;
    public AudioClip levelUpSfx;

    Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (GameManager.I.state != GameState.Playing) return;

        // เคลื่อนที่
        rb.linearVelocity = moveInput * moveSpeed;

        RotateTowardMouse();

        // ยิงเมื่อกดคลิกซ้าย
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            ShootAtMouse();
        }
    }

    void ShootAtMouse()
    {
        // 1) ตำแหน่งเมาส์บนโลก (world)
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // 2) ทิศทางจาก player → เมาส์
        Vector2 dir = (mousePos - (Vector2)transform.position).normalized;

        // 3) ยิง
        weapon.Fire(transform.position, dir);
    }

    void RotateTowardMouse()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 dir = mousePos - (Vector2)transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    // New Input System event (PlayerInput)
    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    public void TakeDamage(int amount)
    {
        if (GameManager.I.state != GameState.Playing) return;

        hp -= amount;

        if (audioSource != null && damageSfx != null)
        {
            audioSource.PlayOneShot(damageSfx);
        }

        GameManager.I.ui.UpdateHUD(hp, maxHp, lv, exp, ExpToNextLevel(), GameManager.I.waveMgr.WaveIndex, GameManager.I.score);
        if (hp <= 0) Die();
    }

    public void Heal(int amount)
    {
        hp = Mathf.Min(maxHp, hp + amount);
        GameManager.I.ui.UpdateHUD(hp, maxHp, lv, exp, ExpToNextLevel(), GameManager.I.waveMgr.WaveIndex, GameManager.I.score);
    }

    void Die()
    {
        rb.linearVelocity = Vector2.zero;
        weapon.enabled = false;
        GameManager.I.End();
        gameObject.SetActive(false);
    }
    public int ExpToNextLevel()
    {
        return 5 + (lv - 1) * 5;
    }
    public void LevelUp()
    {
        lv++;

        if (audioSource != null && levelUpSfx != null)
        {
            audioSource.PlayOneShot(levelUpSfx);
        }

        GameManager.I.OnPlayerLevelUp();
    }
    public void AddExp(int value)
    {
        exp += value;
        while (exp >= ExpToNextLevel())
        {
            exp -= ExpToNextLevel();
            LevelUp();
        }
        GameManager.I.ui.UpdateHUD(hp, maxHp, lv, exp, ExpToNextLevel(), GameManager.I.waveMgr.WaveIndex, GameManager.I.score);
    }
}
