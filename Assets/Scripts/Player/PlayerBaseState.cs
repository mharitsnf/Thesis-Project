public abstract class PlayerBaseState
{
    public virtual void EnterState(PlayerHorizontalStateManager manager) {}
    public virtual void EnterState(PlayerVerticalStateManager manager) {}
    public virtual void FixedUpdateState(PlayerHorizontalStateManager manager) {}
    public virtual void FixedUpdateState(PlayerVerticalStateManager manager) {}
}