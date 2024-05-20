using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup m_TargetGroup;

    private List<Target> targets = new List<Target>();

    public Target CurrentTarget { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Target target))
        {
            target.OnDestroyed += RemoveTarget;
            targets.Add(target);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Target target))
        {
            RemoveTarget(target);
        }
    }

    private void RemoveTarget(Target target)
    {
        if(CurrentTarget == target)
        {
            m_TargetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }

        target.OnDestroyed -= RemoveTarget;
        targets.Remove(target);
    }

    public bool SelectTarget()
    {
        if (targets.Count == 0) return false;

        CurrentTarget = targets[0];
        m_TargetGroup.AddMember(CurrentTarget.transform, 1f, 2f);
        return true;
    }

    public void Cancel()
    {
        if (CurrentTarget == null) return;

        m_TargetGroup.RemoveMember(CurrentTarget.transform);
        CurrentTarget = null;
    }
}
