using UnityEngine;

public abstract class NoteControllerBase : MonoBehaviour
{
    public NoteProperty noteProperty;
    public bool isProcessed = false; // ロングノーツ用処理中フラグ
    public virtual void OnKeyDown(JudgementType judgementType) { }

    public virtual void OnKeyUp(JudgementType judgementType) { }

}
