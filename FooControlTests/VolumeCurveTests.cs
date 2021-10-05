using FooControl;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FooControlTests
{
    [TestClass]
    public class VolumeCurveTests
    {
        [TestMethod]
        public void calcVolumeCurve_BasicTest100()
        {
            double inputValue = 1;
            double expected = 100;

            VolumeCurve volumeCurve = new VolumeCurve(false, VolumeCurveType.Basic);
            double actual = volumeCurve.calcVolumeCurve(inputValue);

            Assert.AreEqual(expected, actual, 0, "Max volume is not 100!");
        }

        [TestMethod]
        public void calcVolumeCurve_BasicTest0()
        {
            double inputValue = 0;
            double expected = 0;

            VolumeCurve volumeCurve = new VolumeCurve(false, VolumeCurveType.Basic);
            double actual = volumeCurve.calcVolumeCurve(inputValue);

            Assert.AreEqual(expected, actual, 0, "Min volume is not 0!");
        }

        [TestMethod]
        public void calcVolumeCurve_BasicTest50()
        {
            double inputValue = 0.5;
            double expected = 75.48;

            VolumeCurve volumeCurve = new VolumeCurve(false, VolumeCurveType.Basic);
            double actual = volumeCurve.calcVolumeCurve(inputValue);

            Assert.AreEqual(expected, actual, 0.01, "Output value is not expected on basic curve!");
        }

        [TestMethod]
        public void calcVolumeCurve_BetterTest100()
        {
            double inputValue = 1;
            double expected = 100;

            VolumeCurve volumeCurve = new VolumeCurve(false, VolumeCurveType.Better);
            double actual = volumeCurve.calcVolumeCurve(inputValue);

            Assert.AreEqual(expected, actual, 0, "Max volume is not 100!");
        }

        [TestMethod]
        public void calcVolumeCurve_BetterTest0()
        {
            double inputValue = 0;
            double expected = 0;

            VolumeCurve volumeCurve = new VolumeCurve(false, VolumeCurveType.Better);
            double actual = volumeCurve.calcVolumeCurve(inputValue);

            Assert.AreEqual(expected, actual, 0, "Min volume is not 0!");
        }

        [TestMethod]
        public void calcVolumeCurve_BetterTest50()
        {
            double inputValue = 0.5;
            double expected = 86.88;

            VolumeCurve volumeCurve = new VolumeCurve(false, VolumeCurveType.Better);
            double actual = volumeCurve.calcVolumeCurve(inputValue);

            Assert.AreEqual(expected, actual, 0.01, "Output value is not expected on better curve!");
        }

        [TestMethod]
        public void calcVolumeCurve_FlatTest100()
        {
            double inputValue = 1;
            double expected = 100;

            VolumeCurve volumeCurve = new VolumeCurve(false, VolumeCurveType.Flat);
            double actual = volumeCurve.calcVolumeCurve(inputValue);

            Assert.AreEqual(expected, actual, 0, "Max volume is not 100!");
        }

        [TestMethod]
        public void calcVolumeCurve_FlatTest0()
        {
            double inputValue = 0;
            double expected = 0;

            VolumeCurve volumeCurve = new VolumeCurve(false, VolumeCurveType.Flat);
            double actual = volumeCurve.calcVolumeCurve(inputValue);

            Assert.AreEqual(expected, actual, 0, "Min volume is not 0!");
        }

        [TestMethod]
        public void calcVolumeCurve_FlatTest50()
        {
            double inputValue = 0.5;
            double expected = 50;

            VolumeCurve volumeCurve = new VolumeCurve(false, VolumeCurveType.Flat);
            double actual = volumeCurve.calcVolumeCurve(inputValue);

            Assert.AreEqual(expected, actual, 0.01, "Output value is not expected on flat curve!");
        }

        [TestMethod]
        public void calcVolumeCurve_TestGreater100()
        {
            double inputValue = 1.1;
            double expected = 100;

            VolumeCurve volumeCurve = new VolumeCurve(false, VolumeCurveType.Basic);
            double actual = volumeCurve.calcVolumeCurve(inputValue);

            Assert.AreEqual(expected, actual, 0, "Volume is greater than 100!");
        }

        [TestMethod]
        public void calcVolumeCurve_TestLess0()
        {
            double inputValue = -0.1;
            double expected = 0;

            VolumeCurve volumeCurve = new VolumeCurve(false, VolumeCurveType.Basic);
            double actual = volumeCurve.calcVolumeCurve(inputValue);

            Assert.AreEqual(expected, actual, 0, "Volume is less than 0!");
        }

        [TestMethod]
        public void calcVolumeCurve_TestDBFormatGreater100()
        {
            double inputValue = 2;
            double expected = 0;

            VolumeCurve volumeCurve = new VolumeCurve(true, VolumeCurveType.Basic);
            double actual = volumeCurve.calcVolumeCurve(inputValue);

            Assert.AreEqual(expected, actual, 0, "Volume DB format is incorrect!");
        }

        [TestMethod]
        public void calcVolumeCurve_TestDBFormatLess0()
        {
            double inputValue = -1;
            double expected = -100;

            VolumeCurve volumeCurve = new VolumeCurve(true, VolumeCurveType.Basic);
            double actual = volumeCurve.calcVolumeCurve(inputValue);

            Assert.AreEqual(expected, actual, 0, "Volume DB format is incorrect!");
        }

        [TestMethod]
        public void calcVolumeCurve_TestDBFormat()
        {
            double inputValue = 1;
            double expected = 0;

            VolumeCurve volumeCurve = new VolumeCurve(true, VolumeCurveType.Basic);
            double actual = volumeCurve.calcVolumeCurve(inputValue);

            Assert.AreEqual(expected, actual, 0, "Volume DB format is incorrect!");
        }

        [TestMethod]
        public void revertVolumeCurve_BasicTest100()
        {
            double inputValue = 100;
            double expected = 1;

            VolumeCurve volumeCurve = new VolumeCurve(false, VolumeCurveType.Basic);
            double actual = volumeCurve.revertVolumeCurve(inputValue);

            Assert.AreEqual(expected, actual, 0, "Max volume is not 1!");
        }

        [TestMethod]
        public void revertVolumeCurve_BasicTest0()
        {
            double inputValue = 0;
            double expected = 0;

            VolumeCurve volumeCurve = new VolumeCurve(false, VolumeCurveType.Basic);
            double actual = volumeCurve.revertVolumeCurve(inputValue);

            Assert.AreEqual(expected, actual, 0, "Min volume is not 0!");
        }

        [TestMethod]
        public void revertVolumeCurve_BasicTest50()
        {
            double inputValue = 75.48;
            double expected = 0.5;

            VolumeCurve volumeCurve = new VolumeCurve(false, VolumeCurveType.Basic);
            double actual = volumeCurve.revertVolumeCurve(inputValue);

            Assert.AreEqual(expected, actual, 0.01, "Output value is not expected on basic curve!");
        }

        [TestMethod]
        public void revertVolumeCurve_BetterTest100()
        {
            double inputValue = 100;
            double expected = 1;

            VolumeCurve volumeCurve = new VolumeCurve(false, VolumeCurveType.Better);
            double actual = volumeCurve.revertVolumeCurve(inputValue);

            Assert.AreEqual(expected, actual, 0, "Max volume is not 1!");
        }

        [TestMethod]
        public void revertVolumeCurve_BetterTest0()
        {
            double inputValue = 0;
            double expected = 0;

            VolumeCurve volumeCurve = new VolumeCurve(false, VolumeCurveType.Better);
            double actual = volumeCurve.revertVolumeCurve(inputValue);

            Assert.AreEqual(expected, actual, 0, "Min volume is not 0!");
        }

        [TestMethod]
        public void revertVolumeCurve_BetterTest50()
        {
            double inputValue = 86.88;
            double expected = 0.5;

            VolumeCurve volumeCurve = new VolumeCurve(false, VolumeCurveType.Better);
            double actual = volumeCurve.revertVolumeCurve(inputValue);

            Assert.AreEqual(expected, actual, 0.01, "Output value is not expected on better curve!");
        }

        [TestMethod]
        public void revertVolumeCurve_FlatTest100()
        {
            double inputValue = 100;
            double expected = 1;

            VolumeCurve volumeCurve = new VolumeCurve(false, VolumeCurveType.Flat);
            double actual = volumeCurve.revertVolumeCurve(inputValue);

            Assert.AreEqual(expected, actual, 0, "Max volume is not 100!");
        }

        [TestMethod]
        public void revertVolumeCurve_FlatTest0()
        {
            double inputValue = 0;
            double expected = 0;

            VolumeCurve volumeCurve = new VolumeCurve(false, VolumeCurveType.Flat);
            double actual = volumeCurve.revertVolumeCurve(inputValue);

            Assert.AreEqual(expected, actual, 0, "Min volume is not 0!");
        }

        [TestMethod]
        public void revertVolumeCurve_FlatTest50()
        {
            double inputValue = 50;
            double expected = 0.5;

            VolumeCurve volumeCurve = new VolumeCurve(false, VolumeCurveType.Flat);
            double actual = volumeCurve.revertVolumeCurve(inputValue);

            Assert.AreEqual(expected, actual, 0.01, "Output value is not expected on flat curve!");
        }

        [TestMethod]
        public void revertVolumeCurve_TestGreater100()
        {
            double inputValue = 200;
            double expected = 1;

            VolumeCurve volumeCurve = new VolumeCurve(false, VolumeCurveType.Basic);
            double actual = volumeCurve.revertVolumeCurve(inputValue);

            Assert.AreEqual(expected, actual, 0, "Volume is greater than 100!");
        }

        [TestMethod]
        public void revertVolumeCurve_TestLess0()
        {
            double inputValue = -100;
            double expected = 0;

            VolumeCurve volumeCurve = new VolumeCurve(false, VolumeCurveType.Basic);
            double actual = volumeCurve.revertVolumeCurve(inputValue);

            Assert.AreEqual(expected, actual, 0, "Volume is less than 0!");
        }

        [TestMethod]
        public void revertVolumeCurve_TestDBFormatGreater100()
        {
            double inputValue = 10;
            double expected = 1;

            VolumeCurve volumeCurve = new VolumeCurve(true, VolumeCurveType.Basic);
            double actual = volumeCurve.revertVolumeCurve(inputValue);

            Assert.AreEqual(expected, actual, 0, "Volume DB format is incorrect!");
        }

        [TestMethod]
        public void revertVolumeCurve_TestDBFormatLess0()
        {
            double inputValue = -200;
            double expected = 0;

            VolumeCurve volumeCurve = new VolumeCurve(true, VolumeCurveType.Basic);
            double actual = volumeCurve.revertVolumeCurve(inputValue);

            Assert.AreEqual(expected, actual, 0, "Volume DB format is incorrect!");
        }

        [TestMethod]
        public void revertVolumeCurve_TestDBFormat()
        {
            double inputValue = 0;
            double expected = 1;

            VolumeCurve volumeCurve = new VolumeCurve(true, VolumeCurveType.Basic);
            double actual = volumeCurve.revertVolumeCurve(inputValue);

            Assert.AreEqual(expected, actual, 0, "Volume DB format is incorrect!");
        }
    }
}
