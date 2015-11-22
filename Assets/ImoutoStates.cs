using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public enum Transition
{
    NullTransition = 0,
    Angry,
    Annoyed,
    Interested,
    Happy,
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
            Debug.Log("Current State assigned to: " + a_state.ID.ToString());
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
            Debug.Log("hello2");
            return;
        }

        StateID id = currentState.GetOutputState(trans);
        if (id == StateID.NullState)
        {
            Debug.Log("hello3");
            return;
        }

        currentStateID = id;
        foreach (FSMState state in states)
        {
            Debug.Log(state.ID.ToString() + " " + currentStateID.ToString());
            if (state.ID == currentStateID)
            {
                Debug.Log("Performing Transition");
                currentState.DoBeforeLeaving();

                currentState = state;

                currentState.DoBeforeEntering();
                break;
            }
        }

        Debug.Log("Transition Error: Output State does not exist");
        return;
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





