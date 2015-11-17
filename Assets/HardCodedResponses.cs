using UnityEngine;
using System.Collections;

public class HardCodedResponses : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void RespondToKnock()
    {
        GameObject.Find("List_TextBubble").GetComponent<List_ChatBubbleScript>().AddChatBubble("What is it?", SPEAKER_TYPE.IMOUTO);
    }

    public void RespondToEntryWithoutKnock()
    {
        GameObject.Find("List_TextBubble").GetComponent<List_ChatBubbleScript>().AddChatBubble("Do you know how to knock?", SPEAKER_TYPE.IMOUTO);
    }

    public void RespondToEntryWithKnock()
    {
        GameObject.Find("List_TextBubble").GetComponent<List_ChatBubbleScript>().AddChatBubble("I don't remember saying you could come in.", SPEAKER_TYPE.IMOUTO);
    }
}
