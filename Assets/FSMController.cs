using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;


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

public class FSMController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
