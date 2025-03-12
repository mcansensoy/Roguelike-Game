using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasicSkillState : PlayerState
{
    private float rotationSpeed;
    private float rotationProgress;
    private Quaternion initialRotation;
    private Quaternion targetRotation;

    private BasicSkill basicSkill;
    private int currentSkillNumber;

    public BasicSkillState(PlayerController _controller, string _animBoolName) : base(_controller, _animBoolName) { }

    public override void EnterState()
    {
        base.EnterState();
        rotationSpeed = PlayerManager.instance.stat.CurrentRotationSpeed;

        rotationProgress = 0f;
        SetTargetRotation();

        // Start the skill
        basicSkill = SkillManager.instance.basicSkill;
        currentSkillNumber = basicSkill.currentBasic;
        controller.animator.SetInteger("CurrentBasic", currentSkillNumber);
        basicSkill.UseSkill();
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
