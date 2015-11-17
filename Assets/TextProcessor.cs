using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TextProcessor : MonoBehaviour {

    List<string> positiveModifiers = new List<string>();

    List<string> neutralKeywords = new List<string>();

    List<string> neutralResponses = new List<string>();

    List<string> fillerResponses = new List<string>();

    void Start()
    {
        LoadScripts();
    }

    void LoadScripts()
    {
        ScriptLoader sl = this.gameObject.GetComponent<ScriptLoader>();
        string filepath = Application.dataPath + "/txt/";
        // Load filler text
        if (sl.LoadScriptToList(filepath + "imouto_filler_scripts.txt", fillerResponses))
            Debug.Log("Filler Scripts loaded successfully");
        else
            Debug.Log("Filler Scripts failed to Load");
    }

    public string ProcessTextAndGenerateResponse(string a_stringToProcess)
    {
        return fillerResponses[Random.Range(0, fillerResponses.Count)];
    }

}
