using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public float maxAngularVelocity = 180f;

    private SpriteRenderer sr;
    private Color originalColor;
    private Vector3 originalScale;
    private Rigidbody2D rb;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        if (sr != null)
            originalColor = sr.color;
        originalScale = transform.localScale;
    }

    void FixedUpdate()
    {
        if (rb != null && rb.angularVelocity > maxAngularVelocity)
            rb.angularVelocity = maxAngularVelocity;
        if (rb != null && rb.angularVelocity < -maxAngularVelocity)
            rb.angularVelocity = -maxAngularVelocity;
    }

    public void OnGrab()
    {
        if (sr != null)
            sr.color = Color.yellow;
        transform.localScale = originalScale * 1.3f;
    }

    public void OnRelease()
    {
        if (sr != null)
            sr.color = originalColor;
        transform.localScale = originalScale;
    }
}
