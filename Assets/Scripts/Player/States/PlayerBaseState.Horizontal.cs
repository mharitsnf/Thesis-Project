using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract partial class PlayerBaseState
{
    public virtual void EnterState(HorizontalStateController controller) {}
    public virtual void FixedUpdateState(HorizontalStateController controller) {}
}
