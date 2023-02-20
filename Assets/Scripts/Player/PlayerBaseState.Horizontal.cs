using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract partial class PlayerBaseState
{
    public virtual void EnterState(PlayerHorizontalStateManager manager) {}
    public virtual void FixedUpdateState(PlayerHorizontalStateManager manager) {}
}
