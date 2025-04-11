using UnityEngine;

public class Character_Player : Character_Base
{
    [SerializeField] private ThirdPersonController thirdPersonController;
    [SerializeField] private ThirdPersonShooterController thirdPersonShooterController;
    [SerializeField] private InputManager inputManager;
    protected override void HandleDeath(AITargetType killedBy)
    {
        base.HandleDeath(killedBy);
        
        thirdPersonController.enabled = false;
        thirdPersonShooterController.enabled = false;
        inputManager.enabled = false;
        targetType = AITargetType.None;
    }
}
