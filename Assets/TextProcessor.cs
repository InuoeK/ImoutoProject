using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class TextProcessor : MonoBehaviour
{

    public GameObject textInput;

    public DebugLogText dblt;

    public float intentWeight = 0.25f;
    public float modifierWeight = 0.10f;
    public float keywordWeight = 0.65f;

    ThreadedTextProcessing modWord_pos;
    ThreadedTextProcessing modWord_neg;

    float timer;

    bool running;

    List<string> fillerResponses = new List<string>();



    string textToProcess;

    ThreadedTextProcessingController ttpController = new ThreadedTextProcessingController();



    void Start()
    {
        LoadScripts();
    }

    void LoadScripts()
    {
        textToProcess = null;
        string filepath = Application.dataPath + "/txt/";

        ttpController.AddTTPModule(new ThreadedTextProcessing(filepath + "test_modifier_words_pos.txt", "Positive Modifier"));
        ttpController.AddTTPModule(new ThreadedTextProcessing(filepath + "test_modifier_words_neg.txt", "Negative Modifier"));
        ttpController.AddTTPModule(new ThreadedTextProcessing(filepath + "keyword_interested.txt", "Interested Keywords"));
        ttpController.AddTTPModule(new ThreadedTextProcessing(filepath + "negative_expletives.txt", "Swear Words"));
        ttpController.AddTTPModule(new ThreadedTextProcessing(filepath + "intent_words.txt", "Intent Words"));

        Debug.Log("Total number of strings loaded: " + ttpController.GetNumberOfWordsTotal());
        dblt.DebugLog("Total number of strings loaded: " + ttpController.GetNumberOfWordsTotal());
    }



    void TestProcessing(string a_stringToProcess)
    {
        List<string> matchesFound = new List<string>();

        a_stringToProcess = a_stringToProcess.ToLower();

    }


    public string ProcessTextAndGenerateResponse(string a_stringToProcess)
    {
        textToProcess = a_stringToProcess;

        return "debug";
    }

    void ThreadedCheck()
    {
        if (textToProcess != null && textInput.active == true)
        {
            textInput.active = false;

//            Debug.Log("1: " + Time.realtimeSinceStartup);
//            timer = Time.realtimeSinceStartup;
            ttpController.StartTTPTasks(textToProcess.ToLower());
        }



        if (ttpController.CheckIfTTPTaskFinished() && textToProcess != null)
        {
   //         Debug.Log("Time taken: " + (timer - Time.realtimeSinceStartup) * -1);

            NLPProcessScore();

            ttpController.DumpAllMatchedWords();


            textInput.active = true;
            textToProcess = null;

        }
    }

    void NLPProcessScore()
    {
        float swayValue = 0f;

        int numModifierWords =
         ttpController.GetMatchedStringFromModule("Positive Modifier") +
         ttpController.GetMatchedStringFromModule("Negative Modifier") +
         ttpController.GetMatchedStringFromModule("Swear Words");

        int numIntentWords = ttpController.GetMatchedStringFromModule("Intent Words");

        // Determine if keywords have been detected
        int numKeywords = ttpController.GetMatchedStringFromModule("Interested Keywords");


        Debug.Log("Total Modifier Words Matched: " + numModifierWords);



        if (numKeywords > 0)
        {
            // Process sway value if keyword detected
            swayValue = (numModifierWords * modifierWeight) + (numKeywords * keywordWeight) + (numIntentWords * intentWeight);
            if (ttpController.GetMatchedStringFromModule("Negative Modifier") > 0 || ttpController.GetMatchedStringFromModule("Swear Words") > 0)
                swayValue *= -1;
        }


        if (swayValue > 1.0)
            dblt.DebugLog("Positive Reaction, Sway Value: " + swayValue);
        //Debug.Log("Positive Reaction");
        else if (swayValue < 0.0)
            dblt.DebugLog("Negative Reaction, Sway Value: " + swayValue);
        //Debug.Log("Negative Reaction");
        else
            dblt.DebugLog("Neutral Reaction, Sway Value: " + swayValue);
       // Debug.Log("Neutral Reaction");
    }

    void MainThreadCheck()
    {
        if (textToProcess != null)
        {
            textInput.active = false;
            timer = Time.realtimeSinceStartup;
            ttpController.NonThreadedCheck(textToProcess.ToLower());
            Debug.Log("Time taken: " + (timer - Time.realtimeSinceStartup) * -1);
            textToProcess = null;
            textInput.active = true;
        }
    }


    void Update()
    {
        ThreadedCheck();
        // MainThreadCheck(); // Uncomment if there are issues with multithreaded checks
    }
}


