using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using System.Linq;
using UnityStandardAssets.CrossPlatformInput;
using System;
using Wolio.Actor.Player;

public class study1 : MonoBehaviour
{

    void Awake()
    {
    }

    void Start()
    {
        var TestSubject = new Subject<String>();

        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                bool Left = false;
                bool Right = false;
                bool Up = false;
                bool Down = false;
                bool Z = false;

                if (Input.GetAxisRaw("Horizontal") == -1) Left = true;
                else if (Input.GetAxisRaw("Horizontal") == 1) Right = true;
                else if (Input.GetAxisRaw("Vertical") == -1) Down = true;
                else if (Input.GetButton("Z")) Z = true;

                if (Left) TestSubject.OnNext("left");
                else if (Right) TestSubject.OnNext("right");
                else if (Up) TestSubject.OnNext("Up");
                else if (Down) TestSubject.OnNext("Down");
                else if (Z) TestSubject.OnNext("Z");
                else TestSubject.OnNext("none");
            });

        //TestSubject.Buffer(10).SelectMany(x => x).Subscribe(Debug.Log);

        //var a = this.UpdateAsObservable().Select(x => 1);
        //var b = this.UpdateAsObservable().Select(x => 2);
        //var c = this.UpdateAsObservable().Select(x => 3);
        //var mix = Observable.Merge(a, b, c).Buffer(3);
        //mix.SelectMany(x => x).Subscribe(_ => Debug.Log(_));
        
        //var left = this.UpdateAsObservable().Select(x => Input.GetAxisRaw("Horizontal")).Select(x => x == -1 ? "left" : "none");
        //var right = this.UpdateAsObservable().Select(x => Input.GetAxisRaw("Horizontal")).Select(x => x == 1 ? "right" : "none");
        //var down = this.UpdateAsObservable().Select(x => Input.GetAxisRaw("Vertical")).Select(x => x == -1 ? "down" : "none");
        //var z = this.UpdateAsObservable().Select(x => Input.GetButton("Z")).Select(x => x ? "Z" : "none");

        ////var left = this.UpdateAsObservable().Select(x => Input.GetAxisRaw("Horizontal")).Select(x => x == -1 ? "left" : "none").DistinctUntilChanged();
        ////var right = this.UpdateAsObservable().Select(x => Input.GetAxisRaw("Horizontal")).Select(x => x == 1 ? "right" : "none").DistinctUntilChanged();
        ////var down = this.UpdateAsObservable().Select(x => Input.GetAxisRaw("Vertical")).Select(x => x == -1 ? "down" : "none").DistinctUntilChanged();
        ////var z = this.UpdateAsObservable().Select(x => Input.GetButton("Z")).Select(x => x ? "Z" : "none").DistinctUntilChanged();
        ////var fireball = Observable.Merge(left, right, down, z).Buffer(3);
        ////var fireball = Observable.Merge(left, right, down, z).Buffer(5);
        //var fireball = Observable.Amb(left).Amb(right).Amb(down).Amb(z);

        //fireball.First().Repeat().Subscribe(_ => Debug.Log(_));
    }
}
