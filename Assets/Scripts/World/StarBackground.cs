// using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class StarBackground : MonoBehaviour
{
    public static readonly int NUM_STARS = 50;
    public static readonly float STAR_DEFAULT_SPEED = 0.5f;

    [SerializeField] private GameObject starPrefab;

    private GameObject[] stars = new GameObject[NUM_STARS];
    private float[] speeds = new float[NUM_STARS];

    private float respawnStar(GameObject star, float? newX = null)
    {
        var pos = new Vector2(Random.Range(0, 40f), Random.Range(0, 30f));
        var speed = Random.Range(STAR_DEFAULT_SPEED, STAR_DEFAULT_SPEED * 2);

        if (newX.HasValue) pos.x = newX.Value;

        star.transform.parent = transform;
        star.transform.localPosition = new Vector3(pos.x, pos.y, 0);
        star.transform.localScale = new Vector3(0.25f, 0.25f, 1);

        var c = (byte)(speed < 1.5f * STAR_DEFAULT_SPEED ? 0x22 : 0x55);
        star.GetComponent<SpriteRenderer>().color = new Color32(c, c, c, 0xff);

        return speed;
    }

    void Start()
    {
        foreach (var i in Enumerable.Range(0, NUM_STARS))
        {
            stars[i] = Instantiate(starPrefab);
            speeds[i] = respawnStar(stars[i]);
        }
    }

    void Update()
    {
        foreach (var i in Enumerable.Range(0, NUM_STARS))
        {
            var t = stars[i].transform;
            var x = t.localPosition.x - speeds[i];
            var alpha = 0.65f; // TODO: alpha can chage in accumulator
            x = lerp(x + speeds[i], x, alpha);
            t.localPosition = new Vector3(x, t.localPosition.y, t.localPosition.z);

            if (x < -1.25) speeds[i] = respawnStar(stars[i], x + 42.5f);
        }
    }

    public static float lerp(float a, float b, float alpha) => a + alpha * (b - a);
}