using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerTargetingState : PlayerBaseState
{
    private readonly int TARGETINGBLENDTREE = Animator.StringToHash("TargetingBlendTree");
    private readonly int TARGETINGFORWARDHASH = Animator.StringToHash("TargetingForwardSpeed");
    private readonly int TARGETINGRIGHTHASH = Animator.StringToHash("TargetingRightSpeed");

    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        _stateMachine.Animator.Play(TARGETINGBLENDTREE);
        _stateMachine.InputManager.TargetEvent += OnCancel;
    }

    public override void Update(float deltaTime)
    {
        if (_stateMachine.Targeter.CurrentTarget == null)
        {
            _stateMachine.ChangeState(new PlayerFreeLookState(_stateMachine));
            return;
        }

        Vector3 movement = CalculateMovement();
        Move(movement * _stateMachine.TargetingMovementSpeed, deltaTime);

        UpdateAnimator(deltaTime);

        FaceTarget();
    }

    private Vector3 CalculateMovement()
    {
        Vector3 movement = Vector3.zero;

        movement += _stateMachine.transform.right * _stateMachine.InputManager.MovementValue.x;
        movement += _stateMachine.transform.forward * _stateMachine.InputManager.MovementValue.y;

        return movement;
    }   

    private void UpdateAnimator(float deltaTime)
    {
        //if(_stateMachine.InputManager.MovementValue.y == 0)
        //{
        //    _stateMachine.Animator.SetFloat(TARGETINGFORWARDHASH, 0, 0.1f, deltaTime);
        //}
        //else
        //{
        //    float value = _stateMachine.InputManager.MovementValue.y > 0 ? 1f : -1f;
        //    _stateMachine.Animator.SetFloat(TARGETINGFORWARDHASH, value, 0.1f, deltaTime);
        //}

        //if (_stateMachine.InputManager.MovementValue.x == 0)
        //{
        //    _stateMachine.Animator.SetFloat(TARGETINGRIGHTHASH, 0, 0.1f, deltaTime);
        //}
        //else
        //{
        //    float value = _stateMachine.InputManager.MovementValue.x > 0 ? 1f : -1f;
        //    _stateMachine.Animator.SetFloat(TARGETINGRIGHTHASH, value, 0.1f, deltaTime);
        //}

        _stateMachine.Animator.SetFloat(TARGETINGFORWARDHASH, _stateMachine.InputManager.MovementValue.y, 0.1f, deltaTime);
        _stateMachine.Animator.SetFloat(TARGETINGRIGHTHASH, _stateMachine.InputManager.MovementValue.x, 0.1f, deltaTime);
    }

    public override void Exit()
    {
        _stateMachine.InputManager.TargetEvent -= OnCancel;
    }

    private void OnCancel()
    {
        _stateMachine.Targeter.Cancel();
        _stateMachine.ChangeState(new PlayerFreeLookState(_stateMachine));
    }

}
