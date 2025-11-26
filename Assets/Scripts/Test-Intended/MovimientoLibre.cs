using UnityEngine;

public class MovimientoLibre : MonoBehaviour
{

  //Este script es solo para facilitar los testeos
  public float Speed = 10f;
  public float JumpForce = 10f;
    // Update is called once per frame
    void Update() {
      bool isInputRight = (Input.GetKey(KeyCode.D));
      if (isInputRight) {
        transform.position += new Vector3(Speed, 0, 0) * Time.deltaTime;
      }

      bool isInputLeft = (Input.GetKey(KeyCode.A));
      if (isInputLeft) {
        transform.position += new Vector3(-Speed, 0, 0) * Time.deltaTime;
      }

      bool isInputUp = (Input.GetKey(KeyCode.W));
      if (isInputUp) {
        transform.position += new Vector3(0, Speed, 0) * Time.deltaTime;
      }
      bool isInputDown = (Input.GetKey(KeyCode.S));
      if (isInputDown) {
        transform.position += new Vector3(0, -Speed, 0) * Time.deltaTime;
      }
    }
}
