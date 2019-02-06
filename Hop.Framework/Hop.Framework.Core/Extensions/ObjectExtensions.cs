namespace Hop.Framework.Core.Extensions
{
	public static class ObjectExtensions
	{
		public static TTo As<TTo>(this object @object)
		{
			return (TTo)@object;
		}
	}
}
