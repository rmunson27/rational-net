using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RemTest.Core.Numerics;

using TInt = BigInteger;
using TRatio = BigRatio;

/// <summary>
/// Tests of the <see cref="TRatio"/> struct.
/// </summary>
[TestClass]
public class BigRatioTest
{
    #region Properties
    /// <summary>
    /// Tests the <see cref="TRatio.Reciprocal"/> property.
    /// </summary>
    [TestMethod]
    public void TestReciprocal()
    {
        Assert.That.RatioEquals(3, 1, TRatio.CreateOneOver(3).Reciprocal); // Control - should not change

        // Sign should be propagated through the numerator
        Assert.That.RatioEquals(-4, 1, TRatio.CreateOneOver(-4).Reciprocal);

        Assert.ThrowsException<DivideByZeroException>(() => TRatio.Create(0).Reciprocal);
    }
    #endregion

    #region Factory Methods
    /// <summary>
    /// Tests the <see cref="TRatio.Create(TInt, TInt)"/> factory method.
    /// </summary>
    [TestMethod]
    public void TestCreate()
    {
        Assert.That.RatioEquals(3, 4, TRatio.Create(3, 4)); // Control - should be unchanged
        Assert.That.RatioEquals(0, 1, TRatio.Create(0, 4)); // 0 numerator should reduce denominator to 1
        Assert.That.RatioEquals(1, 3, TRatio.Create(2, 6)); // Should be reduced
        Assert.That.RatioEquals(-3, 4, TRatio.Create(3, -4)); // Sign should be propagated through the numerator
    }

    /// <summary>
    /// Tests the <see cref="TRatio.Create(TInt)"/> factory method.
    /// </summary>
    [TestMethod]
    public void TestCreateNumerator()
    {
        Assert.That.RatioEquals(3, 1, TRatio.Create(3));
        Assert.That.RatioEquals(-4, 1, TRatio.Create(-4));
        Assert.That.RatioEquals(0, 1, TRatio.Create(0));
    }

    /// <summary>
    /// Tests the <see cref="TRatio.CreateOneOver(TInt)"/> factory method.
    /// </summary>
    [TestMethod]
    public void TestCreateOneOver()
    {
        Assert.That.RatioEquals(1, 5, TRatio.CreateOneOver(5)); // Control - should be unchanged
        Assert.That.RatioEquals(-1, 4, TRatio.CreateOneOver(-4)); // Sign should be propagated through the numerator
    }
    #endregion

    #region Comparison
    /// <summary>
    /// Tests comparison of two <see cref="TRatio"/> instances.
    /// </summary>
    [TestMethod]
    public void TestRatioComparison()
    {
        var oneHalf = TRatio.Create(1, 2);
        var two = TRatio.Create(2, 1);

        // Non-equal
        Assert.IsTrue(oneHalf < two);
        Assert.IsTrue(two > oneHalf);
        Assert.IsTrue(oneHalf <= two);
        Assert.IsTrue(two >= oneHalf);

        // Equal
#pragma warning disable CS1718 // We are testing the operators
        Assert.IsFalse(oneHalf < oneHalf);
        Assert.IsFalse(oneHalf > oneHalf);
        Assert.IsTrue(oneHalf <= oneHalf);
        Assert.IsTrue(oneHalf >= oneHalf);
#pragma warning restore CS1718
    }

    /// <summary>
    /// Tests comparison of a <see cref="TRatio"/> and a <see cref="TInt"/>.
    /// </summary>
    [TestMethod]
    public void TestIntComparison()
    {
        var sevenFourths = TRatio.Create(7, 4);
        var two = TRatio.Create(2);

        // Not equal
        Assert.IsTrue(sevenFourths < 2);
        Assert.IsTrue(2 > sevenFourths);
        Assert.IsTrue(sevenFourths <= 2);
        Assert.IsTrue(2 >= sevenFourths);

        // Equal
        Assert.IsFalse(two < 2);
        Assert.IsFalse(2 < two);
        Assert.IsFalse(two > 2);
        Assert.IsFalse(2 > two);
        Assert.IsTrue(two >= 2);
        Assert.IsTrue(2 >= two);
        Assert.IsTrue(two <= 2);
        Assert.IsTrue(2 <= two);
    }
    #endregion
}
