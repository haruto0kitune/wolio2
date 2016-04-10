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
}
