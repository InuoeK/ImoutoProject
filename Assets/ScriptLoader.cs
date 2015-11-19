using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System.Collections.Generic;


public class ScriptLoader : MonoBehaviour
{

    public bool LoadScriptToList(string a_filepath, List<string> a_scriptList)
    {
        try
        {
            int counter = 0;
            StreamReader sr = new StreamReader(a_filepath);
            using (sr)
            {
                do
                {
                    string line = sr.ReadLine();
                    //Debug.Log(line);
                    counter++;
                    a_scriptList.Add(line);
                }
                while (!sr.EndOfStream) ;
                sr.Close();
                //Debug.Log(counter + " strings loaded from: " + a_filepath);
                return true;
            }
        }
        catch (System.Exception e) { Debug.Log("Something Happened: " + e); }

        Debug.Log("Failed to load: " + a_filepath);
        return false;
    }
}
