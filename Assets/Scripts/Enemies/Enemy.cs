using System.Collections;
using UnityEngine;

/// <summary>
/// Deliberately plain. One enemy, one health value.
/// This is the obvious thing for club members to extend
/// after the session ends.
/// </summary>
public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxHealth = 50f;

    private float currentHealth;

    private Color cachedColor;
    private Renderer _renderer;

    private void Awake()
    {
        currentHealth = maxHealth;

        _renderer = GetComponent<Renderer>();
        cachedColor = _renderer.material.color;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log(name + " took " + amount + " damage, " + currentHealth + " left");

        StartCoroutine(UpdateColor());

        if (currentHealth <= 0f)
            Die();
    }

    private IEnumerator UpdateColor()
    {
        _renderer.material.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        _renderer.material.color = cachedColor;
    }

    private void Die()
    {
        _renderer.material.color = Color.red;

        Destroy(gameObject, 1);
    }
}
