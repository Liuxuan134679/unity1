using UnityEngine;

public class PanController : MonoBehaviour
{
    [Header("锅设置")]
    public float grabHeight = 1.5f;
    public float grabLerpSpeed = 18f;
    public float maxThrowSpeed = 30f;
    public int velocitySampleCount = 5;

    [Header("碰撞推力")]
    public float pushScale = 0.3f;
    public float maxPushSpeed = 6f;

    private Rigidbody2D panRb;
    private Camera cam;
    private Rigidbody2D grabbed;
    private Ingredient grabbedIngredient;

    private Vector2 panVelocity;
    private Vector2 lastMousePos;
    private Vector2[] velocitySamples;
    private int velIndex;
    private bool samplesInitialized;

    void Start()
    {
        panRb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        velocitySamples = new Vector2[velocitySampleCount];
    }

    void FixedUpdate()
    {
        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);

        panVelocity = (mouseWorld - panRb.position) / Time.fixedDeltaTime;
        panRb.MovePosition(mouseWorld);

        if (grabbed != null)
        {
            Vector2 target = mouseWorld + Vector2.up * grabHeight;
            Vector2 newPos = Vector2.Lerp(
                grabbed.position, target,
                grabLerpSpeed * Time.fixedDeltaTime);
            grabbed.MovePosition(newPos);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.attachedRigidbody == grabbed || other.attachedRigidbody == null)
            return;

        Vector2 dir = (other.attachedRigidbody.position - panRb.position).normalized;
        float speed = Mathf.Min(panVelocity.magnitude * pushScale, maxPushSpeed);
        other.attachedRigidbody.velocity = dir * speed;
    }

    void Update()
    {
        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);

        if (samplesInitialized)
        {
            Vector2 delta = mouseWorld - lastMousePos;
            velocitySamples[velIndex] = delta / Time.deltaTime;
            velIndex = (velIndex + 1) % velocitySampleCount;
        }
        lastMousePos = mouseWorld;
        samplesInitialized = true;

        if (Input.GetMouseButtonDown(0) && grabbed == null)
        {
            TryGrab(mouseWorld);
        }

        if (Input.GetMouseButtonUp(0) && grabbed != null)
        {
            Release();
        }
    }

    void TryGrab(Vector2 pos)
    {
        int layerMask = 1 << LayerMask.NameToLayer("Ingredient");
        Collider2D hit = Physics2D.OverlapPoint(pos, layerMask);
        if (hit != null)
        {
            grabbed = hit.GetComponent<Rigidbody2D>();
            if (grabbed != null)
            {
                grabbed.gravityScale = 0;
                grabbedIngredient = hit.GetComponent<Ingredient>();
                if (grabbedIngredient != null)
                    grabbedIngredient.OnGrab();
            }
        }
    }

    void Release()
    {
        Vector2 avg = Vector2.zero;
        foreach (var v in velocitySamples) avg += v;
        avg /= velocitySampleCount;
        avg = Vector2.ClampMagnitude(avg, maxThrowSpeed);

        grabbed.gravityScale = 1;
        grabbed.velocity = avg;

        if (grabbedIngredient != null)
            grabbedIngredient.OnRelease();

        grabbed = null;
        grabbedIngredient = null;
    }
}
