using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEditorInternal;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    private readonly int FREE_LOOK_SPEED = Animator.StringToHash("FreeLookSpeed");
    private readonly int FREELOOKHASH = Animator.StringToHash("FreeLookBlendTree");

    private const float ANIMATOR_DAMP_TIME = 0.1f;

    public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        _stateMachine.Animator.Play(FREELOOKHASH);
        _stateMachine.InputManager.TargetEvent += OnTarget;
    }

    public override void Exit()
    {
        _stateMachine.InputManager.TargetEvent -= OnTarget;
    }

    private void OnTarget()
    {
        if(_stateMachine.Targeter.SelectTarget())
        {
            _stateMachine.ChangeState(new PlayerTargetingState(_stateMachine));
        }
    }

    public override void Update(float deltaTime)
    {
        Vector3 movement = CalculateMovement();

        _stateMachine.Controller.Move(movement * deltaTime * _stateMachine.FreeLookMovementSpeed);
        if(_stateMachine.InputManager.MovementValue == Vector2.zero)
        {
            _stateMachine.Animator.SetFloat(FREE_LOOK_SPEED, 0f, ANIMATOR_DAMP_TIME, deltaTime);
            return;
        }

        _stateMachine.Animator.SetFloat(FREE_LOOK_SPEED, 1f, ANIMATOR_DAMP_TIME, deltaTime);
        FaceMovementDirection(movement, deltaTime);
    }

    private void OnJump()
    {
        _stateMachine.ChangeState(new PlayerFreeLookState(_stateMachine));
    }

    private Vector3 CalculateMovement()
    {
        Vector3 forward = _stateMachine.CameraTransform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = _stateMachine.CameraTransform.right;
        right.y = 0;
        right.Normalize();

        return forward * _stateMachine.InputManager.MovementValue.y +
            right * _stateMachine.InputManager.MovementValue.x;
    }

    private void FaceMovementDirection(Vector3 direction, float deltaTime)
    {
        _stateMachine.transform.rotation = Quaternion.Lerp(_stateMachine.transform.rotation,
                                            Quaternion.LookRotation(direction),
                                            deltaTime * _stateMachine.RotationDamping);
    }
}
