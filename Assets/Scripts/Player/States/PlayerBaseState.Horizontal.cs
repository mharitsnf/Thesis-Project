using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract partial class PlayerBaseState
{
    public virtual void EnterState() {}
    public virtual void FixedUpdateState() {}
    public virtual void ExitState() {}
}
