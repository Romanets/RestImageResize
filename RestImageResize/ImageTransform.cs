namespace RestImageResize
{
    /// <summary>
    /// Enumerates the supported image transformation types.
    /// </summary>
    public enum ImageTransform
    {
        /// <summary>
        /// The scale to fit transformation.
        /// </summary>
        Fit = 0,

        /// <summary>
        /// The scale to fill transformation.
        /// </summary>
        Fill = 1,

        /// <summary>
        /// The scale down to fit transformation.
        /// </summary>
        DownFit = 2,

        /// <summary>
        /// The scale down to fill transformation.
        /// </summary>
        DownFill = 3,

        /// <summary>
        /// The central crop transformation.
        /// </summary>
        Crop = 4,

        /// <summary>
        /// The stretch transformation.
        /// </summary>
        Stretch = 5,

        /// <summary>
        /// 
        /// </summary>
        ResizeMin = 6,
        
        /// <summary>
        /// 
        /// </summary>
        DownResizeMin = 7,
    }
}