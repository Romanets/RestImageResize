namespace RestImageResize
{
    /// <summary>
    /// Represents the image crop area
    /// </summary>
    public class CropArea
    {
        /// <summary>
        /// Gets or sets the coordinates of crop start point
        /// </summary>
        public Coordinates CropPoint { get; set; }

        /// <summary>
        /// Gets or sets the height of crop area
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the width of crop area
        /// </summary>
        public int Width { get; set; }
    }
}