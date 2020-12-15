using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
public class PlayerController : MonoBehaviour
{
   [SerializeField] private GameObject prefabSingleNote; // 生成するPrefab
   [SerializeField] private GameObject prefabLongNote; // 生成するPrefab
   [SerializeField] AudioSource audioSource; // 音源再生用AudioSource

   public static float ScrollSpeed = 1.0f; // 譜面のスクロール速度
   public static float CurrentSec = 0f; // 現在の経過時間(秒)
   public static float CurrentBeat = 0f; // 現在の経過時間(beat)
   public static List<NoteControllerBase> ExistingNoteControllers;
            
   public static Beatmap beatmap; //譜面データを管理する
   private float startOffset = 1.0f; //譜面のオフセット（秒）
   private float startSec = 0f; //　譜面再生開始秒数（再生停止用）
   private bool isPlaying = false;  //　譜面停止中か否か

   void Awake()
   {
        //値を初期化
        CurrentSec = 0f;
        CurrentBeat = 0f;

        // 未処理ノーツ一覧を初期化
        ExistingNoteControllers = new List<NoteControllerBase>();

        // ノーツ配置情報を設定   
       
        // 読み込む譜面があるディレクトリのパス
        var beatmapDirectory = Application.dataPath + "/../Beatmaps";
        // Beatmapクラスのインスタンスを作成
        beatmap = new Beatmap(beatmapDirectory + "/sample1.bms");

        //デバッグ用にテンポ変化をコンソールに出力
        foreach (var tempoChange in beatmap.tempoChanges)
        {
            Debug.Log(tempoChange.beat + ": " + tempoChange.tempo);
        }

    

       // TODO　ここでノーツの生成を行う
       foreach (var noteProperty in beatmap.noteProperties)
       {
           // beatmapのnotePropertiesの各要素の情報からGameObjectを生成
           GameObject ObjNote = null;
           switch (noteProperty.noteType)
           {
                case NoteType.Single:
                    ObjNote = Instantiate(prefabSingleNote);
                    break;
                case NoteType.Long:
                    ObjNote = Instantiate(prefabLongNote);
                    break;
           }
           ExistingNoteControllers.Add(ObjNote.GetComponent<NoteControllerBase>());
           ObjNote.GetComponent<NoteControllerBase>().noteProperty = noteProperty;
       }

       //  音源読み込み
       //StartCoroutine(LoadAudioFile(beatmap.audioFilePath));
   }

   void Update()
   {
       //  譜面停止中にスペースを押した時
       if (!isPlaying && Input.GetKeyDown(KeyCode.Space))
       {
           //譜面再生
           isPlaying = true;
           //　指定した定数待って音源再生
           audioSource.PlayScheduled(
               AudioSettings.dspTime + startOffset + beatmap.audioOffset
           );
       }
       //譜面停止中
       if (!isPlaying)
       {
           // startSecを更新し続ける
           startSec = Time.time;
       }
       //秒数を更新
       CurrentSec = Time.time - startOffset;
       //拍を更新(ToBeatを使用)
       CurrentBeat = Beatmap.ToBeat(CurrentSec, beatmap.tempoChanges);

   }

        /*//  指定されたパスに存在する音源を読み込み
        private IEnumerator LoadAudioFile(string filepath)
        {
            //　ファイルが存在しなければ処理を行わない
            if (!File.Exists(filepath)) {yield break; }
            //音源のフォーマット種別
            var audioType = GetAudioType(filepath);
            //  unityWebRequestを用いて外部リソースを読み込む
            using(var reques = UnityWebRequestMultimedia.GetAudioClip(
                "file://" + filePath, audioType
            ))
            {
                yield return AssetBundleCreateRequest.SendWebRequest();
                //  エラーが発生しなかった場合
                if (!request.isNetworkError)
                {
                    //  オーディオクリップを読み込み
                    var audioClip = DownloadHandlerAudioClip.GetContent(request);
                    //  audioSourceのclipに設定
                    audioSource.clip = audioClip;
                }
            }
        }

        //　ファイル名から音源のフォーマットを取得する
        private AudioType GetAudioType(string filePath)
        {
            //　拡張子を取得
            string ext = Path.GetExtension(filePath).ToLower();
            switch (ext)
            {
                case ".ogg":
                return AudioType.OGGVORBIS;
            case "mp3":
                return AudioType.MPEG;
            case ".wav":
                return AudioType.WAV;
                default:
                return AudioType.UNKNOWN;
            }
        } */
}
