namespace Web.Api.ServiceConfigurations
{
    public static class ServiceCollectionTypeResolversExtensions
    {
       static Dictionary<string, object> namedDependencies = new Dictionary<string, object>();

        public static IServiceCollection AddServiceTypeResolvers(this IServiceCollection serviceCollection, Dictionary<string, object> _namedDependencies)
        {
            namedDependencies = _namedDependencies;
            serviceCollection.AddSingleton(GetDependencyTypeResolverByName<string>());

            return serviceCollection;
        }
        private static Func<string, T> GetDependencyTypeResolverByName<T>()
        {
            return dependencyName =>
            {
                if (namedDependencies.TryGetValue(dependencyName, out var dependency))
                {
                    return (T)dependency;
                }

                throw new InvalidOperationException($"Cannot resolve dependency with the name: '{dependencyName}'");
            };
        }
    }
}
