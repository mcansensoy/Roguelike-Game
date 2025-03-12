using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    private float dashTime;
    private Vector3 dashDirection;

    private DashSkill dashSkill;

    private float dashDuration;
    private float dashSpeed;
    private float currentDash;

    public PlayerDashState(PlayerController _controller, string _animBoolName) : base(_controller, _animBoolName) { }

    public override void EnterState()
    {
        base.EnterState();
        dashTime = 0f;
        SkillManager.instance.dash.UseSkill();

        dashSkill = SkillManager.instance.dash;

        dashDuration = dashSkill.dashDuration;

        dashSpeed = dashSkill.dashSpeed;

        currentDash = dashSkill.currentDash;

        // Set the dash direction to the current forward direction of the player
        dashDirection = controller.transform.forward;

        if(currentDash == 1)
        {
            // do teleporting dash movement
            dashTime = dashDuration / 2f;
            TeleportDash();
        }
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if(currentDash != 1)
        {
            Dash(); // if it is not teleporting dash, it will be move like a normal dash
        }     

        dashTime += Time.deltaTime;
        if (dashTime >= dashDuration)
        {
            //SkillManager.instance.dash.isDashing = false;

            // Transition back to Idle state after dashing
            controller.TransitionToState(controller.IdleState);
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        controller.isUsingSkill = false;

        if(currentDash != 1) SkillManager.instance.dash.CancelTrail();
    }

    private void Dash()
    {
        controller.rb.MovePosition(controller.rb.position + dashDirection * dashSpeed * Time.deltaTime);
    }

    private void TeleportDash()
    {
        Vector3 startPosition = controller.transform.position;

        // Calculate teleport endpoint
        Vector3 teleportEndpoint = startPosition + dashDirection * dashSpeed * dashDuration;

        // Teleport player to the endpoint
        controller.transform.position = teleportEndpoint;

        // Spawn smoke at the end of the teleport
        dashSkill.SpawnSmoke();
    }
}
