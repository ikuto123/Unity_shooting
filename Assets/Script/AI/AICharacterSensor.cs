using AIState;
using UnityEngine;

public class AICharacterSensor
{
    
     private readonly AIController _aiController;
     
    public AICharacterSensor(AIController controller)
    {
        _aiController = controller;
    }

    //敵がいるかどうか
    public Transform FindEnemy()
    {
        //必要な情報は_AIControllerから取得する
        //視界範囲内のすべてのコライダーを取得し敵の情報を取得
        Collider[] targetsInSight = Physics.OverlapSphere(_aiController.transform.position, _aiController.sightRange);

        foreach (var targetCollider in targetsInSight)
        {
            var targetTeam = targetCollider.GetComponent<CharactorTeam>();
            //敵か味方かの判定、味方ならスキップ
            if (targetTeam == null || targetTeam.Team == _aiController.MyTeam.Team) continue;
            
            Transform enemy = targetCollider.transform;
            Vector3 directionToEnemy = (enemy.position - _aiController.transform.position).normalized;

            //視界角内にいるかどうか
            if (Vector3.Angle(_aiController.transform.forward, directionToEnemy) < _aiController.sightAngle / 2)
            {
                float distanceToEnemy = Vector3.Distance(_aiController.transform.position, enemy.position);
                //障害物がないかどうか(Wall)
                if (!Physics.Raycast(_aiController.transform.position, directionToEnemy, distanceToEnemy, _aiController.obstacleLayer))
                {
                    return enemy;
                }
            }
        }
        return null;
    }
    
}
