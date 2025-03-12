using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UltimateSkillState : PlayerState
{
    private float rotationSpeed;
    private float rotationProgress;
    private Quaternion initialRotation;
    private Quaternion targetRotation;

    private UltimateSkill ultimateSkill;
    private int currentSkillNumber;

    public UltimateSkillState(PlayerController _controller, string _animBoolName) : base(_controller, _animBoolName) { }

    public override void EnterState()
    {
        base.EnterState();
        rotationSpeed = PlayerManager.instance.stat.CurrentRotationSpeed;

        rotationProgress = 0f;
        SetTargetRotation();

        // Start the ultimate skill
        ultimateSkill = SkillManager.instance.ultimateSkill;
        currentSkillNumber = ultimateSkill.currentUltimate;
        controller.animator.SetInteger("CurrentUltimate", currentSkillNumber);
        ultimateSkill.UseSkill();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        // Smooth rotation
        rotationProgress += rotationSpeed * 18f * Time.deltaTime;
        controller.transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, rotationProgress);
    }

    public override void ExitState()
    {
        base.ExitState();
        controller.isUsingSkill = false;
    }

    private void SetTargetRotation()
    {
        Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 direction = (hit.point - controller.transform.position).normalized;
            direction.y = 0; // Ensure the character stays upright
            targetRotation = Quaternion.LookRotation(direction);
        }
        else
        {
            targetRotation = controller.transform.rotation;
        }

        initialRotation = controller.transform.rotation;
    }
}
