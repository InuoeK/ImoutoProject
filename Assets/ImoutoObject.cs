using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class ImoutoFSM
{
    private FSMSystem fsm;

    public void SetTransition(Transition a_t) { fsm.PerformTransition(a_t); }

    public void Start()
    {
        InitializeFSM();
    }

    private void InitializeFSM()
    {
        NeutralState neutral = new NeutralState();
        neutral.Low = -10.0f;
        neutral.High = 10.0f;


        fsm = new FSMSystem();

        fsm.AddState(neutral);


        Debug.Log("Imouto FSM successfully Initialized");

        DebugLogText dblt = new DebugLogText();

        dblt.DebugLog("Imouto FSM successfully Initialized");


    }
}

public class ImoutoObject : MonoBehaviour
{


    private float sway;
    public float Sway { get { return sway; } }


    public string imoutoName;


    // Use this for initialization
    void Start()
    {


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
