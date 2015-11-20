using UnityEngine;
using System.Collections;

public class ShowDebugLog : MonoBehaviour {
    public GameObject dbLog;
    
    public void ToggleDebugLog()
    {
        dbLog.active = !dbLog.active;
    }
}
