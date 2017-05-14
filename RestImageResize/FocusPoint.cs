using System;
using System.Globalization;
using RestImageResize.Utils;

namespace RestImageResize
{
    /// <summary>
    /// Represents the focus point of the image
    /// This point will be as close to the center of your crop as possible while keeping the crop within the image
    /// </summary>
    public class FocusPoint
    {
        /// <summary>
        /// default ctor
        /// </summary>
        public FocusPoint()
            : this(0,0)
        {}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left">fraction: value between 0 and 1</param>
        /// <param name="top">fraction: value between 0 and 1</param>
        public FocusPoint(double left, double top)
        {
            Left = left;
            Top = top;
        }

        /// <summary>
        /// Gets or the left margin for focus point (fraction: value between 0 and 1)
        /// </summary>
        public double Left { get; }

        /// <summary>
        /// Gets or sets top margin for focus point
        /// </summary>
        public double Top { get; }

        /// <summary>
        /// Parses string to focus point
        /// </summary>
        /// <param name="value"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static FocusPoint TryParse(string value, string separator = ",")
        {
            if (string.IsNullOrEmpty(value))
                return null;

            var fractions = value.Split(new[] {separator}, StringSplitOptions.None);

            var fractionX = SmartConvert.ChangeType<double>(fractions[0]);
            var fractionY = SmartConvert.ChangeType<double>(fractions[1]);

            return new FocusPoint(fractionX, fractionY);
        }

        public static FocusPoint Parse(string value, string separator = ",")
        {
            var result = FocusPoint.TryParse(value);
            if (result == Empty)
                throw new FormatException($"Unable to parse focus point from string '{value}'");
            return result;
        }

        public override string ToString()
        {
            return $"{Left.ToString(CultureInfo.InvariantCulture)},{Top.ToString(CultureInfo.InvariantCulture)}";
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly FocusPoint Empty = new FocusPoint(-1, -1);
    }
}