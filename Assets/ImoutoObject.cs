using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public enum IMOUTO_STATE
{
    NEUTRAL,
    ANGRY,
    ANNOYED,
    INTERESTED,
    HAPPY
}

public class ImoutoObject : MonoBehaviour
{



    IMOUTO_STATE imoutoState;


    public string imoutoName;

    public void SetState(IMOUTO_STATE a_state)
    {
        imoutoState = a_state;
    }

    // Use this for initialization
    void Start()
    {
        imoutoState = IMOUTO_STATE.NEUTRAL;
    }



    /// <summary>
    /// Respond to User message by selecting a message from specified script list
    /// </summary>
    public void Respond(string a_playerText)
    {
        //Process the user's text
        string response="";

        response = GetComponent<TextProcessor>().ProcessTextAndGenerateResponse(a_playerText);

        GameObject.Find("List_TextBubble").GetComponent<List_ChatBubbleScript>().AddChatBubble(response, SPEAKER_TYPE.IMOUTO);

    }



}
