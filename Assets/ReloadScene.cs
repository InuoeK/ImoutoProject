using UnityEngine;
using System.Collections;

public class ReloadScene : MonoBehaviour
{

    public void ReloadMainScene()
    {
        Application.LoadLevel(0);
    }
}
