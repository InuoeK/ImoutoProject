using UnityEngine;
using System.Collections;

public class EnterWithoutAsking : MonoBehaviour {

public void SetImoutoAngry()
    {
        GameObject.Find("Object_Imouto").GetComponent<ImoutoObject>().SetState(IMOUTO_STATE.ANGRY);
    }
}
