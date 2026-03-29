using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsShowListener
{
    public static AdsManager I;

    [SerializeField] private string androidGameId = "6077504";
    [SerializeField] private string iosGameId = "";
    [SerializeField] private string rewardedAdUnitIdAndroid = "Rewarded_Android";
    [SerializeField] private string rewardedAdUnitIdIOS = "Rewarded_iOS";
    [SerializeField] private bool testMode = true;

    private string gameId;
    private string adUnitId;
    private bool isInitialized = false;

    void Awake()
    {
        if (I != null && I != this)
        {
            Destroy(gameObject);
            return;
        }

        I = this;
        DontDestroyOnLoad(gameObject);

#if UNITY_IOS
        gameId = iosGameId;
        adUnitId = rewardedAdUnitIdIOS;
#else
        gameId = androidGameId;
        adUnitId = rewardedAdUnitIdAndroid;
#endif
    }

    void Start()
    {
        InitializeAds();
    }

    public void InitializeAds()
    {
        if (isInitialized) return;

        if (string.IsNullOrWhiteSpace(gameId))
        {
            Debug.LogError("Unity Ads Game ID ยังไม่ได้ตั้งค่า");
            return;
        }

        Advertisement.Initialize(gameId, testMode, this);
    }

    public void ShowAd()
    {
        if (!isInitialized)
        {
            Debug.LogWarning("Ads ยัง initialize ไม่เสร็จ");
            return;
        }

        Advertisement.Show(adUnitId, this);
    }

    public void OnInitializationComplete()
    {
        isInitialized = true;
        Debug.Log("Ads initialize สำเร็จ");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        isInitialized = false;
        Debug.LogError($"Ads init fail: {error} - {message}");
    }

    public void OnUnityAdsShowComplete(string unityAdsAdUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (unityAdsAdUnitId != adUnitId) return;
        if (showCompletionState != UnityAdsShowCompletionState.COMPLETED) return;

        if (AnalyticsManager.I != null && GameManager.I != null)
        {
            AnalyticsManager.I.LogGameEndChoice(
                "revive",
                GameManager.I.lastWaveBeforeDeath,
                GameManager.I.lastPlayerLevelBeforeDeath,
                true
            );
        }

        if (GameManager.I != null)
        {
            GameManager.I.RevivePlayer();
        }
    }

    public void OnUnityAdsShowFailure(string unityAdsAdUnitId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Show Ads fail: {error} - {message}");
    }

    public void OnUnityAdsShowStart(string unityAdsAdUnitId)
    {
        Debug.Log("Ads started");
    }

    public void OnUnityAdsShowClick(string unityAdsAdUnitId) { }
}
