using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] private float lifetime = 1f, floatSpeed = 1f;

    private TextMeshProUGUI textMesh;
    private Color originalColor;

    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        originalColor = textMesh.color;
    }

    public void ShowDamage(float amount, bool isCritical)
    {
        textMesh.text = amount.ToString("F1");
        textMesh.color = isCritical ? Color.red : originalColor;
        Destroy(gameObject, lifetime);
    }
    
    public void ShowUpgrade(string upgrade)
    {
        textMesh.text = upgrade;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Sobe suavemente
        transform.position += Vector3.up * (floatSpeed * Time.deltaTime);

        // Fade-out progressivo
        Color c = textMesh.color;
        c.a -= Time.deltaTime / lifetime;
        textMesh.color = c;
    }
}