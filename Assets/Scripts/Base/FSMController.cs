using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSMController<T> : MonoBehaviour
{

    public T curState;
    protected StateBase<T> state;

    private Dictionary<T, StateBase<T>> stateDict = new Dictionary<T, StateBase<T>>();

    public void ChangeState<K>(T newState, bool ignoreSame = true) where K : StateBase<T>, new()
    {
        if (newState.Equals(curState) && ignoreSame)
            return;
        state?.OnExit();
        curState = newState;
        state = GetStateObj<K>(newState);
        state.OnEnter();
    }

    public void ChangeState<K>(bool ignoreSame = false) where K : StateBase<T>, new()
    {
        T newStateKey = (T)Enum.Parse(typeof(T), typeof(K).Name);
        ChangeState<K>(newStateKey, ignoreSame);
    }

    private StateBase<T> GetStateObj<K>(T stateType) where K : StateBase<T>, new()
    {
        if (stateDict.ContainsKey(stateType))
            return stateDict[stateType];
        StateBase<T> state = new K();
        state.Init(this, stateType);
        stateDict.Add(stateType, state);
        return state;
    }

    protected virtual void Update()
    {
        if (state != null)
            state.OnUpdate();
    }

}

