using UnityEngine;
using Unity.Entities;

public class PlayerAuthoring : MonoBehaviour

{

    public float MoveSpeed = 5;
    public float ShootCooldown = 1;
    public GameObject BulletPrefab;

    public class PlayerBaker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)

        {
            Entity playerEntity = GetEntity(TransformUsageFlags.None);
            AddComponent(playerEntity, new PlayerComponent
            {
                MoveSpeed = authoring.MoveSpeed,
                ShootCoolDown = authoring.ShootCooldown,
                BulletPrefab = GetEntity(authoring.BulletPrefab, TransformUsageFlags.None)
            });
        }
    }

}

