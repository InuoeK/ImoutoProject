using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public enum Transition
{
    NullTransition = 0,
    Transition_Angry,
    Transition_Annoyed,
    Transition_Interested,
    Transition_Happy,
}

public enum StateID
{
    NullState = 0,
    Neutral,
    Angry,
    Annoyed,
    Interested,
    Happy,
}

public class FSMSystem
{
    List<FSMState> states;


    private StateID currentStateID;
    public StateID CurrentStateID { get { return currentStateID; } }
    private FSMState currentState;
    public FSMState CurrentState { get { return currentState; } }

    public FSMSystem()
    {
        states = new List<FSMState>();
    }

    public void AddState(FSMState a_state)
    {
        if (a_state == null)
        {
            Debug.Log("Cannot add null state");
        }

        // Set added state to current state if there are no states in list
        if (states.Count == 0)
        {
            states.Add(a_state);
            currentState = a_state;
            currentStateID = a_state.ID;
        }

        states.Add(a_state);

    }

    public void DeleteState(StateID id)
    {
        if (id == StateID.NullState)
        {
            return;
        }

        foreach (FSMState state in states)
        {
            if (state.ID == id)
            {
                states.Remove(state);
                return;
            }
        }

        Debug.Log("State not found");
    }

    public void PerformTransition(Transition trans)
    {
        if (trans == Transition.NullTransition)
        {
            return;
        }

        StateID id = currentState.GetOutputState(trans);
        if (id == StateID.NullState)
        {
            return;
        }

        currentStateID = id;
        foreach (FSMState state in states)
        {
            if (state.ID == currentStateID)
            {
                currentState.DoBeforeLeaving();

                currentState = state;

                currentState.DoBeforeEntering();
                break;
            }
        }
    }




}

public abstract class FSMState
{
    protected Dictionary<Transition, StateID> map = new Dictionary<Transition, StateID>();
    protected StateID stateID;

    public StateID ID {get { return stateID; }}

    public void AddTransition(Transition a_trans, StateID a_id)
    {
        if (a_trans == Transition.NullTransition)
        {
            Debug.Log("STATE_ADD: Null transition not allowed");
            return;
        }

        if (a_id == StateID.NullState)
        {
            Debug.Log("STATE_ADD: Null State ID not allowed");
            return;
        }

        // Check if transition already exists in list
        if (map.ContainsKey(a_trans))
        {
            Debug.LogError("FSM Already contains transition");
            return;
        }

        map.Add(a_trans, a_id);
    }

    public void DeleteTransition(Transition a_trans)
    {
        if (a_trans == Transition.NullTransition)
        {
            Debug.Log("REMOVE: Null Transition not allowed");
            return;
        }


        // Remove transition if it exists
        if (map.ContainsKey(a_trans))
        {
            map.Remove(a_trans);
            return;
        }

        Debug.Log("REMOVE: Transition not found.");

    }

    public StateID GetOutputState(Transition a_trans)
    {
        if (map.ContainsKey(a_trans))
        {
            return map[a_trans];
        }
        return StateID.NullState;
    }

    public virtual void DoBeforeEntering() { }

    public virtual void DoBeforeLeaving() { }

    public abstract void Reason(GameObject npc);

    public abstract void Act(GameObject npc);

}



public class NeutralState : FSMState
{
    private float sway;
    public float Sway { get { return sway; } }
    private float high;
    public float High { get { return high; } set { high = value; } }

    private float low;
    public float Low { get { return low; } set { low = value; } }

    public NeutralState()
    {
        stateID = StateID.Neutral;
    }


    public override void Reason(GameObject npc)
    {
        if (sway > High)
        {
            // Transition to Interested
        }
        else if (sway < Low)
        {
            // Transition to Annoyed
        }
    }

    public override void Act(GameObject npc)
    {
        // Do stuff here
    }
}

