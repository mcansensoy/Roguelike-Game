using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPrimaryAttackState : PlayerState
{
    private float rotationSpeed;
    private float attackSpeed;
    private float tempSpeed;

    private float rotationProgress;
    private Quaternion initialRotation;
    private Quaternion targetRotation;

    private int comboCounter = 0;
    private float lastTimeAttacked;
    private float comboResetTime = 1.75f;

    private Vector3 direction;

    // Reference to AudioSource and sound effect
    //private AudioSource audioSource;
    //public AudioClip swordSwingSound;

    public PlayerPrimaryAttackState(PlayerController _controller, string _animBoolName) : base(_controller, _animBoolName)
    {
        //audioSource = _controller.GetComponent<AudioSource>();
    }

    public override void EnterState()
    {
        base.EnterState();
        controller.isAttacking = true;

        rotationSpeed = PlayerManager.instance.stat.CurrentRotationSpeed;
        attackSpeed = PlayerManager.instance.stat.CurrentAttackSpeed;

        rotationProgress = 0f;
        SetTargetRotation();

        // Combo logic
        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboResetTime) comboCounter = 0;

        //Debug.Log(comboCounter + "Girdi");

        controller.animator.SetInteger("ComboCounter", comboCounter);

        // Set animator speed
        tempSpeed = controller.animator.speed;
        if (attackSpeed != 1f) controller.animator.speed = attackSpeed;

        // Apply a force to simulate momentum
        controller.rb.AddForce(direction * 550f, ForceMode.Impulse);

        comboCounter++;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        // Smooth rotation towards target
        rotationProgress += rotationSpeed * 18f * Time.deltaTime;
        controller.transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, rotationProgress);
    }

    public override void ExitState()
    {
        base.ExitState();
        controller.animator.speed = tempSpeed;
        controller.isAttacking = false;
        lastTimeAttacked = Time.time;
        //Debug.Log(comboCounter + "Çýktý");
    }

    private void SetTargetRotation()
    {
        Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            direction = (hit.point - controller.transform.position).normalized;
            direction.y = 0; // Ensure the character stays upright
            targetRotation = Quaternion.LookRotation(direction);
        }
        else
        {
            targetRotation = controller.transform.rotation;
        }

        initialRotation = controller.transform.rotation;
    }

    /*public void SpawnSlashEffect()
    {
        GameObject slashEffect = null;
        

        switch (comboCounter)
        {
            case 0:
                slashEffect = controller.slashEffect1;
                effectRotation.Set(90f, 0f, 0f, 0f);
                break;
            case 1:
                slashEffect = controller.slashEffect2;
                effectRotation.Set(0f, 0f, 0f, 0f);
                break;
            case 2:
                slashEffect = controller.slashEffect3;
                effectRotation.Set(0f, 90f, 0f, 0f);
                break;
        }

        if (slashEffect != null)
        {
            // Assuming the weapon has a position where the effect should appear
            Vector3 effectPosition = controller.gameObject.transform.GetChild(0).transform.position;

            //Quaternion effectRotation = slashEffect.transform.rotation;
            effectRotation.y = controller.transform.rotation.y;

            // Instantiate the slash effect
            GameObject instantiatedEffect = GameObject.Instantiate(slashEffect, effectPosition, effectRotation);
            // Optionally, parent the effect to the weapon for more precise positioning
            //instantiatedEffect.transform.SetParent(controller.currentWeapon.transform);
        }
    }*/
}
