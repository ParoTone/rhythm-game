using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    string FilePath;

    [SerializeField]
    Button Play;
    [SerializeField]
    Button SetChart;

    [SerializeField]
    GameObject Don;
    [SerializeField]
    GameObject Ka;

    [SerializeField]
    Transform SpawnPoint;
    [SerializeField]
    Transform BeatPoint;

    string Title;
    int BPM;
    List<GameObject> Notes;

    void OnEnable()
    {
        Play.onClick
          .AsObservable()
          .Subscribe(_ => play());

        SetChart.onClick
          .AsObservable()
          .Subscribe(_ => loadChart());
    }

    void loadChart()
    {
        Notes = new List<GameObject>();

        string jsonText = Resources.Load<TextAsset>(FilePath).ToString();

        JsonNode json = JsonNode.Parse(jsonText);
        Title = json["title"].Get<string>();
        BPM = int.Parse(json["bpm"].Get<string>());

        foreach (var note in json["notes"])
        {
            string type = note["type"].Get<string>();
            float timing = float.Parse(note["timing"].Get<string>());

            GameObject Note;
            if (type == "don")
            {
                Note = Instantiate(Don, SpawnPoint.position, Quaternion.identity);
            }
            else if (type == "ka")
            {
                Note = Instantiate(Ka, SpawnPoint.position, Quaternion.identity);
            }
            else
            {
                Note = Instantiate(Don, SpawnPoint.position, Quaternion.identity); // default don
            }

            Notes.Add(Note);
        }
    }

    void play()
    {
        Debug.Log("Game Start!");
    }
}