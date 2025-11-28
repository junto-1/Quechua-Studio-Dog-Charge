using System;
using UnityEngine;

public class ScoreSystem : MonoBehaviour {
  public int Score = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update() {
      Score += 1;
      print(Score);
    }
}
