using UnityEngine;
using UnityEngine.InputSystem;

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

    [Header("Mobile Controls")]
    public VirtualJoystick joystick;
    public FireButton fireButton;
    public UnityEngine.Transform firePoint;

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
        /*if (GameManager.I.state != GameState.Playing) return;

        // เคลื่อนที่
        rb.linearVelocity = moveInput * moveSpeed;*/

        if (GameManager.I.state != GameState.Playing) return;

        Vector2 input = joystick != null ? joystick.Direction : moveInput;

        rb.linearVelocity = input * moveSpeed;

        if (input.sqrMagnitude > 0.01f)
        {
            RotateToDirection(input);
        }

        if (fireButton != null && fireButton.IsHolding)
        {
            FireForward();
        }
    }

    // New Input System event (PlayerInput)
    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    void RotateToDirection(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // ถ้าหน้ายาน sprite หันขึ้น ให้ใช้ -90f
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    void FireForward()
    {
        if (weapon == null) return;

        Vector2 origin = firePoint != null ? firePoint.position : transform.position;
        Vector2 dir = transform.up;

        weapon.Fire(origin, dir);
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
    public void Revive()
    {
        hp = maxHp;
        weapon.enabled = true;
        gameObject.SetActive(true);
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
