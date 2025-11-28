using UnityEngine;
using TMPro;
public class NewMonoBehaviourScript : MonoBehaviour
{
  public TextMeshProUGUI scoreText;
  public ScoreSystem scoreSystem;

    void Update()
    {
      scoreText.text = "Score: " + scoreSystem.Puntuación;
    }
}
