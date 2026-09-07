using UnityEngine;

/// <summary>
/// One shot, straight down the crosshair.
/// Everything else it needs already lives in Weapon.
/// </summary>
public class Pistol : Weapon
{
    protected override void Fire()
    {
        FireRay(aimSource.forward);
    }
}
