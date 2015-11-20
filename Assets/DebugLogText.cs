using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DebugLogText : MonoBehaviour {
    string thisText;
	// Use this for initialization
	void Start () {
        thisText = "";
	}
	
    void UpdateText()
    {
        GetComponent<Text>().text = thisText;
    }

    public void DebugLog(string a_string)
    {
        thisText = thisText + "\n" +  System.DateTime.Now.Hour+ ":"+ System.DateTime.Now.Minute+ ":"+ System.DateTime.Now.Second+ ": "  + "\n" + a_string + "\n";
        UpdateText();
    }

}
