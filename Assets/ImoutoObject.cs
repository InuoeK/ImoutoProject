using UnityEngine;
using System.Collections;
using System.Collections.Generic;





public class ImoutoObject : MonoBehaviour
{


    private float sway;
    public float Sway { get { return this.sway; } set { this.sway += value; } }


    public string imoutoName;

    ImoutoFSM ifsm;

    List<string> noKeywordResponses = new List<string>();
    List<string> neutralResponses = new List<string>();
    List<string> annoyedResponses = new List<string>();
    List<string> positiveResponses = new List<string>();

    List<string> negativeResponsesOne = new List<string>();



    // Use this for initialization
    void Start()
    {
        ifsm = new ImoutoFSM();
        LoadResponses();
    }


    void LoadResponses()
    {
        ScriptLoader sl = new ScriptLoader();
        string filepath = Application.dataPath + "/txt/";
        sl.LoadScriptToList(filepath + "responseNeutral.txt", neutralResponses);
        sl.LoadScriptToList(filepath + "responseNegative1.txt", negativeResponsesOne);
        sl.LoadScriptToList(filepath + "responseNoKeyword.txt", noKeywordResponses);
        sl.LoadScriptToList(filepath + "responsePositive.txt", positiveResponses);
    }



    /// <summary>
    /// Respond to User message by selecting a message from specified script list
    /// </summary>
    public void Respond(string a_playerText)
    {
        //Process the user's text
        string response = "";

        response = GetComponent<TextProcessor>().ProcessTextAndGenerateResponse(a_playerText);

        //GameObject.Find("List_TextBubble").GetComponent<List_ChatBubbleScript>().AddChatBubble(response, SPEAKER_TYPE.IMOUTO);




    }

    bool CheckQuestionResponse(string a_s)
    {
        if (a_s.Contains("Do") || a_s.Contains("How") || a_s.Contains("What") || a_s.Contains("Could"))
        {
            return true;
        }
        return false;
    }

    public void GenerateResponse(float calculatedSway, int numKeywords)
    {
       // Debug.Log("Generating Response");
 
        string response = "";

        if (numKeywords <= 0 || calculatedSway == 0f)
        {
            int rand = Random.Range(0, noKeywordResponses.Count);
            response = noKeywordResponses[rand];
        }  
        else if (calculatedSway < 0f) // Negative response
        {
            int rand = Random.Range(0, negativeResponsesOne.Count);
            response = negativeResponsesOne[rand];
        }
        else if (calculatedSway > 0f) // Positive response
        {
            int rand = Random.Range(0, positiveResponses.Count);
            response = positiveResponses[rand];
        }
        // Simple check for appending extras
        if (CheckQuestionResponse(response))
        {
            if (Random.Range(0, 10) < 2)
            {
                response += ", " + GameObject.Find("Player").GetComponent<PlayerStats>().playerName + "?";
            }
            else
                response += "?";
        }
        else
        {
            if (Random.Range(0, 10) < 2)
            {

                response += ", " + GameObject.Find("Player").GetComponent<PlayerStats>().playerName + ".";
            }
            else
                response += ".";
        }



        GameObject.Find("List_TextBubble").GetComponent<List_ChatBubbleScript>().AddChatBubble(response, SPEAKER_TYPE.IMOUTO);
    }





}
