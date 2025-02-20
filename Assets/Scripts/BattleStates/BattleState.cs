using System;

public abstract class BattleState
{
    public Action OnEndState;
    
    public virtual void OnEnter()
    {
    }
    public virtual void Update()
    {
    }
    public virtual void OnExit()
    {
    }
}
