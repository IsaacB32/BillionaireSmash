using UnityEngine;

public class CutsceneController : MonoBehaviour
{
   public static CutsceneController Instance;
   private void Awake()
   {
      if (Instance == null) Instance = this;
      DontDestroyOnLoad(gameObject);
   }
   
   [SerializeField] private bool _playCutscene;
   
   public void DisableCutscene() { _playCutscene = false; }
   public bool GetCutscene() { return _playCutscene; }
}
