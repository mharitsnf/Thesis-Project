using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract partial class PlayerBaseState
{
    public virtual void EnterState(PlayerVerticalStateManager manager) {}
    public virtual void FixedUpdateState(PlayerVerticalStateManager manager) {}
}
