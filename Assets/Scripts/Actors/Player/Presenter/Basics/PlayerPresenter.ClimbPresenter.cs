﻿using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerPresenter : MonoBehaviour
{
    void ClimbPresenter()
    {
        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Ladder")
            .Subscribe(_ => PlayerState.IsClimbable.Value = true);

        this.OnTriggerExit2DAsObservable()
            .Where(x => x.gameObject.tag == "Ladder")
            .Subscribe(_ => PlayerState.IsClimbable.Value = false);

        this.OnTriggerExit2DAsObservable()
            .Where(x => x.gameObject.tag == "Ladder")
            .Subscribe(_ => PlayerState.IsClimbing.Value = false);

        this.OnTriggerEnter2DAsObservable()
            .Where(x => PlayerState.IsClimbing.Value)
            .Where(x => x.gameObject.tag == "Hard Platform")
            .Subscribe(_ =>
            {
                transform.position = new Vector2(transform.position.x, _.gameObject.transform.position.y + _.gameObject.GetComponent<SpriteRenderer>().bounds.extents.y + SpriteRenderer.bounds.extents.y);
            });

        this.OnTriggerStay2DAsObservable()
            .Where(x => x.gameObject.tag == "CanClimbDownLadder")
            .Where(x => Key.Vertical.Value == -1)
            .Subscribe(_ =>
            {
                transform.position = new Vector2(transform.position.x, transform.position.y - 0.63f);
                PlayerState.IsClimbing.Value = true;
            });

        this.UpdateAsObservable()
            .Select(x => PlayerState.IsClimbable.Value && Key.Vertical.Value != 0)
            .Where(x => x)
            .Subscribe(_ => PlayerState.IsClimbing.Value = true);

        this.UpdateAsObservable()
            .Select(x => PlayerState.IsClimbing.Value && Key.Horizontal.Value != 0)
            .Where(x => x)
            .Subscribe(_ => PlayerState.IsClimbing.Value = false);

        this.FixedUpdateAsObservable()
            .Where(x => PlayerState.IsClimbing.Value)
            .Subscribe(_ => Rigidbody2D.gravityScale = 0f);

        this.FixedUpdateAsObservable()
            .Where(x => !PlayerState.IsClimbing.Value)
            .Subscribe(_ => Rigidbody2D.gravityScale = PlayerConfig.GravityScaleStore);

        this.FixedUpdateAsObservable()
            .Where(x => PlayerState.IsClimbing.Value)
            .Where(x => Key.Vertical.Value == 0)
            .Subscribe(_ => Rigidbody2D.velocity = Vector2.zero); 

        this.FixedUpdateAsObservable()
            .Where(x => PlayerState.IsClimbing.Value)
            .Where(x => Key.Vertical.Value != 0)
            .Subscribe(_ => PlayerMotion.Climb(Key.Vertical.Value, PlayerConfig.MaxSpeed));
    }
}