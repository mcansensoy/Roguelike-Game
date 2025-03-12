using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerController controller;

    private string animBoolName;

    public PlayerState(PlayerController _controller, string _animBoolName)
    {
        this.controller = _controller;
        this.animBoolName = _animBoolName;
    }

    public virtual void EnterState()
    {
        controller.animator.SetBool(animBoolName, true);
    }

    public virtual void UpdateState()
    {

    }

    public virtual void ExitState()
    {
        controller.animator.SetBool(animBoolName, false);
    }
}
