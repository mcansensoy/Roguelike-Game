using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerController _controller, string _animBoolName) : base(_controller, _animBoolName)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (controller.MovementInput != Vector2.zero)
        {
            controller.TransitionToState(controller.RunState);
        }
        /*else if (SkillManager.instance.dash.isDashing)
        {
            controller.TransitionToState(controller.DashState);
        }*/
    }
}
