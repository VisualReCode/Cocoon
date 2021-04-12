using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ReCode.Cocoon.Proxy.BlazorCodeGen
{
    internal static class TemplateFinder
    {
        private const string RouteAttributeName = "Microsoft.AspNetCore.Components.RouteAttribute";

        private static readonly HashSet<string> RouteAttributeNames = new()
        {
            RouteAttributeName,
            "Route",
            "RouteAttribute"
        };
        
        public static ImmutableArray<string> GetRouteTemplates(Compilation compilation)
        {
            // Get all classes
            IEnumerable<SyntaxNode> allNodes = compilation.SyntaxTrees.SelectMany(s => s.GetRoot().DescendantNodes());
            IEnumerable<ClassDeclarationSyntax> allClasses = allNodes
                .Where(d => d.IsKind(SyntaxKind.ClassDeclaration))
                .OfType<ClassDeclarationSyntax>();
            
            return allClasses
                .Select(component => GetRoutePath(compilation, component))
                .Where(route => route is not null)
                .Cast<string>()// stops the nullable lies
                .ToImmutableArray();
        }
        
        private static string? GetRoutePath(Compilation compilation, ClassDeclarationSyntax component)
        {
            var routeAttribute = component.AttributeLists
                .SelectMany(x => x.Attributes)
                .FirstOrDefault(attr => RouteAttributeNames.Contains(attr.Name.ToString()));
                
            if (routeAttribute?.ArgumentList?.Arguments.Count != 1)
            {
                // no route path
                return null;
            }
                
            var semanticModel = compilation.GetSemanticModel(component.SyntaxTree);

            var symbol = semanticModel.GetSymbolInfo(routeAttribute).Symbol;

            if (symbol is IMethodSymbol methodSymbol)
            {
                if (methodSymbol.ContainingType is INamedTypeSymbol namedTypeSymbol)
                {
                    var fullName = $"{namedTypeSymbol.ContainingNamespace}.{namedTypeSymbol.Name}";
                    if (fullName == RouteAttributeName)
                    {
                        var routeArg = routeAttribute.ArgumentList.Arguments[0];
                        var routeExpr = routeArg.Expression;
                        return semanticModel.GetConstantValue(routeExpr).ToString();
                    }
                }
            }

            return null;
        }
    }
}