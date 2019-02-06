using System.Linq.Expressions;

namespace Hop.Framework.EFCore.Repository
{
	internal class SoftDeleteVisitor : ExpressionVisitor
	{
		private static readonly ExpressionVisitor Default = new SoftDeleteVisitor();

		private SoftDeleteVisitor()
		{
		}

		public new static Expression Visit(Expression node)
		{
			return Default.Visit(node);
		}

		protected override Expression VisitUnary(UnaryExpression node)
		{
			if (node.NodeType == ExpressionType.Convert
				&& node.Type.IsAssignableFrom(node.Operand.Type))
			{
				return base.Visit(node.Operand);
			}
			return base.VisitUnary(node);
		}
	}
}
