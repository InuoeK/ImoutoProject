using UnityEngine;
using System.Collections;

public class HideStartingButtons : MonoBehaviour {

    public GameObject buttonsandpanel;
    public GameObject textinputgroup;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void HideThis()
    {
        gameObject.SetActive(false);
    }

    public void HidePanelAndText()
    {
        buttonsandpanel.SetActive(false);
    }

    public void ShowTextInputGroup()
    {
        textinputgroup.SetActive(true);
    }
}