public class ResponseGenerator
{
    List<ResponseContainer> rc = new List<ResponseContainer>();

    public void AddResponseContainer(ResponseContainer a_rcon)
    {
        rc.Add(a_rcon);
    }

    public void RemoveResponseContainer(string a_name)
    {
        for (int i = 0; i < rc.Count; i++)
        {
            if (rc[i].name == a_name)
            {
                Debug.Log("Response Container Removed");
                rc.RemoveAt(i);
            }
        }

        Debug.Log("No Response Container found for : " + a_name);
     
    }
}

public class ResponseContainer
{
    List<string> responses = new List<string>();
    public string name;

    public ResponseContainer(string a_filepathtoload, string a_name)
    {
        ScriptLoader sl = new ScriptLoader();
        sl.LoadScriptToList(a_filepathtoload, responses);
        name = a_name;
    }

    public int GetNumberOfResponses()
    {
        return responses.Count;
    }

    public string GetResponse(int a_index)
    {
        return responses[a_index];
    }
}


public class ThreadedTextProcessingController
{
    List<ThreadedTextProcessing> ttpList = new List<ThreadedTextProcessing>();




    /// <summary>
    /// Add a Threaded Text Processing Module to the list
    /// </summary>
    /// <param name="a_ttp"></param>
    public void AddTTPModule(ThreadedTextProcessing a_ttp)
    {
        ttpList.Add(a_ttp);
    }

    /// <summary>
    /// Outputs the name of al Threaded Text Processing Modules that are in the list
    /// </summary>
    public void PrintTTPModuleList()
    {
        string temp = "";
        for (int i = 0; i < ttpList.Count; i++)
        {
            temp = "\n" + temp + " " + i + 1 + ": " + ttpList[i].name;
        }

        Debug.Log("List of Text Processing Modules: " + temp);
    }

    /// <summary>
    /// Runs check with all loaded libraries on main thread
    /// </summary>
    public void NonThreadedCheck(string a_stringtoprocess)
    {
        for (int i = 0; i < ttpList.Count; i++)
        {
            ttpList[i].SetTextToProcess(a_stringtoprocess);
            ttpList[i].MainFunction();
        }
    }

    /// <summary>
    /// Starts all TTP tasks, requires a string to process
    /// </summary>
    public void StartTTPTasks(string a_stringtoprocess)
    {
        for (int i = 0; i < ttpList.Count; i++)
        {
            ttpList[i].SetTextToProcess(a_stringtoprocess);
            ttpList[i].Start();
        }
    }


    /// <summary>
    /// Checks if all Threaded Text Processing tasks are completed
    /// </summary>
    public bool CheckIfTTPTaskFinished()
    {
        for (int i = 0; i < ttpList.Count; i++)
        {
            if (!ttpList[i].IsDone)
                return false;
        }
        return true;
    }

    /// <summary>
    /// Outputs all matched words found from the last check
    /// </summary>
    public void DumpAllMatchedWords()
    {
        string final = "";
        for (int i = 0; i < ttpList.Count; i++)
        {
            string temp = "";
            for (int k = 0; k < ttpList[i].matchedStrings.Count; k++)
            {
                temp = temp + " " + (k + 1) + ": " + ttpList[i].matchedStrings[k] + "\t";
            }
            Debug.Log(ttpList[i].name + " Matched strings: " + temp + "\n\n");
        }

        // Debug.Log("Beginning Dump of all Matched Strings : " + final);
    }

