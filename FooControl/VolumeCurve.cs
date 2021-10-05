using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FooControl
{
    public class VolumeCurve
    {
        private bool inDBFormat;
        private VolumeCurveType curveType;

        public VolumeCurve(bool inDBFormat, VolumeCurveType curveType)
        {
            this.inDBFormat = inDBFormat;
            this.curveType = curveType;
        }

        /// <summary>
        /// Calculates the volume using a sqrt(sin(x)) curve with a max of 100.00.
        /// Optionally converted into dB format (result - 100).
        /// </summary>
        /// <param name="value">Value from 0 to 1 to calculate.</param>
        /// <returns>Double of the calculated value.</returns>
        public double calcVolumeCurve(double value)
        {
            if(value > 1)
            {
                if (inDBFormat)
                {
                    return 0;
                }
                else
                {
                    return 100;
                }
            }
            else if(value < 0)
            {
                if (inDBFormat)
                {
                    return -100;
                }
                else
                {
                    return 0;
                }
            }

            double ans = 0;

            switch (curveType)
            {
                case VolumeCurveType.Basic:
                    ans = calcBasicVolumeCurve(value);
                    break;
                case VolumeCurveType.Better:
                    ans = calcBetterVolumeCurve(value);
                    break;
                case VolumeCurveType.Flat:
                    ans = calcFlatVolumeCurve(value);
                    break;
                default:
                    break;
            }

            if (inDBFormat)
            {
                ans = ans - 100;
            }

            return ans;
        }

        /// <summary>
        /// Will revert a curved value (0 to 100) to a slider value (0 to 1).
        /// </summary>
        /// <param name="value">Value from 0 to 100 to revert.</param>
        /// <returns>Double of the reverted value.</returns>
        public double revertVolumeCurve(double value)
        {
            if (inDBFormat)
            {
                if (value > 0)
                {
                    return 1;
                }
                else if (value < -100)
                {
                    return 0;
                }
            }
            else
            {
                if (value > 100)
                {
                    return 1;
                }
                else if (value < 0)
                {
                    return 0;
                }
            }

            double ans = 0;

            if (inDBFormat)
            {
                value = value + 100;
            }

            switch (curveType)
            {
                case VolumeCurveType.Basic:
                    ans = revertBasicVolumeCurve(value);
                    break;
                case VolumeCurveType.Better:
                    ans = revertBetterVolumeCurve(value);
                    break;
                case VolumeCurveType.Flat:
                    ans = revertFlatVolumeCurve(value);
                    break;
                default:
                    break;
            }

            return ans;
        }

        private double calcBasicVolumeCurve(double value)
        {
            return Math.Sqrt(Math.Sin(value)) * (100 / Math.Sqrt(Math.Sin(1)));
        }

        private double revertBasicVolumeCurve(double value)
        {
            return Math.Asin(Math.Pow(value / (100 / Math.Sqrt(Math.Sin(1))), 2));
        }

        private double calcBetterVolumeCurve(double value)
        {
            return Math.Pow(Math.Sin(value), 1.0 / 4.0) * (100 / Math.Pow(Math.Sin(1), 1.0 / 4.0));
        }

        private double revertBetterVolumeCurve(double value)
        {
            double result = Math.Asin(Math.Pow(value / (100 / Math.Pow(Math.Sin(1), 1.0 / 4.0)), 4));

            //result of 100 is not perfect (e.g. 1.0000000000000007)
            if(result > 1)
            {
                result = 1;
            }

            return result;
        }

        private double calcFlatVolumeCurve(double value)
        {
            double result = Math.Pow(Math.E, Math.Log(100)) * value;

            //result of 1 is not perfect (e.g. 100.00000000000001)
            if (result > 100)
            {
                result = 100;
            }

            return result;
        }

        private double revertFlatVolumeCurve(double value)
        {
            double result = Math.Pow(Math.E, Math.Log(0.01)) * value;

            //result of 100 is not perfect (e.g. 1.00000000000006E-200)
            if (result > 1)
            {
                result = 1;
            }

            return result;
        }
    }

    public enum VolumeCurveType
    {
        Basic,
        Better,
        Flat
    }
}
