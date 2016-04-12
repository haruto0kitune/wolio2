using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class UtilityTest
{
    [Test]
    public void DegToRadTest()
    {
        Assert.AreEqual(Utility.DegToRad(30), Utility.DegToRad(Utility.RadToDeg(Utility.DegToRad(30))));
        Assert.AreEqual(Utility.DegToRad(45), Utility.DegToRad(Utility.RadToDeg(Utility.DegToRad(45))));
    }

    [Test]
    public void RadToDegTest()
    {
        Assert.AreEqual(Utility.RadToDeg(Mathf.PI / 2), 90);
        Assert.AreEqual(Utility.RadToDeg(Mathf.PI / 4), 45);
    }

    [Test]
    public void PolarToRectanguler2DTest()
    {
        var Rectanguler1 = new Vector2(1, 1);
        var Rectanguler2 = Utility.PolarToRectangular2D(45, Mathf.Sqrt(2));

        Assert.AreEqual(Rectanguler1.x, Rectanguler2.x);
        Assert.AreEqual(Rectanguler1.y, Rectanguler2.y);

        Rectanguler1 = new Vector2(1, 2);
        Rectanguler2 = Utility.PolarToRectangular2D(60, Mathf.Sqrt(3));

        Assert.AreEqual(Rectanguler1.x, Rectanguler2.x);
        Assert.AreEqual(Rectanguler1.y, Rectanguler2.y);
    }

    [Test]
    public void RectangulerToPolar2DTest()
    {
        var Polar1 = new Vector2(Mathf.Sqrt(2), 45);
        var Polar2 = Utility.RectangularToPolar2D(1f, 1f);

        Assert.AreEqual(Polar1.x, Polar2.x);
        Assert.AreEqual(Polar1.y, Polar2.y);
    }
}
