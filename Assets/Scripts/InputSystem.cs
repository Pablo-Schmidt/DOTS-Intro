using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class InputSystem : SystemBase
{
    private Controls controls;

    protected override void OnCreate()
    {
        if (!SystemAPI.TryGetSingleton<InputComponent>(out InputComponent input))
        {
            EntityManager.CreateEntity(typeof(InputComponent));
        }
        controls = new Controls();
        controls.Enable();
    }

    protected override void OnUpdate()
    {
        // Use UnityEngine.Vector2 instead of System.Numerics.Vector2
        Vector2 moveVectorTemp = controls.ActionMap.Movement.ReadValue<Vector2>();
        float2 moveVector = new float2(moveVectorTemp.x, moveVectorTemp.y);

        Vector2 mousePositionTemp = controls.ActionMap.MousePosition.ReadValue<Vector2>();
        float2 mousePosition = new float2(mousePositionTemp.x, mousePositionTemp.y);

        bool isPressingLMB = controls.ActionMap.Shoot.ReadValue<float>() == 1;

        SystemAPI.SetSingleton(new InputComponent { mousePos = mousePosition, movement = moveVector, pressingLMB = isPressingLMB });
    }
}
