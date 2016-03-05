using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public partial class PlayerPresenter : MonoBehaviour
{
    void ClimbPresenter()
    {
        this.FixedUpdateAsObservable()
            .Zip(Key.Vertical, (a, b) => b)
            .Where(x => PlayerState.IsClimbable.Value)
            .Where(x => x == 1)
            .Subscribe(_ => 
            {
                Rigidbody2D.isKinematic = true;
                PlayerMotion.Climb(_, PlayerConfig.MaxSpeed);
            });

        this.FixedUpdateAsObservable()
            .Where(x => PlayerState.IsClimbable.Value)
            .Where(x => !PlayerState.IsGrounded.Value)
            .Zip(Key.Vertical, (a, b) => b)
            .Where(x => x == -1)
            .Subscribe(_ => 
            {
                //Rigidbody2D.isKinematic = true;
                PlayerMotion.Climb(_, PlayerConfig.MaxSpeed);
            });

        this.FixedUpdateAsObservable()
            .Zip(PlayerState.IsGrounded, (a, b) => b)
            .Where(x => x)
            .Subscribe(_ => Rigidbody2D.isKinematic = false);

        this.FixedUpdateAsObservable()
            .Zip(Key.Vertical, (a, b) => b)
            .Where(x => PlayerState.IsClimbable.Value)
            .Where(x => x == 0)
            .Subscribe(_ => Rigidbody2D.velocity = Vector2.zero);

        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Ladder")
            .Subscribe(_ =>
            {
                PlayerState.IsClimbable.Value = true;
            });

        this.OnTriggerExit2DAsObservable()
            .Where(x => x.gameObject.tag == "Ladder")
            .Subscribe(_ => 
            {
                Rigidbody2D.isKinematic = false;
                PlayerState.IsClimbable.Value = false;
            });
    }
}
