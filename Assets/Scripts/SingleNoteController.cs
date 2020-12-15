using UnityEngine;

public class SingleNoteController : NoteControllerBase
{
    [SerializeField] AudioClip ClipHit; //効果音
    void Update()
    {
        SetTransform();
        CheckMiss();
    }

    //　ノーツの座標設定
    private void SetTransform()
    {
        //ノーツの座標
        Vector2 position = new Vector2();
        position.x = noteProperty.lane - 2;
        position.y = (noteProperty.beatBegin - PlayerController.CurrentBeat) *
            PlayerController.ScrollSpeed;
        transform.localPosition = position;
    }

    //見逃し検出
    private void CheckMiss()
    {
        //判定ラインを通過した後、BADの判定幅よりも離れるとノーツを削除
        if (noteProperty.secBegin - PlayerController.CurrentSec <
            -JudgementManager.JudgementWidth[JudgementType.Bad])
        {
            //未処理ノーツ一覧から削除
            PlayerController.ExistingNoteControllers.Remove(
                GetComponent<NoteControllerBase>()
            );
            //GameObject自体も削除
            Destroy(gameObject);
        }
    }

    //キーが押された時
    public override void OnKeyDown(JudgementType judgementType)
    {
        //デバッグ用にコンソールに判定を出力
        Debug.Log(judgementType);

        //判定がMissでないとき（BAD以内の時）
        if (judgementType != JudgementType.Miss)
        {
            //効果音再生
            AudioSource.PlayClipAtPoint(ClipHit, transform.position);
            //　未処理ノーツ一覧から削除
            PlayerController.ExistingNoteControllers.Remove(
                GetComponent<NoteControllerBase>()
            );
            // GameObject自体も削除
            Destroy(gameObject);
        }
    }
}