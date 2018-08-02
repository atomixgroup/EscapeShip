using System.Collections;
using System.Collections.Generic;
using TapsellSDK;
using TapsellSimpleJSON;
using UnityEngine;
using UnityEngine.UI;

public class AdsController : MonoBehaviour
{
    public static bool available = false;
    public static bool bannerIsHidden = true;
    public static TapsellAd ad = null;
    public static TapsellNativeBannerAd nativeAd = null;
    public GameObject countDown;
    public GameObject watchAd;
    private bool isShow = false;
    private GameController gm;
    public static bool allow = true;
    // Use this for initialization
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        Tapsell.initialize("pfngctibpmqikpksakojsqtdekhctoearehjsiphnekqibnqbockjelqdfaadrognqeiod");

        Debug.Log("Tapsell Version: " + Tapsell.getVersion());
        Tapsell.setDebugMode(true);
        Tapsell.setPermissionHandlerConfig(Tapsell.PERMISSION_HANDLER_AUTO);
        
           
        Tapsell.setRewardListener(
            (TapsellAdFinishedResult result) =>
            {
                // onFinished, you may give rewards to user if result.completed and result.rewarded are both True
                Debug.Log("onFinished, adId:" + result.adId + ", zoneId:" + result.zoneId + ", completed:" +
                          result.completed + ", rewarded:" + result.rewarded);

                // You can validate suggestion from you server by sending a request from your game server to tapsell, passing adId to validate it
                if (result.completed && result.rewarded)
                {
                    validateSuggestion(result.adId);
                }
            }
        );
        requestAd("5af42da0ada66f0001c9061c", false);
    }

    public void ShowAd()
    {
        
        if (allow)
        {
            isShow = true;
            countDown.SetActive( false);
            watchAd.SetActive( false);
            
        }
        else
        {
            gm.MenuPressed();
        }
       
    }

    private void Update()
    {
        if (available && isShow )
        {
            available = false;
            TapsellShowOptions options = new TapsellShowOptions();
            options.backDisabled = false;
            options.immersiveMode = false;
            options.rotationMode = TapsellShowOptions.ROTATION_LOCKED_PORTRAIT;
            options.showDialog = true;
            Tapsell.showAd(ad, options);
            isShow = false;
            allow = false;
        }
    }


    public void validateSuggestion(string suggestionId)
    {
        try
        {
            string ourPostData = "{\"suggestionId\":\"" + suggestionId + "\"}";
            System.Collections.Generic.Dictionary<string, string> headers =
                new System.Collections.Generic.Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");

            byte[] pData = System.Text.Encoding.ASCII.GetBytes(ourPostData.ToCharArray());

            WWW api = new WWW("http://api.tapsell.ir/v2/suggestions/validate-suggestion", pData, headers);
            StartCoroutine(WaitForRequest(api));
        }
        catch (UnityException ex)
        {
            Debug.Log(ex.Message);
        }

        return;
    }

    IEnumerator WaitForRequest(WWW data)
    {
        yield return data; // Wait until the download is done
        if (data.error != null)
        {
            Debug.Log("my server error is " + data.error);
        }
        else
        {
            Debug.Log("my server result is " + data.text);

            JSONNode node = JSON.Parse(data.text);
            ScoreCounter.enable = true;
            bool valid = node["valid"].AsBool;
            if (valid)
            {
                gm.ContinueGame();
                
            }
            else
            {
                gm.MenuPressed();
            }
        }
    }

    private void requestAd(string zone, bool cached)
    {
        Tapsell.requestAd(zone, cached,
            (TapsellAd result) =>
            {
                // onAdAvailable
                Debug.Log("Action: onAdAvailable");
                available = true;
                ad = result;
            },
            (string zoneId) =>
            {
                // onNoAdAvailable
                Debug.Log("No Ad Available");
            },
            (TapsellError error) =>
            {
                // onError
                Debug.Log(error.error);
            },
            (string zoneId) =>
            {
                // onNoNetwork
                Debug.Log("No Network: " + zoneId);
            },
            (TapsellAd result) =>
            {
                //onExpiring
                Debug.Log("Expiring");
                available = false;
                ad = null;
                requestAd(result.zoneId, false);
            }
        );
    }
}