namespace RestImageResize.Security
{
    /// <summary>
    /// Authorizes image transformation queries.
    /// </summary>
    public interface IQueryAuthorizer
    {
        /// <summary>
        /// Checks if image transformation query is allowed.
        /// </summary>
        /// <param name="query">Image transformation query.</param>
        bool IsAuthorized(ImageTransformQuery query);
    }
}