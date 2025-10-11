using AIState;
using UnityEngine;

public class AICharacterSensor
{
    
     private readonly AIController _controller;

    // コンストラクタでAIControllerへの参照を受け取る
    public AICharacterSensor(AIController controller)
    {
        _controller = controller;
    }

    //敵がいるかどうか
    public Transform FindEnemy()
    {
        // 必要な情報は_controllerから取得する
        Collider[] targetsInSight = Physics.OverlapSphere(_controller.transform.position, _controller.sightRange);

        foreach (var targetCollider in targetsInSight)
        {
            var targetTeam = targetCollider.GetComponent<CharactorTeam>();
            if (targetTeam == null || targetTeam.Team == _controller.MyTeam.Team) continue;
            
            Transform enemy = targetCollider.transform;
            Vector3 directionToEnemy = (enemy.position - _controller.transform.position).normalized;

            if (Vector3.Angle(_controller.transform.forward, directionToEnemy) < _controller.sightAngle / 2)
            {
                float distanceToEnemy = Vector3.Distance(_controller.transform.position, enemy.position);
                if (!Physics.Raycast(_controller.transform.position, directionToEnemy, distanceToEnemy, _controller.obstacleLayer))
                {
                    return enemy;
                }
            }
        }
        return null;
    }
    
}
