using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class List_ChatBubbleScript : MonoBehaviour
{
    int numMessages = 0;

    public int maxMessages = 100;

    public GameObject scrollbar;

    public void AddChatBubble(string a_text, SPEAKER_TYPE a_spt)
    {
        GameObject temp_chatBubble = Instantiate(Resources.Load("Textbubble") as GameObject);
        temp_chatBubble.GetComponent<ChatBubbleScript>().Initialize(a_text, a_spt);

        temp_chatBubble.GetComponentInChildren<SetNumber>().SetTextNumber(numMessages++);


        if (transform.childCount > maxMessages)
        {
            Destroy(transform.GetChild(0).gameObject);
        }



        // Ensure that the player's chat bubble appears before the game begins processing imouto's response
        if (a_spt == SPEAKER_TYPE.PLAYER)
        {
            GameObject.Find("Object_Imouto").GetComponent<ImoutoObject>().Respond(a_text);

        }
        

    }
}