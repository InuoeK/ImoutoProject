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
            StreamReader sr = new StreamReader(a_filepath);
            using (sr)
            {
                do
                {
                    string line = sr.ReadLine();
                    Debug.Log(line);
                    a_scriptList.Add(line);
                }
                while (!sr.EndOfStream) ;
                sr.Close();
                return true;
            }
        }
        catch (System.Exception e) { Debug.Log("Something Happened: " + e); }

        return false;
    }
}
