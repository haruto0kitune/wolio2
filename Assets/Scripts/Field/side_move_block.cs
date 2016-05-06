using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;

public class side_move_block : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.01f;
    [SerializeField]
    private float volume = 100;
    private Transform Parent;

    void Start()
    {
        StartCoroutine(Move());

        // Move with block
        this.OnTriggerEnter2DAsObservable()
            .ThrottleFirstFrame(1)
            .Where(x => x.gameObject.tag == "Player" || x.gameObject.tag == "Enemy")
            .Subscribe(_ =>
            {
                Parent = _.gameObject.transform.parent;
                _.gameObject.transform.parent = transform;
            });

        this.OnTriggerExit2DAsObservable()
            .ThrottleFirstFrame(1)
            .Where(x => x.gameObject.tag == "Player" || x.gameObject.tag == "Enemy")
            .Subscribe(_ =>
            {
                _.gameObject.transform.parent = Parent;
            });
    }

    IEnumerator Move()
    {
        while (true)
        {
            // move left
            for (int i = 0; i < volume; i++)
            {
                transform.Translate(Vector3.left * speed);
                yield return null;
            }

            // move right
            for (int i = 0; i < volume; i++)
            {
                transform.Translate(Vector3.right * speed);
                yield return null;
            }
        }
    }
}
