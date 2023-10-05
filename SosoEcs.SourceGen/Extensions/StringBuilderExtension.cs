using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Text;

namespace SosoEcs.SourceGen.Extensions
{
	public static class StringBuilderExtension
	{
		public static string ParseCS(this StringBuilder sb)
		{
			return CSharpSyntaxTree.ParseText(sb.ToString()).GetRoot().NormalizeWhitespace().ToFullString();
		}
	}
}
