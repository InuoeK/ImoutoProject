using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextfieldInputScript : MonoBehaviour
{
    public GameObject list_chatBubble;
    public SoundModule sm;

    InputField ifield;

    public int maxMessages = 100;


    // Use this for initialization
    void Start()
    {
        ifield = this.gameObject.GetComponent<InputField>();
        ifield.onEndEdit.AddListener(value =>
        {
            if (Input.GetKeyDown(KeyCode.Return))
                SendStringToChatBubble();
        });
    }


    public void SendStringToChatBubble()
    {
        if (ifield.text != "")
        {
            GameObject.Find("List_TextBubble").GetComponent<List_ChatBubbleScript>().AddChatBubble(ifield.text, SPEAKER_TYPE.PLAYER);
            sm.PlaySound("popsound");
            ifield.text = "";
        }
    }


}
