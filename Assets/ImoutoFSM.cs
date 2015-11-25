using UnityEngine;
using System.Collections;
using System;


public class EmotionState : FSMState
{

    private float high;
    public float High { get { return high; } set { high = value; } }

    private float low;
    public float Low { get { return low; } set { low = value; } }

    public EmotionState(StateID a_stateid)
    {
        stateID = a_stateid;
        high = low = 100000f;
    }

    public override void Act(GameObject npc)
    {
        // Do stuff here
        //  Debug.Log("Current State: " + stateID.ToString());
    }

    public override void Reason(GameObject npc)
    {
        float sway = npc.GetComponent<ImoutoObject>().Sway;
        if (sway >= high)
        {

            // Positive transition occured
            if (stateID == StateID.Angry)
            {
                npc.GetComponent<ImoutoFSM>().SetTransition(Transition.Annoyed);
            }
            else if (stateID == StateID.Annoyed)
            {
                npc.GetComponent<ImoutoFSM>().SetTransition(Transition.Interested);
            }
            else if (stateID == StateID.Interested)
            {
                npc.GetComponent<ImoutoFSM>().SetTransition(Transition.Happy);
            }
            else if (stateID == StateID.Neutral)
            {
                npc.GetComponent<ImoutoFSM>().SetTransition(Transition.Interested);
            }
            else if (stateID == StateID.Happy)
            {
                npc.GetComponent<ImoutoFSM>().SetTransition(Transition.Win);
            }
        }

        else if (sway <= low)
        {
            if (stateID == StateID.Annoyed)
            {
                npc.GetComponent<ImoutoFSM>().SetTransition(Transition.Angry);
            }
            else if (stateID == StateID.Interested)
            {
                npc.GetComponent<ImoutoFSM>().SetTransition(Transition.Annoyed);
            }
            else if (stateID == StateID.Neutral)
            {
                npc.GetComponent<ImoutoFSM>().SetTransition(Transition.Annoyed);
            }
            else if (stateID == StateID.Angry)
            {
                npc.GetComponent<ImoutoFSM>().SetTransition(Transition.Lose);
            }
        }

        //Debug.Log("Current Sway Value: " + sway);
    }

    public override void DoBeforeEntering()
    {
        // Reset sway value
        Debug.Log("Transition Occured, sway value reset");
        GameObject.Find("Object_Imouto").GetComponent<ImoutoObject>().Sway = -GameObject.Find("Object_Imouto").GetComponent<ImoutoObject>().Sway;

    }

    public override void DoBeforeLeaving()
    {

        if (stateID == StateID.Angry)
        {
            GameObject.Find("TextInputGroup").active = false;
        }
    }
}




public class ImoutoFSM : MonoBehaviour
{
    private FSMSystem fsm;

    public void SetTransition(Transition a_t) { fsm.PerformTransition(a_t); }
    GameObject gameoverscreen;
    GameObject winscreen;
    public void Start()
    {
        gameoverscreen = GameObject.Find("Gameover");
        gameoverscreen.active = false;
        winscreen = GameObject.Find("Win");
        winscreen.active = false;
        InitializeFSM();
    }

    private void InitializeFSM()
    {
        EmotionState neutral = new EmotionState(StateID.Neutral);
        neutral.High = 10f;
        neutral.Low = -10f;
        neutral.AddTransition(Transition.Interested, StateID.Interested);
        neutral.AddTransition(Transition.Annoyed, StateID.Annoyed);


        EmotionState interested = new EmotionState(StateID.Interested);
        interested.High = 10f;
        interested.Low = -10f;
        interested.AddTransition(Transition.Happy, StateID.Happy);
        interested.AddTransition(Transition.Annoyed, StateID.Annoyed);


        EmotionState annoyed = new EmotionState(StateID.Annoyed);
        annoyed.High = 10f;
        annoyed.Low = -10f;
        annoyed.AddTransition(Transition.Interested, StateID.Interested);
        annoyed.AddTransition(Transition.Angry, StateID.Angry);

        EmotionState angry = new EmotionState(StateID.Angry);
        angry.High = 12f;
        angry.Low = -8f;
        angry.AddTransition(Transition.Annoyed, StateID.Annoyed);
        angry.AddTransition(Transition.Lose, StateID.Lose);

        EmotionState happy = new EmotionState(StateID.Happy);
        happy.High = 10f;
        happy.AddTransition(Transition.Win, StateID.Win);

        EmotionState win = new EmotionState(StateID.Win);
        EmotionState lose = new EmotionState(StateID.Lose);




        fsm = new FSMSystem();

        fsm.AddState(neutral);
        fsm.AddState(annoyed);
        fsm.AddState(angry);
        fsm.AddState(happy);
        fsm.AddState(interested);
        fsm.AddState(lose);
        fsm.AddState(win);

        Debug.Log("Imouto FSM successfully Initialized");
    }

    public void FixedUpdate()
    {
        if (fsm.CurrentStateID == StateID.Lose && gameoverscreen.active == false)
        {
            gameoverscreen.active = true;
        }
        if (fsm.CurrentStateID == StateID.Win && gameoverscreen.active == false)
        {
            gameoverscreen.active = true;
        }
        fsm.CurrentState.Reason(this.gameObject);
        fsm.CurrentState.Act(this.gameObject);
    }

}