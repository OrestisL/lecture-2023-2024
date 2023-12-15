using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class PatrolPosition : Command
{
    public Vector3 targetPosition;
    public Transform refTransform;
    public float targetOffset = 0.2f;

    public PatrolPosition(Vector3 target, Transform current)
    {
        targetPosition = target;
        refTransform = current;
    }

    public PatrolPosition(Vector3 target, Transform current, float offset)
    {
        targetPosition = target;
        refTransform = current;
        targetOffset = offset;
    }

    public override bool IsFinished => Vector3.Distance(targetPosition, refTransform.position) <= targetOffset;

    public override void Execute()
    {

    }
}
public class PatrolMovement : MonoBehaviour
{
    public List<PatrolPosition> positions;
    private PatrolPosition _currentPosition;
    public NavMeshAgent agent;
    [SerializeField] private int _currentPositionIndex = 0;

    private void Start()
    {
        if (!agent)
        {
            agent = GetComponent<NavMeshAgent>();
        }
    }

    public void AddPosition(Vector3 position)
    {
        PatrolPosition add = new PatrolPosition(position, transform, transform.lossyScale.z * 0.5f + 0.1f);
        positions.Add(add);

        if (_currentPosition == null)
        {
            _currentPosition = add;
            agent.SetDestination(add.targetPosition);
        }
    }

    public void RemovePosition(int index) 
    {
        if (index >= positions.Count)
            return;

        positions.RemoveAt(index);
    }

    public void Move() 
    {
        if (_currentPosition == null)
            return;

        if (!_currentPosition.IsFinished)
            return;

        _currentPositionIndex = (_currentPositionIndex + 1) % positions.Count;
        _currentPosition = positions[_currentPositionIndex];
        agent.SetDestination(_currentPosition.targetPosition);
    }

    private void Update()
    {
        Move();
    }
}
