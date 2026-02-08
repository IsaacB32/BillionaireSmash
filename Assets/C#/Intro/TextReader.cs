using System;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextReader : MonoBehaviour
{
   [Header("Text Settings")]
   [SerializeField] private float _textSpeed;
   [SerializeField] [TextArea] private string[] sentences;
   private bool _textFinished;
   private int _index;
   
   [Header("References")]
   [SerializeField] private Image _nextButton;
   
   private TextMeshProUGUI _text;

   private void Start()
   {
      _text = GetComponent<TextMeshProUGUI>();
   }

   public void BeginReading()
   {
      _index = 0;
      DisableButton();
      ReadCharacter(sentences[_index]);
   }

   public void ReadCharacter(string sentence)
   {
      _text.maxVisibleCharacters = 0;
      _text.text = sentence;
      int length = Regex.Replace(sentence, @"</?color[^>]*>", "").Length;
      StartCoroutine(ReadSentence(length));
   }

   IEnumerator ReadSentence(int length)
   {
      yield return new WaitForSeconds(.56f);
      
      while (_text.maxVisibleCharacters <= length)
      {
         _text.maxVisibleCharacters++;
         yield return new WaitForSeconds(_textSpeed);
      }

      yield return new WaitForSeconds(.3f);
      EnableButton();
   }

   public void NextSentence()
   {
      Game.Instance.audioManager.PlayClick();
      _index++;
      if (_index >= sentences.Length)
      {
         //start game
         CutsceneController.Instance.DisableCutscene();
         Game.Instance.StartGame();
      }
      else ReadCharacter(sentences[_index]);
      DisableButton();
   }


   private void DisableButton()
   {
      _nextButton.enabled = false;
      _nextButton.raycastTarget = false;
   }

   private void EnableButton()
   {
      _nextButton.enabled = true;
      _nextButton.raycastTarget = true;
   }
}
