using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]private Text textValue;
    private string valueFormat;
    private void Start()
    {
        valueFormat = textValue.text;
    }

    // Update is called once per frame
    private void Update()
    {
        textValue.text = string.Format(valueFormat,
        // {0}: スコア
        Mathf.CeilToInt(EvaluationManager.Score),
        // {1}: コンボ数
        EvaluationManager.Combo,
        // {2}: 最大コンボ数
        EvaluationManager.MaxCombo,
        // {3}: PERFECT
        EvaluationManager.JudgmentCounts[JudgementType.Perfect],
        // {4}: GOOD
        EvaluationManager.JudgmentCounts[JudgementType.Good],
        // {5}: BAD
        EvaluationManager.JudgmentCounts[JudgementType.Bad],
        // {6}: MISS
        EvaluationManager.JudgmentCounts[JudgementType.Miss]
        );
    }
}
