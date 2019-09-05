using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SSLTest : MonoBehaviour
{
    public string url = "https://www.twitch.tv";
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Coroutine());
    }

    IEnumerator Coroutine()
    {
        UnityWebRequest req = UnityWebRequest.Get(url);
        var response = req.SendWebRequest();
        yield return response;
        var text = GetComponent<Text>();
        if (req.isNetworkError)
        {
            text.text = "Error: " + req.error;
        }
        else
        {
            string responseText = req.downloadHandler.text;
            if (responseText.Length > 5000)
                responseText = responseText.Substring(0, 5000);
            text.text = responseText;
        }
    }
}
