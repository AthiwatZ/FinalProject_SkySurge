using UnityEngine;
using Unity.Services.Analytics;
using System.Collections.Generic;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager I;

    void Awake()
    {
        if (I != null && I != this)
        {
            Destroy(gameObject);
            return;
        }

        I = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LogGameEndChoice(string choice, int wave, int playerLevel, bool usedAd)
    {
        try
        {
            CustomEvent myEvent = new CustomEvent("game_end_choice")
            {
                { "choice", choice },
                { "wave", wave },
                { "player_level", playerLevel },
                { "used_ad", usedAd }
            };

            AnalyticsService.Instance.RecordEvent(myEvent);
            Debug.Log($"Analytics sent: game_end_choice | {choice} | wave={wave} | level={playerLevel} | used_ad={usedAd}");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Analytics send failed: " + e);
        }
    }

    public void LogWaveComplete(int wave, int playerLevel)
    {
        try
        {
            CustomEvent myEvent = new CustomEvent("wave_complete")
            {
                { "wave", wave },
                { "player_level", playerLevel }
            };

            AnalyticsService.Instance.RecordEvent(myEvent);
            Debug.Log($"Analytics sent: wave_complete | wave={wave} | level={playerLevel}");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Analytics send failed: " + e);
        }
    }

    public void LogWaveFail(int wave, int playerLevel)
    {
        try
        {
            CustomEvent myEvent = new CustomEvent("wave_fail")
            {
                { "wave", wave },
                { "player_level", playerLevel }
            };

            AnalyticsService.Instance.RecordEvent(myEvent);
            Debug.Log($"Analytics sent: wave_fail | wave={wave} | level={playerLevel}");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Analytics send failed: " + e);
        }
    }
}
