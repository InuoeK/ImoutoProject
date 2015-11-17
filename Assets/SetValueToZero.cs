using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetValueToZero : MonoBehaviour {

    public GameObject list;

	public void SetToZero()
    {
        Canvas.ForceUpdateCanvases();
        list.GetComponent<ScrollRect>().verticalScrollbar.value = -10.0f;
        Canvas.ForceUpdateCanvases();
    }
}
