using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SSLTest : MonoBehaviour
{
    public GameObject CopiedAlert;
    public Text URLLabel;
    public int CharacterLimit = 3000;

    string _cachedResult;
    Text _cachedText;

    private void Awake()
    {
        _cachedText = GetComponent<Text>();
        Refresh();
    }

    // Start is called before the first frame update
    public void Run(InputField field)
    {
        StartCoroutine(Coroutine(field.text));
    }

    void Refresh(string newText = "")
    {
        CopiedAlert?.SetActive(false);
        _cachedResult = null;
        _cachedText.color = Color.white;
        _cachedText.text = newText;
        if (URLLabel) URLLabel.text = string.Empty;
    }

    IEnumerator Coroutine(string url)
    {
        Refresh($"Sending Request to {url}...");

        UnityWebRequest req = UnityWebRequest.Get(url);
        var response = req.SendWebRequest();
        yield return response;
        try
        {
            if (req.isNetworkError)
            {
                _cachedText.color = Color.red;
                _cachedText.text = "Error: " + req.error;
                _cachedResult = req.error;
            }
            else
            {
                string responseText = req.downloadHandler.text;
                if (responseText.Length > CharacterLimit)
                    responseText = responseText.Substring(0, CharacterLimit);
                _cachedText.text = responseText;
                _cachedResult = responseText;
            }
        }
        catch(Exception e)
        {
            Debug.LogException(e);
            _cachedText.color = Color.red;
            _cachedText.text = "Exception: " + e.Message;
        }
        finally
        {
            if(URLLabel) URLLabel.text = url;
        }
    }

    public void CopyResultToClipboard()
    {
        if(_cachedResult == null)
        {
            Debug.LogError("No result to copy");
            return;
        }

        TextEditor tempEditor = new TextEditor
        {
            text = _cachedResult
        };
        tempEditor.SelectAll();
        tempEditor.Copy();
        CopiedAlert?.SetActive(true);
    }
}