    public int GetNumberOfWordsTotal()
    {
        int temp = 0;
        for (int i = 0; i < ttpList.Count; i++)
        {
            temp += ttpList[i].GetNumberOfReferenceStrings();
        }

        return temp;
    }

    public int GetMatchedStringFromModule(string a_ttpname)
    {
        for (int i = 0; i < ttpList.Count; i++)
        {
            if (ttpList[i].name == a_ttpname)
                return ttpList[i].matchedStrings.Count;
        }
        return -1;
    }

}




public class ThreadedTextProcessing : MultiThreading
{
    private List<string> referenceStrings = new List<string>();
    public List<string> matchedStrings = new List<string>();
    private string stringToProcess;
    public string name;


    public int GetNumberOfReferenceStrings()
    {
        return referenceStrings.Count;
    }

    public ThreadedTextProcessing(string a_filepath)
    {
        ScriptLoader sl = new ScriptLoader();
        sl.LoadScriptToList(a_filepath, referenceStrings);
    }

    public ThreadedTextProcessing(string a_filepath, string a_name)
    {
        ScriptLoader sl = new ScriptLoader();
        sl.LoadScriptToList(a_filepath, referenceStrings);

        name = a_name;

        Debug.Log(name + " Threaded Processing Module Loaded");
    }

    protected override void ThreadFunction()
    {
        MainFunction();
    }

    public void MainFunction()
    {
        for (int i = 0; i < referenceStrings.Count; i++)
        {
            if (stringToProcess.Contains(referenceStrings[i]))
            {
                matchedStrings.Add(referenceStrings[i]);
            }
        }
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
        Debug.Log("Processing finished, dumping matched strings");
        string temp = "";


        for (int i = 0; i < matchedStrings.Count; i++)
        {
            temp = temp + " " + i + ": " + matchedStrings[i] + "\t";
        }

        Debug.Log("Total Matches: " + matchedStrings.Count + temp);
    }



    public class ThreadedTextProcessingController
    {
        List<ThreadedTextProcessing> ttpList = new List<ThreadedTextProcessing>();

        /// <summary>
        /// Add a Threaded Text Processing Module to the list
        /// </summary>
        /// <param name="a_ttp"></param>
        public void AddTTPModule(ThreadedTextProcessing a_ttp)
        {
            ttpList.Add(a_ttp);
        }

        /// <summary>
        /// Outputs the name of al Threaded Text Processing Modules that are in the list
        /// </summary>
        public void PrintTTPModuleList()
        {
            string temp = "";
            for (int i = 0; i < ttpList.Count; i++)
            {
                temp = "\n" + temp + " " + i + 1 + ": " + ttpList[i].name;
            }

            Debug.Log("List of Text Processing Modules: " + temp);
        }

        /// <summary>
        /// Starts all TTP tasks, requires a string to process
        /// </summary>
        public void StartTTPTasks(string a_stringtoprocess)
        {
            for (int i = 0; i < ttpList.Count; i++)
            {
                ttpList[i].SetTextToProcess(a_stringtoprocess);
                ttpList[i].Start();
            }
        }


        /// <summary>
        /// Checks if all Threaded Text Processing tasks are completed
        /// </summary>
        public bool CheckIfTTPTaskFinished()
        {
            for (int i = 0; i < ttpList.Count; i++)
            {
                if (!ttpList[i].IsDone)
                    return false;
            }
            return true;
        }

        public void DumpAllMatchedWords()
        {
            string final = "";
            for (int i = 0; i < ttpList.Count; i++)
            {
                string temp = "";
                for (int k = 0; k < ttpList[i].matchedStrings.Count; k++)
                {
                    temp = temp + " " + (k + 1) + ": " + ttpList[i].matchedStrings[k] + "\t";
                }
                Debug.Log(ttpList[i].name + " Matched strings: " + temp + "\n\n");
            }

            // Debug.Log("Beginning Dump of all Matched Strings : " + final);
        }

    }

}