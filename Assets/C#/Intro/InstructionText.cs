using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class InstructionText : MonoBehaviour
{
    private CanvasGroup _canvas;
    private TextMeshProUGUI _text;
    private int _index;
    
    [SerializeField] private float _showTime = 0.6f;

    private void Start()
    {
        _canvas = GetComponent<CanvasGroup>();
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void Show(Action ending)
    {
        _canvas.alpha = 1;
        _text.maxVisibleLines = 1;
        StartCoroutine(ShowText(ending));
    }

    private IEnumerator ShowText(Action ending)
    {
        yield return new WaitForSeconds(0.45f);
        for (int i = 0; i <= 3; i++)
        {
            yield return new WaitForSeconds(_showTime);
            _text.maxVisibleLines++;
            if (i < 3) Game.Instance.audioManager.PlayInstructionTextAppear();
        }

        yield return new WaitForSeconds(0.8f);
        Game.Instance.audioManager.PlayInstructionTextAppear(2f);
        _canvas.alpha = 0;
        ending.Invoke();
    }
}
