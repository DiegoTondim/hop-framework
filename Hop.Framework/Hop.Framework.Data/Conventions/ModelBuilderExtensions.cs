using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using System.Reflection;

namespace Hop.Framework.EFCore.Conventions
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder AddConvention(this ModelBuilder modelBuilder, IModelConvention convention)
        {
            var imb = modelBuilder.GetInfrastructure();
            var cd = imb.Metadata.ConventionDispatcher;
            var cs = cd.GetType().GetField("_scope", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(cd);
            var cset = cs.GetType().GetField("_conventionSet", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(cs) as ConventionSet;

            cset.ModelBuiltConventions.Add(convention);

            return modelBuilder;
        }

        public static ModelBuilder AddConvention<TConvention>(this ModelBuilder modelBuilder) where TConvention : IModelConvention, new()
        {
            return modelBuilder.AddConvention(new TConvention());
        }
    }

    public interface IModelConvention : IModelBuiltConvention
    {

    }
}
