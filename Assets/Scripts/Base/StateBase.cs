using System;

public abstract class StateBase<T>
{
    public T stateType;

    public virtual void Init(FSMController<T> controller, T stateType)
    {
        this.stateType = stateType;
        OnInit(controller, stateType);
    }
    protected virtual void OnInit(FSMController<T> controller, T stateType) { }

    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnExit();
}
