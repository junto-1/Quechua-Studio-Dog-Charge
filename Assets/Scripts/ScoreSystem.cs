using System;
using UnityEngine;

public class ScoreSystem : MonoBehaviour {
  public int Puntuación = 0;

  //Aumenta el puntaje a lo largo del tiempo.
    void Update()
    {
      Puntuación += 1;
    }
}
