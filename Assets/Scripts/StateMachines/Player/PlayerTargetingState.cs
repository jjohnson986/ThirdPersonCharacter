using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    private readonly int TARGETINGBLENDTREE = Animator.StringToHash("TargetingBlendTree");

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
        if(_stateMachine.Targeter.CurrentTarget == null)
        {
            _stateMachine.ChangeState(new PlayerFreeLookState(_stateMachine));
            return;
        }
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
