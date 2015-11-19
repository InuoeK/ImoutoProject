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

        ttpController.AddTTPModule(new ThreadedTextProcessing(filepath + "test_modifier_words_pos.txt", "Positive Modifier Check"));
        ttpController.AddTTPModule(new ThreadedTextProcessing(filepath + "test_modifier_words_neg2.txt", "Negative Modifier Check"));
        ttpController.AddTTPModule(new ThreadedTextProcessing(filepath + "keyword_interested.txt", "Interested Keywords Check"));
        ttpController.AddTTPModule(new ThreadedTextProcessing(filepath + "negative_expletives.txt", "Swear Words Check"));

        Debug.Log("Total number of strings loaded: " + ttpController.GetNumberOfWordsTotal());
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

            Debug.Log("1: " + Time.realtimeSinceStartup);


            timer = Time.realtimeSinceStartup;
            ttpController.StartTTPTasks(textToProcess.ToLower());
        }



        if (ttpController.CheckIfTTPTaskFinished() && textToProcess != null)
        {
            Debug.Log("Time taken: " + (timer - Time.realtimeSinceStartup) * -1);

            ttpController.DumpAllMatchedWords();

            textInput.active = true;
            textToProcess = null;

        }
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

        // MainThreadCheck(); // Uncheck if there are issues with multithreaded checks




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