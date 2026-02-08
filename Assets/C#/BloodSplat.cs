using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BloodSplat : MonoBehaviour
{
    [SerializeField] private float maxTtl = 3f;
    [SerializeField] private float fadeDuration = 1f;

    private float ttl;
    private bool fading;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Coroutine fadeRoutine;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void Init()
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fading = false;
        ttl = maxTtl;

        Color c = spriteRenderer.color;
        c.a = 1f;
        spriteRenderer.color = c;

        animator.Play("[splat]", 0, 0f);
        animator.speed = Random.Range(0.8f, 1.2f);
    }

    void Update()
    {
        if (fading)
            return;

        ttl -= Time.deltaTime;

        if (ttl <= 0f)
        {
            fadeRoutine = StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeOut()
    {
        fading = true;

        float elapsed = 0f;
        Color start = spriteRenderer.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;

            Color c = start;
            c.a = Mathf.Lerp(1f, 0f, t);
            spriteRenderer.color = c;

            yield return null;
        }

        Color final = spriteRenderer.color;
        final.a = 0f;
        spriteRenderer.color = final;

        Game.Instance.enemyManager.ReleaseBloodSplat(this);
    }
}