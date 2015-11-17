using UnityEngine;
using System.Collections;

public class SetImoutoState : MonoBehaviour {

    public void SetAngry()
    {
        GameObject.Find("Object_Imouto").GetComponent<ImoutoObject>().SetState(IMOUTO_STATE.ANGRY);
    }

    public void SetAnnoyed()
    {
        GameObject.Find("Object_Imouto").GetComponent<ImoutoObject>().SetState(IMOUTO_STATE.ANNOYED);
    }
}
