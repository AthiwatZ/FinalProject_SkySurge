using UnityEngine;

public enum GameState { Playing, Paused, GameOver }

public class GameManager : MonoBehaviour
{
    public static GameManager I { get; private set; }

    public GameState state = GameState.Playing;
    public UpgradeSystem upgradeSystem;
    public WaveManager waveMgr;
    public UIManager ui;
    public Player player;
    public int score;

    void Awake()
    {
        I = this;
        StartGame();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void StartGame()
    {
        state = GameState.Playing;
        score = 0;
        ui.UpdateHUD(player.hp, player.maxHp, player.lv, player.exp, player.ExpToNextLevel(), waveMgr.WaveIndex, score);
        waveMgr.StartNextWave();
    }

    public void End()
    {
        if (state == GameState.GameOver) return;
        state = GameState.GameOver;

        Time.timeScale = 0f;
        waveMgr.StopWave();
        ui.ShowGameOver(score);
    }

    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
    public void OnPlayerLevelUp()
    {
        // หยุดเกมชั่วคราว
        state = GameState.Paused;
        Time.timeScale = 0f;

        // สุ่มการ์ดจาก UpgradeSystem
        var choices = upgradeSystem.RollChoices(player.lv, waveMgr.WaveIndex);

        // ส่งไปให้ UI แสดง พร้อม callback ตอนเลือกเสร็จ
        ui.ShowUpgrade(choices, OnUpgradePicked);
    }
    void OnUpgradePicked(UpgradeCard card)
    {
        // ใช้เอฟเฟกต์ของการ์ดกับ Player
        upgradeSystem.ApplyUpgrade(player, card);

        // ปิด Panel + กลับมาเล่นต่อ
        ui.HideUpgrade();
        Time.timeScale = 1f;
        state = GameState.Playing;
    }


}
