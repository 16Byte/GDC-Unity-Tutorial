using UnityEngine;

/// <summary>
/// Shared logic for every weapon in the game.
/// Subclasses only decide HOW a shot leaves the barrel.
/// Cooldown, ammo, aiming and damage application all live here.
/// </summary>
public abstract class Weapon : MonoBehaviour
{
    [Header("Common stats (tune these in the Inspector)")]
    [SerializeField] protected string weaponName = "Weapon";
    [SerializeField] protected float damage = 10f;
    [SerializeField] protected float range = 100f;
    [SerializeField] protected float shotsPerSecond = 4f;
    [SerializeField] protected int maxAmmo = 30;
    [SerializeField] protected bool hasUnlimitedAmmo = false;
    
    public FireType fireType = FireType.SemiAuto;
    
    public enum FireType
    {
        SemiAuto,
        Auto,
    }

    [Header("Shot tracer")]
    [SerializeField] private float tracerSeconds = 0.05f;
    [SerializeField] private float tracerWidth = 0.02f;
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private Color missColor = Color.white;

    // One material shared by every tracer in the game. static means it is
    // created once, not once per shot.
    private static Material tracerMaterial;

    protected int currentAmmo;
    protected Transform aimSource;

    private float nextAllowedShotTime;

    protected virtual void Awake()
    {
        currentAmmo = maxAmmo;

        // The camera is what the player is actually aiming with.
        if (Camera.main != null)
            aimSource = Camera.main.transform;
    }

    /// <summary>
    /// Called by the player every frame the fire button is held.
    /// Handles the rules that are the same for all weapons,
    /// then hands off to the subclass.
    /// </summary>
    public void TryFire()
    {
        if (Time.time < nextAllowedShotTime) return;
        if (currentAmmo <= 0) return;

        nextAllowedShotTime = Time.time + (1f / shotsPerSecond);

        if (hasUnlimitedAmmo == false)
            currentAmmo--;

        Fire();
    }

    /// <summary>
    /// The one thing every weapon does differently.
    /// </summary>
    protected abstract void Fire();

    /// <summary>
    /// Helper the subclasses use so they never repeat raycast code.
    /// </summary>
    protected void FireRay(Vector3 direction)
    {
        if (aimSource == null) return;

        // Where the shot LOOKS like it goes. The gun, not the camera.
        Vector3 tracerStart = transform.position;

        // Assume a miss: the shot travels the full range and hits nothing.
        Vector3 tracerEnd = aimSource.position + direction * range;
        Color tracerColor = missColor;

        if (Physics.Raycast(aimSource.position, direction, out RaycastHit hit, range))
        {
            // We hit something, so stop the line there instead.
            tracerEnd = hit.point;
            tracerColor = hitColor;

            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
                enemy.TakeDamage(damage);
        }

        DrawTracer(tracerStart, tracerEnd, tracerColor);
    }

    /// <summary>
    /// Makes a throwaway line for one shot and lets Unity delete it a moment later.
    /// A new object per pellet means the shotgun's 8 pellets each get their own line.
    /// </summary>
    private void DrawTracer(Vector3 start, Vector3 end, Color color)
    {
        if (tracerMaterial == null)
            tracerMaterial = new Material(Shader.Find("Sprites/Default"));

        GameObject tracerObject = new GameObject("Tracer");

        LineRenderer line = tracerObject.AddComponent<LineRenderer>();
        line.material = tracerMaterial;
        line.startColor = color;
        line.endColor = color;
        line.startWidth = tracerWidth;
        line.endWidth = tracerWidth;

        line.positionCount = 2;
        line.SetPosition(0, start);
        line.SetPosition(1, end);

        Destroy(tracerObject, tracerSeconds);
    }

    public string DisplayName => weaponName;
    public int Ammo => currentAmmo;
}
