using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public enum SPEAKER_TYPE
{
    IMOUTO,
    PLAYER
}



public class ChatBubbleScript : MonoBehaviour 
{



    public void Initialize(string a_text, SPEAKER_TYPE a_speaker)
    {
        string temp_speakerPrefix = null;
       
        if (a_speaker == SPEAKER_TYPE.PLAYER)
        {
            temp_speakerPrefix = GameObject.Find("Player").GetComponent<PlayerStats>().playerName;
            GetComponent<Image>().color = UnityEngine.Color.cyan;
        }
        else if (a_speaker == SPEAKER_TYPE.IMOUTO)
        {
            temp_speakerPrefix = GameObject.Find("Object_Imouto").GetComponent<ImoutoObject>().imoutoName;
            GetComponent<Image>().color = UnityEngine.Color.magenta;
        }

        // Make the message box transparent
        Color c = GetComponent<Image>().color;
        GetComponent<Image>().color = new Color(c.r, c.g, c.b, 0.4f);
        
        // Set text of the chat bubble
        GetComponentInChildren<Text>().text = temp_speakerPrefix + "\n\t" + a_text;

        SetTransforms();


        StartCoroutine(Wait());
       
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(.03f);
        CheckMessageLimit();
    }


    /// <summary>
    /// Set the message as a child of the scrollable message list
    /// and normalize its scale
    /// </summary>
    void SetTransforms()
    {
        // Set add chat bubble to the list
        GameObject chatbubblelist = GameObject.Find("List_TextBubble");
        Vector3 scaleOne = new Vector3(1.0f, 1.0f, 1.0f);
        this.transform.parent = chatbubblelist.transform;
        this.transform.SetAsLastSibling();
        this.transform.localScale = scaleOne;
    }

    /// <summary>
    /// Check if the message list has reached its limit and
    /// delete the one at the top of the list
    /// </summary>
    void CheckMessageLimit()
    {
        if (GameObject.Find("List_TextBubble").transform.childCount > GameObject.Find("GameController").GetComponent<GlobalScripts>().ChatBubble_MaxMessages)
        {
            Destroy(GameObject.Find("List_TextBubble").transform.GetChild(0).gameObject);
        }

        GameObject.Find("ChatScrollBar").GetComponent<SetValueToZero>().SetToZero();
    }
}
        