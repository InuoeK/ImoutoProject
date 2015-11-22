using UnityEngine;
using System.Collections;
using System.Collections.Generic;





public class ImoutoObject : MonoBehaviour
{


    private float sway;
    public float Sway { get { return this.sway; } set { this.sway += value; } }


    public string imoutoName;

    ImoutoFSM ifsm;


    // Use this for initialization
    void Start()
    {
        ifsm = new ImoutoFSM();

    }



    void Update()
    {

    }


    /// <summary>
    /// Respond to User message by selecting a message from specified script list
    /// </summary>
    public void Respond(string a_playerText)
    {
        //Process the user's text
        string response = "";

        response = GetComponent<TextProcessor>().ProcessTextAndGenerateResponse(a_playerText);

        GameObject.Find("List_TextBubble").GetComponent<List_ChatBubbleScript>().AddChatBubble(response, SPEAKER_TYPE.IMOUTO);

    }





}
