using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionController : MonoBehaviour
{
    [SerializeField] private float _swipeTime = 0.4f;
    
    [Header("Values")]
    [SerializeField] private float _rightStart;
    [SerializeField] private float _centerEnd;
    [SerializeField] private float _leftEnd;
    
    private void Awake()
    {
        StartCoroutine(SwipeOut());
    }
    
    private IEnumerator SwipeOut()
    {
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(Swipe(_centerEnd, _leftEnd, _swipeTime, null));
    }

    public void ResetGame()
    {
        StartCoroutine(Swipe(_rightStart, _centerEnd, _swipeTime, FinishIn));
    }
    private void FinishIn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator Swipe(float start, float end, float time, Action finished)
    {
        float elapsed = 0;
        RectTransform rect = GetComponent<RectTransform>();
        while (elapsed <= _swipeTime)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / time);

            float x = Mathf.Lerp(start, end, t);
            rect.anchoredPosition = new Vector2(x, rect.anchoredPosition.y);
            
            yield return null;
        }
        rect.anchoredPosition = new Vector2(end, rect.anchoredPosition.y);
        finished?.Invoke();
    }
}
