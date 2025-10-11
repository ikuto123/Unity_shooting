using AIState;
using UnityEngine;
using UnityEngine.AI;

public class AIMovement
{
    private readonly AIController _controller;
    private readonly NavMeshAgent _navMeshAgent;
    private readonly Rigidbody _rigidbody;

    public AIMovement(AIController controller)
    {
        _controller = controller;
        _navMeshAgent = controller.GetComponent<NavMeshAgent>();
        _rigidbody = controller.GetComponent<Rigidbody>();
        
        if (_navMeshAgent != null)
        {
            _navMeshAgent.updatePosition = false;
            _navMeshAgent.updateRotation = false;
        }
    }

    public void SetTarget(Vector3 destination)
    {
        if (_navMeshAgent != null && _navMeshAgent.isOnNavMesh)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(destination);
        }
    }
    
    public void Movement()
    {
        if (_navMeshAgent == null || _rigidbody == null) return;
        
        _navMeshAgent.speed = _controller.moveSpeed;
        _navMeshAgent.nextPosition = _rigidbody.position;

        if (!_navMeshAgent.hasPath || _navMeshAgent.isStopped)
        {
            _rigidbody.linearVelocity = new Vector3(0, _rigidbody.linearVelocity.y, 0);
            return;
        }
        
        Vector3 desiredVelocity = _navMeshAgent.velocity;
        _rigidbody.linearVelocity = new Vector3(desiredVelocity.x, _rigidbody.linearVelocity.y, desiredVelocity.z);
        
        Vector3 lookDirection = (_navMeshAgent.steeringTarget - _controller.transform.position).normalized;
        lookDirection.y = 0; 
        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            _rigidbody.MoveRotation(Quaternion.Slerp(_rigidbody.rotation, targetRotation, Time.fixedDeltaTime * 10f)); 
        }
    }
    
    public void Stop()
    {
        if (_navMeshAgent != null)
        {
            _navMeshAgent.isStopped = true;
        }
    }
}
