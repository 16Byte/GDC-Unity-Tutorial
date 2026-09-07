using UnityEngine;

/// <summary>
/// Same contract as the pistol, genuinely different behavior.
/// Several rays per trigger pull, each nudged off center.
/// </summary>
public class Shotgun : Weapon
{
    [Header("Shotgun only")]
    [SerializeField] private int pelletCount = 8;
    [SerializeField] private float spreadAngle = 5f;

    protected override void Fire()
    {
        for (int i = 0; i < pelletCount; i++)
        {
            Vector3 direction = aimSource.forward;

            // Nudge each pellet off the center line.
            direction += aimSource.right * Random.Range(-1f, 1f) * (spreadAngle / 100f);
            direction += aimSource.up * Random.Range(-1f, 1f) * (spreadAngle / 100f);

            FireRay(direction.normalized);
        }
    }
}
