# Weapon Pickups

`Pickup Base.prefab` is the base prefab that `Pistol Pickup` and `Shotgun Pickup`
are variants of. Make a new pickup by creating another variant of it.

## Setting up a new pickup

When you create a pickup variant, two things need attention or it won't work:

1. **Make sure the Box Collider encompasses the mesh.**
   The collider is what the player actually walks into. If it doesn't cover the
   visible mesh, the pickup will look like it's there but won't trigger.

2. **Set the Weapon Prefab field on the `WeaponPickup` component.**
   This is the weapon the player receives on pickup. Leave it empty and the
   pickup does nothing when collected.

## How to create one

1. Right-click `Pickup Base.prefab` → **Create → Prefab Variant**
2. Name it after the weapon, e.g. `Rifle Pickup`
3. Swap in the mesh you want
4. Resize the Box Collider to fit that mesh (point 1 above)
5. Drag the weapon prefab into **Weapon Prefab** on the `WeaponPickup` component (point 2 above)

See `Assets/Scripts/Weapons/WeaponPickup.cs` for what happens on collection.
