namespace RestImageResize.Security
{
    /// <summary>
    /// Represents a hash generator for image transformation queries.
    /// </summary>
    public interface IHashGenerator
    {
        /// <summary>
        /// Computes a hash for provided image transformation parameters.
        /// </summary>
        /// <param name="privateKey">Private key received from the image service administrator.</param>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <param name="transform">Image transformation mode.</param>
        string ComputeHash(string privateKey, int width, int height, ImageTransform transform);
    }
}