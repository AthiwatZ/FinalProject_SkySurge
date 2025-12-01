using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Weapon : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float fireRate = 0.5f;
    float cd;

    public AudioSource weaponAudio;
    public AudioClip shootSfx;

    public int damageBonus = 0;

    void Update()
    {
        if (GameManager.I.state != GameState.Playing) return;
        cd -= Time.deltaTime;
    }

    public void Fire(Vector2 origin, Vector2 dir)
    {
        if (cd > 0f) return;
        cd = fireRate;

        var go = GameObject.Instantiate(projectilePrefab, origin, Quaternion.identity);
        var p = go.GetComponent<Projectile>();

        p.Init(dir.normalized);
        p.owner = GameManager.I.player;

        if (weaponAudio != null && shootSfx != null)
        {
            weaponAudio.PlayOneShot(shootSfx);
        }
    }
}
