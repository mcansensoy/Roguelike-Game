using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerRunState : PlayerState
{
    private float moveSpeed;
    private float rotationSpeed;
    public PlayerRunState(PlayerController _controller, string _animBoolName) : base(_controller, _animBoolName)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        moveSpeed = PlayerManager.instance.stat.CurrentMoveSpeed;
        rotationSpeed = PlayerManager.instance.stat.CurrentRotationSpeed;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (controller.MovementInput == Vector2.zero)
        {
            controller.TransitionToState(controller.IdleState);
        }
        /*else if (SkillManager.instance.dash.isDashing)
        {
            controller.TransitionToState(controller.DashState);
        }*/
        else
        {
            Move();
        }
    }

    private void Move()
    {
        Vector3 movement = new Vector3(controller.MovementInput.x, 0f, controller.MovementInput.y).normalized;

        Vector3 move = movement * moveSpeed * Time.deltaTime;
        controller.rb.MovePosition(controller.rb.position + move);

        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, targetRotation, rotationSpeed);
        }
    }
}
