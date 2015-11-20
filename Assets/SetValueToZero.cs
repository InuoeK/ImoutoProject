using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetValueToZero : MonoBehaviour {

    public GameObject list;

	public void SetToZero()
    {

        list.GetComponent<ScrollRect>().verticalScrollbar.value = -10.0f;


        Debug.Log("Scroll bar value: " + list.GetComponent<ScrollRect>().verticalScrollbar.value);
    }

    void Update()
    {
       // Debug.Log(list.GetComponent<ScrollRect>().verticalScrollbar.value);
    }
}
