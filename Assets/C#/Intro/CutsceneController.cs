using System;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
   public static CutsceneController Instance;
   private void Awake()
   {
      if (Instance == null) Instance = this;
      DontDestroyOnLoad(gameObject);
   }
   
   [SerializeField] private bool _playCutscene = true;
   [SerializeField] private bool _showInstruction = true;
   
   public void DisableCutscene() { _playCutscene = false; }
   public bool GetCutscene() { return _playCutscene; }
   
   public bool GetInstructions() { return _showInstruction; }

   private void OnApplicationQuit()
   {
      _playCutscene = true;
      _showInstruction = true;
   }
}
