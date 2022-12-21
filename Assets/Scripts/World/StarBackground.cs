// using System;
using System.Linq;
using UnityEngine;

namespace VVVVVV.World
{
    public class StarBackground : MonoBehaviour
    {
        [SerializeField] private GameObject starPrefab;

        private static readonly int NUM_STARS = 50;
        private static readonly float STAR_EXPIRE_TRIGGER_X = -Constant.ROOM_LOCAL_SIZE.x * 0.0625f / 2;
        private static readonly float STAR_RESPAWN_X = Constant.ROOM_LOCAL_SIZE.x * 1.0625f;
        private static readonly float STAR_DEFAULT_SPEED = 0.5f;
        private static readonly float FAST_STAR_THRESHOLD = STAR_DEFAULT_SPEED * 1.5f;
        private static readonly float SPEED_ADJUST_PARAMETER = 0.65f;

        private GameObject[] stars = new GameObject[NUM_STARS];
        private float[] speeds = new float[NUM_STARS];

        private void Start()
        {
            foreach (var i in Enumerable.Range(0, NUM_STARS))
            {
                var pos = new Vector2(Random.Range(0, Constant.ROOM_LOCAL_SIZE.x), Random.Range(0, Constant.ROOM_LOCAL_SIZE.y));

                stars[i] = Instantiate(starPrefab, transform);
                stars[i].transform.localPosition = pos;
                speeds[i] = resetSpeed(stars[i]);
            }
        }

        private float resetSpeed(GameObject star)
        {
            var speed = Random.Range(STAR_DEFAULT_SPEED, STAR_DEFAULT_SPEED * 2);

            var c = (byte)(speed < FAST_STAR_THRESHOLD ? 0x22 : 0x55);
            star.GetComponent<SpriteRenderer>().color = new Color32(c, c, c, 0xff);

            return speed;
        }

        private void Update()
        {
            foreach (var i in Enumerable.Range(0, NUM_STARS))
            {
                var p = stars[i].transform.localPosition;

                var newX = p.x - (speeds[i] * SPEED_ADJUST_PARAMETER);
                if (newX < STAR_EXPIRE_TRIGGER_X)
                {
                    newX += STAR_RESPAWN_X;
                    speeds[i] = resetSpeed(stars[i]);
                }

                stars[i].transform.localPosition = new Vector3(newX, p.y);
            }
        }
    }
}
