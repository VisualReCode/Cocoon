using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace ReCode.Cocoon.Proxy.BlazorCodeGen
{
    [Generator]
    public class RouteTesterSourceGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
        }

        public void Execute(GeneratorExecutionContext context)
        {
            try
            {
                var allRoutePaths = TemplateFinder.GetRouteTemplates(context.Compilation);
                if (allRoutePaths.Length == 0)
                {
                    var descriptor = new DiagnosticDescriptor("Cocoon42", "No Routes",
                        "No routes were found",
                        "Cocoon", DiagnosticSeverity.Warning, true);

                    var diagnostic = Diagnostic.Create(descriptor, null);
                    context.ReportDiagnostic(diagnostic);
                    return;
                }

                var generator = new RouteTesterGenerator(allRoutePaths);
                var dictSource = SourceText.From(generator.Generate(), Encoding.UTF8);
                context.AddSource("CocoonBlazorRouteTester", dictSource);
            }
            catch (Exception)
            {
                #if(DEBUG)
                Debugger.Launch();
                #endif
            }
        }
        
    }
}
