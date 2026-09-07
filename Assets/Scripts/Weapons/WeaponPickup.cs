using UnityEngine;

/// <summary>
/// Sits in the world on a trigger collider.
/// The field is typed Weapon, so the SAME pickup script
/// works for the pistol, the shotgun, and anything authored later.
/// </summary>
public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private Weapon weaponPrefab;

    public Weapon WeaponPrefab => weaponPrefab;

    private void Update()
    {
        transform.Rotate(Vector3.up, 45f * Time.deltaTime);
    }
}
