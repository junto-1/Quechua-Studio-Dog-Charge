using UnityEngine;


// Generador de terreno procedural infinito.
// Genera chunks adelante del jugador y destruye los que quedan atrás.
public class TerrainGenerator : MonoBehaviour {
  public GameObject[] chunkPrefabs;
  public float minAngle = -30f;
  public float maxAngle = 30f;
  public Transform playerTransform;

  public float chunkOffset = 100f;

  public GameObject lastChunk;
  private Transform _nextChunkPosition;


  public void Start() {
    _nextChunkPosition = transform;
  }

  public void Update() {
    if (playerTransform.position.x + chunkOffset > _nextChunkPosition.position.x) {
      CreateChunk();
    }
  }

  private void CreateChunk() {
    lastChunk = Instantiate(chunkPrefabs[Random.Range(0, chunkPrefabs.Length)], _nextChunkPosition.position,
      Quaternion.identity);


    if (Random.value > 0.5) {
      lastChunk.transform.rotation = Quaternion.Euler(0, 0, Random.Range(minAngle, maxAngle));
    }


    _nextChunkPosition = lastChunk.GetComponent<Chunk>().Endpoint;
  }
}
