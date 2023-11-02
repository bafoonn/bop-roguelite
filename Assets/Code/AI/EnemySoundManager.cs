using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Tilemaps;

namespace Pasta
{
    public class EnemySoundManager : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;

        [SerializeField] private AudioClip[] movementSounds;
        [SerializeField] private AudioClip[] attackSounds;
        [SerializeField] private AudioClip[] dyingSounds;


        private GameObject parent;
        private Vector3 originPoint;
        private Vector2Int originPoint2int;
        private Vector3Int originPoint3int;
        private Tilemap tileMap;
        // Start is called before the first frame update
        private void Start()
        {
            parent = this.gameObject;
            audioSource = GetComponent<AudioSource>();
        }


        public void MovementSounds()
        {
            int random = Random.Range(0, movementSounds.Length);
            audioSource.clip = movementSounds[random];
            audioSource.Play();

        }


        public void attackingSound()
        {
            int random = Random.Range(0, attackSounds.Length);
            audioSource.clip = attackSounds[random];
            audioSource.Play();
        }


        public void dyingSound()
        {
            int random = Random.Range(0, dyingSounds.Length);
            audioSource.clip = dyingSounds[random];
            audioSource.Play();
        }

    }
}
