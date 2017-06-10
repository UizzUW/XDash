using MVPathway.Builder.Abstractions;
using MVPathway.Utils.ViewModels.ViewObjects;

namespace XDash.Builder
{
    public static class XDashPathwayBuilderExtensions
    {
        public static IPathwayBuilder UseXDash(this IPathwayBuilder builder)
        {
            builder.Container.Register<>
            return builder;
        }
    }
}