using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TextReader : MonoBehaviour
{
   [Header("Text Settings")]
   [SerializeField] private float _textSpeed;
   [SerializeField] [TextArea] private string[] sentences;
   private bool _textFinished;
   private int _index;
   
   [Header("References")]
   [SerializeField] private GameObject _nextButton;
   
   private TextMeshProUGUI _text;

   private void Start()
   {
      _text = GetComponent<TextMeshProUGUI>();
   }

   public void BeginReading()
   {
      _index = 0;
      _nextButton.SetActive(false);
      ReadCharacter(sentences[_index]);
   }

   public void ReadCharacter(string sentence)
   {
      _text.firstVisibleCharacter = 0;
      _text.text = sentence;
      StartCoroutine(ReadSentence(sentence.Length));
   }

   IEnumerator ReadSentence(int length)
   {
      yield return new WaitForSeconds(1f);
      
      while (_text.firstVisibleCharacter < length)
      {
         _text.firstVisibleCharacter++;
         yield return new WaitForSeconds(_textSpeed);
      }

      _nextButton.SetActive(true);
   }

   public void NextSentence()
   {
      Game.Instance.audioManager.PlayClick();
      _index++;
      if (_index >= sentences.Length)
      {
         //start game
         Game.Instance.StartGame();
         CutsceneController.Instance.DisableCutscene();
      }
      else ReadCharacter(sentences[_index]);
      _nextButton.SetActive(false);
   }
}
