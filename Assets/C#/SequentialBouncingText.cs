using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class SequentialBouncingText : MonoBehaviour
{
    [Header("Bounce Settings")]
    public float amplitude = 10f;
    public float frequency = 6f;
    public float characterDelay = 0.08f;
    public bool useUnscaledTime = true;

    TMP_Text _text;
    TMP_TextInfo _textInfo;

    void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        _text.ForceMeshUpdate();
        _textInfo = _text.textInfo;

        float time = useUnscaledTime ? Time.unscaledTime : Time.time;

        for (int i = 0; i < _textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = _textInfo.characterInfo[i];
            if (!charInfo.isVisible)
                continue;

            int matIndex = charInfo.materialReferenceIndex;
            int vertIndex = charInfo.vertexIndex;

            Vector3[] vertices = _textInfo.meshInfo[matIndex].vertices;

            float charTime = time - i * characterDelay;
            float offset = Mathf.Sin(charTime * frequency) * amplitude;

            Vector3 bounce = Vector3.up * offset;

            vertices[vertIndex + 0] += bounce;
            vertices[vertIndex + 1] += bounce;
            vertices[vertIndex + 2] += bounce;
            vertices[vertIndex + 3] += bounce;
        }

        for (int i = 0; i < _textInfo.meshInfo.Length; i++)
        {
            _textInfo.meshInfo[i].mesh.vertices = _textInfo.meshInfo[i].vertices;
            _text.UpdateGeometry(_textInfo.meshInfo[i].mesh, i);
        }
    }
}