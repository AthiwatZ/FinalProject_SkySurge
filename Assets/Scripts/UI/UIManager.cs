using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Refs")]
    public Slider hpBar;
    public Slider expBar;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;
    public TMP_Text hpText;

    [Header("Rarity Colors")]
    public Color commonColor;
    public Color rareColor;
    public Color epicColor;
    public Color legendaryColor;

    public AudioSource uiAudioSource;
    public AudioClip buttonClickSfx;

    Color GetColor(CardRarity r)
    {
        switch (r)
        {
            case CardRarity.Rare: return rareColor;
            case CardRarity.Epic: return epicColor;
            case CardRarity.Legendary: return legendaryColor;
            default: return commonColor;
        }
    }

    public void UpdateHUD(int currentHp, int maxHp, int lv, int currentExp, int expToNextLevel, int wave, int score)
    {
        // HP bar
        hpBar.maxValue = maxHp;
        hpBar.value = currentHp;
        hpText.text = $"{currentHp} / {maxHp}";

        // EXP bar
        expBar.maxValue = expToNextLevel;
        expBar.value = currentExp;


        waveText.text = $"Wave : {wave}";
        scoreText.text = $"Score : {score}";
    }

    public void ShowGameOver(int score)
    {
        gameOverPanel.SetActive(true);
        scoreText.text = $"Score: {score}";
    }

    [Header("Upgrade UI")]
    public GameObject upgradePanel;
    public Button[] cardButtons;
    public TextMeshProUGUI[] cardTitleTexts;
    public TextMeshProUGUI[] cardDescTexts;
    public Image[] cardBackgrounds;

    CanvasGroup cg;
    float fadeTime = 0.25f;

    Action<UpgradeCard> onPickCallback;
    List<UpgradeCard> currentChoices;

    void Awake()
    {
        cg = upgradePanel.GetComponent<CanvasGroup>();

        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;

        upgradePanel.SetActive(false);
    }

    public void ShowUpgrade(List<UpgradeCard> choices, Action<UpgradeCard> onPick)
    {
        upgradePanel.SetActive(true);
        StartCoroutine(FadeIn());

        currentChoices = choices;
        onPickCallback = onPick;

        for (int i = 0; i < cardButtons.Length; i++)
        {
            if (i < choices.Count)
            {
                var card = choices[i];
                cardButtons[i].gameObject.SetActive(true);

                // Title & Desc
                cardTitleTexts[i].text = card.displayName;
                cardDescTexts[i].text = card.description;

                // Rarity Color
                Color rarityColor = GetColor(card.rarity);
                rarityColor.a = 1f;
                cardBackgrounds[i].color = rarityColor;

                int idx = i;
                cardButtons[i].onClick.RemoveAllListeners();
                cardButtons[i].onClick.AddListener(() => OnCardClicked(idx));
            }
            else
            {
                cardButtons[i].gameObject.SetActive(false);
            }
        }

    }

    IEnumerator FadeIn()
    {
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;

        float t = 0;
        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime;
            cg.alpha = t / fadeTime;
            yield return null;
        }

        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }

    public void HideUpgrade()
    {
        StartCoroutine(FadeOut());
    }
    IEnumerator FadeOut()
    {
        cg.interactable = false;
        cg.blocksRaycasts = false;

        float t = 0;
        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime;
            cg.alpha = 1 - (t / fadeTime);
            yield return null;
        }

        cg.alpha = 0;
        upgradePanel.SetActive(false);
    }


    void OnCardClicked(int index)
    {
        if (currentChoices == null || index >= currentChoices.Count) return;

        var card = currentChoices[index];
        onPickCallback?.Invoke(card); // ส่งกลับไปให้ GameManager ? UpgradeSystem.ApplyUpgrade

        HideUpgrade();
    }

    public void OnClickRestart()
    {
        Time.timeScale = 1f;
        GameManager.I.Restart();
    }

    public void OnClickMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void PlayButtonClick()
    {
        if (uiAudioSource != null && buttonClickSfx != null)
        {
            uiAudioSource.PlayOneShot(buttonClickSfx);
        }
    }
}


