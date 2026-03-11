namespace WorkerManagementApi.Config
{
    /// <summary>
    ///     Setup caching
    /// </summary>
    public static class CachingConfig
    {
        /// <summary>
        ///     In-memory Caching
        /// </summary>
        /// <param name="services"></param>
        public static void SetupInMemoryCaching(this IServiceCollection services)
        {
            // Non-distributed in-memory cache services
            services.AddMemoryCache();
        }
    }
}
