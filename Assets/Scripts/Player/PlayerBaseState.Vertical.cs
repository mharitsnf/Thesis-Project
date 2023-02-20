using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract partial class PlayerBaseState
{
    public virtual void EnterState(VerticalStateController controller) {}
    public virtual void FixedUpdateState(VerticalStateController controller) {}
}
