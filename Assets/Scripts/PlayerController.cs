using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject prefabSingleNote; // 生成するPrefab
    [SerializeField] private GameObject prefabLongNote;

    public static float ScrollSpeed = 1.0f; // 譜面のスクロール速度
    public static float CurrentSec = 0f; // 現在の経過時間(秒)
    public static float CurrentBeat = 0f; // 現在の経過時間(beat)
    // まだ判定処理で消えていないノーツ一覧
    public static List<NoteControllerBase> ExistingNoteControllers;

    public static Beatmap beatmap; // 譜面データを管理する
    private float startOffset = 1.0f; // 譜面のオフセット(秒)

    void Awake()
    {
        // 値を初期化
        CurrentSec = 0f;
        CurrentBeat = 0f;

        // 読み込む譜面があるディレクトリのパス
        var beatmapDirectory = Application.dataPath + "/../Beatmaps";
        // Beatmapクラスのインスタンスを作成
        beatmap = new Beatmap(beatmapDirectory + "/sample1.bms");

        // 直打ちしていたノーツは配置情報を削除した

        // ノーツの生成を行う


        // 未処理ノーツ一覧を初期化
        ExistingNoteControllers = new List<NoteControllerBase>();

        // TODO: ここで譜面の読み込みを行う
        // 現段階では直打ち

        // Beatmapクラスのインスタンスを作成
        beatmap = new Beatmap();

        // テンポ変化を設定
        beatmap.tempoChanges = new List<TempoChange>
        {
            new TempoChange(0, 60f),
            new TempoChange(2, 120f),
            new TempoChange(4, 60f),
            new TempoChange(6, 120f)
        };// ノーツの生成を行う
        foreach (var noteProperty in beatmap.noteProperties)
       {
            // beatmapのnotePropertiesの各要素の情報からGameObjectを生成
            GameObject objNote = null;
            switch (noteProperty.noteType)
            {
            case NoteType.Single:
                objNote = Instantiate(prefabSingleNote);
                break;
                case NoteType.Long:
                objNote = Instantiate(prefabLongNote);
                break;
            }
            // ノーツ生成時に未処理ノーツ一覧に追加
            ExistingNoteControllers.Add(objNote.GetComponent<NoteControllerBase>());
            objNote.GetComponent<NoteControllerBase>().noteProperty = noteProperty;
         }
    }

    void Update()
    {
        // 秒数を更新
        CurrentSec = Time.time - startOffset;
        // 拍を更新(ToBeatを使用)
        CurrentBeat = Beatmap.ToBeat(CurrentSec, beatmap.tempoChanges);
    }
}