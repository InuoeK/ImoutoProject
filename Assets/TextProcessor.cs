using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TextProcessor : MonoBehaviour
{

    public GameObject textInput;
    ThreadedTextProcessing modWord_pos;
    ThreadedTextProcessing modWord_neg;

    float timer;

    bool running;

    List<string> fillerResponses = new List<string>();

    List<string> keywords = new List<string>();
    List<string> modifierwordsPos = new List<string>();
    List<string> modifierwordsNeg = new List<string>();
    List<string> intentwords = new List<string>();

    List<string> t1 = new List<string>();
    List<string> t2 = new List<string>();



    string textToProcess;

    void Start()
    {
        LoadScripts();
        textToProcess = null;
        string filepath = Application.dataPath + "/txt/";


   //     modWord_pos = new ThreadedTextProcessing(filepath + "test_modifierwords_pos.txt");
   //     modWord_neg = new ThreadedTextProcessing(filepath + "test_modifierwords_neg.txt");
        modWord_pos = new ThreadedTextProcessing(filepath + "test1.txt");
        modWord_neg = new ThreadedTextProcessing(filepath + "test1.txt");


       

        


    }

    void LoadScripts()
    {
        ScriptLoader sl = this.gameObject.GetComponent<ScriptLoader>();
        string filepath = Application.dataPath + "/txt/";
        sl.LoadScriptToList(filepath + "test1.txt", t1);
        sl.LoadScriptToList(filepath + "test1.txt", t2);
    }



    void TestProcessing(string a_stringToProcess)
    {
        float count = 0;
        List<string> matchesFound = new List<string>();

        a_stringToProcess = a_stringToProcess.ToLower();

        for (int i = 0; i < modifierwordsPos.Count; i++)
        {
            count += Time.deltaTime;

            if (a_stringToProcess.Contains(modifierwordsPos[i]))
                matchesFound.Add(modifierwordsPos[i]);
        }
    }


    public string ProcessTextAndGenerateResponse(string a_stringToProcess)
    {
        textToProcess = a_stringToProcess;
 
       // int matches = 0;

       //timer = 0f;
       //Debug.Log(timer);

       //Debug.Log(Time.realtimeSinceStartup);

       // for (int i = 0; i < t1.Count; i++)
       // {
       //     if(a_stringToProcess.Contains(t1[i]))
       //     matches++;
       //    // timer += Time.deltaTime;
           
       // }

       

        return "debug";
        //return fillerResponses[Random.Range(0, fillerResponses.Count)];
    }

    void ProcessAndInsertResponseIntoChatWindow()
    {
        
    }

    void Update()
    {
        if (textToProcess != null && textInput.active == true)
        {
            Debug.Log("Start:" + Time.realtimeSinceStartup);
            textInput.active = false;
            modWord_pos.SetTextToProcess(textToProcess);
            modWord_pos.Start();

            modWord_neg.SetTextToProcess(textToProcess);
            modWord_neg.Start();
        }

        if (textInput.active == false)
            timer += Time.deltaTime;

        if (modWord_neg.IsDone && modWord_pos.IsDone && textToProcess != null)
        {
            Debug.Log("Finish: " + Time.realtimeSinceStartup);
            // modWord_pos.DumpMatchedStrings();
            // modWord_neg.DumpMatchedStrings();

            textInput.active = true;
            textToProcess = null;

            // Debug.Log("Time required: " + timer);

        }
    }

}


public class ThreadedTextProcessing : MultiThreading
{
    private List<string> referenceStrings = new List<string>();
    public List<string> matchedStrings = new List<string>();
    private string stringToProcess;

    public ThreadedTextProcessing(string a_filepath)
    {
        ScriptLoader sl = new ScriptLoader();
        sl.LoadScriptToList(a_filepath, referenceStrings);
    }

    protected override void ThreadFunction()
    {
        for (int i = 0; i < referenceStrings.Count; i++)
        {
            if(stringToProcess.Contains(referenceStrings[i]))
            {
                matchedStrings.Add(referenceStrings[i]);
            }
        }

        Debug.Log("Total matches: " + matchedStrings.Count);
    }

    public void ResetMatchedStrings()
    {
        Debug.Log("Matched Strings reset");
        matchedStrings.Clear();
    }


    public void SetTextToProcess(string a_string)
    {
        ResetMatchedStrings();
        stringToProcess = a_string;
    }

    protected void OnFinished()
    {
        Debug.Log("Threaded job finished");
    }


    public void DumpMatchedStrings()
    {
        Debug.Log("Processing finished");
        Debug.Log("Dumping matched strings");
        string temp="";
        int i = 0;
        foreach (string s in matchedStrings)
        {
            i++;
            temp = temp + " " + i + ": " + s + "\t"; 
        }
        Debug.Log(temp);
    }

}