using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public InputManager InputManager { get; private set; }
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public Targeter Targeter { get; private set; }
    [field: SerializeField] public float FreeLookMovementSpeed { get; private set; }
    [field: SerializeField] public float RotationDamping { get; private set; }

    public Transform CameraTransform { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        CameraTransform = Camera.main.transform;
        ChangeState(new PlayerFreeLookState(this));
    }
}
