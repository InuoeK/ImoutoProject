using UnityEngine;
using System.Collections;

public class DebugLogKeyInput : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Escape))
            this.gameObject.SetActive(false);
	}
}
