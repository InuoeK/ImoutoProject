using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetNumber : MonoBehaviour {

    public void SetTextNumber(int a_int)
    {
        GetComponent<Text>().text = a_int.ToString();
    }
}
