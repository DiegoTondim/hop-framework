using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Hop.Framework.EFCore.Conventions
{
    public sealed class MaxLengthConvention : IModelConvention
    {
        internal const int DefaultStringLength = 100;
        internal const string MaxLengthAnnotation = "MaxLength";

        private readonly int _defaultStringLength;

        public MaxLengthConvention(int defaultStringLength = DefaultStringLength)
        {
            this._defaultStringLength = defaultStringLength;
        }

        public InternalModelBuilder Apply(InternalModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Metadata.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties())
                {
                    if (property.ClrType == typeof(string))
                    {
                        if (property.FindAnnotation(MaxLengthAnnotation) == null)
                        {
                            property.AddAnnotation(MaxLengthAnnotation, this._defaultStringLength);
                        }
                    }
                }
            }

            return modelBuilder;
        }
    }
}
