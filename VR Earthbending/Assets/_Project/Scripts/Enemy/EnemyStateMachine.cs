using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState[] states;
    public EnemyAgent agent;
    public EnemyStateID currentState;

    public EnemyStateMachine(EnemyAgent agent)
    {
        this.agent = agent;
        int numStates = System.Enum.GetNames(typeof(EnemyStateID)).Length;
        states = new EnemyState[numStates];
    }

    public void RegisterState(EnemyState state)
    {
        int index = (int)state.GetID();
        states[index] = state;
    }

    public EnemyState GetState(EnemyStateID stateID)
    {
        int index = (int)stateID;
        return states[index];
    }
    public void Update()
    {
        GetState(currentState)?.Update(agent);
    }

    public void ChangeState(EnemyStateID newState)
    {
        GetState(currentState)?.Exit(agent);
        currentState = newState;
        GetState(currentState)?.Enter(agent);
    }
}
