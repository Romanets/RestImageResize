namespace RestImageResize
{
    /// <summary>
    /// Represents the focus point of image
    /// This point will be as close to the center of your crop as possible while keeping the crop within the image
    /// </summary>
    public class FocusPoint
    {
        /// <summary>
        /// Gets or sets left margin for focus point
        /// </summary>
        public float Left { get; set; }

        /// <summary>
        /// Gets or sets top margin for focus point
        /// </summary>
        public float Top { get; set; }
    }
}