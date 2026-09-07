using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Lives on the player. This is the payoff script.
///
/// Notice the field type: Weapon, not Pistol and not Shotgun.
/// The player has no idea what it is holding, and does not need to.
/// </summary>
public class WeaponHolder : MonoBehaviour
{
    [SerializeField] private Transform weaponSocket;

    private Weapon equippedWeapon;

    // Filled in at runtime by picking things up. Leave this empty in the Inspector:
    // it holds spawned weapons, not prefabs.
    [SerializeField] private List<Weapon> inventory = new List<Weapon>();

    [SerializeField] private int inventoryIndex = 0;

    private void Update()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
            CycleWeapons();

        if (equippedWeapon == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
            equippedWeapon.TryFire();
    }

    private void OnTriggerEnter(Collider other)
    {
        WeaponPickup pickup = other.GetComponent<WeaponPickup>();
        if (pickup == null) return;

        // Spawn the weapon ONCE, here, and keep that same object for the whole game.
        // If we spawned it every time we swapped, ammo would reset on every swap.
        Weapon newWeapon = Instantiate(pickup.WeaponPrefab, weaponSocket);
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.localRotation = Quaternion.identity;

        inventory.Add(newWeapon);

        // We just added it to the end, so the new weapon lives at the last index.
        Equip(inventory.Count - 1);

        Destroy(other.gameObject);
    }

    /// <summary>
    /// The ONE place that changes inventoryIndex.
    /// Pickup and cycling both go through here, so the index can never get out of sync.
    /// </summary>
    private void Equip(int index)
    {
        if (inventory.Count == 0) return;

        inventoryIndex = index;

        // Hide every weapon, show the one we want. Simple beats clever.
        for (int i = 0; i < inventory.Count; i++)
            inventory[i].gameObject.SetActive(i == inventoryIndex);

        equippedWeapon = inventory[inventoryIndex];

        Debug.Log("Equipped: " + equippedWeapon.DisplayName);
    }

    private void CycleWeapons()
    {
        // Nothing to cycle between.
        if (inventory.Count <= 1) return;

        int nextIndex;

        if (inventoryIndex == inventory.Count - 1)
            nextIndex = 0;
        else
            nextIndex = inventoryIndex + 1;

        Equip(nextIndex);
    }
}
