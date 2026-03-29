using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using UnityEngine.UnityConsent;

public class UGSInitializer : MonoBehaviour
{
    async void Awake()
    {
        try
        {
            ConsentState consentState = EndUserConsent.GetConsentState();
            consentState.AnalyticsIntent = ConsentStatus.Granted;
            EndUserConsent.SetConsentState(consentState);

            await UnityServices.InitializeAsync();

            Debug.Log("UGS Ready + Analytics consent granted");
        }
        catch (System.Exception e)
        {
            Debug.LogError("UGS init failed: " + e);
        }
    }
}
