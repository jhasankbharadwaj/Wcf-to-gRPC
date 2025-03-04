// src/SyntaxAnalyzer/WcfSyntaxAnalyzer.cs
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WcfAnalyzer.Common;

namespace WcfAnalyzer.SyntaxAnalyzer
{
    public class WcfSyntaxAnalyzer
    {
        private readonly string _projectPath;
        private readonly AnalyzerConfig _config;

        public WcfSyntaxAnalyzer(string projectPath, AnalyzerConfig config)
        {
            _projectPath = projectPath;
            _config = config;
        }

        public async Task<List<WcfServiceInfo>> AnalyzeProjectAsync()
        {
            Console.WriteLine($"Analyzing project: {_projectPath}");
            
            var workspace = MSBuildWorkspace.Create();
            var project = await workspace.OpenProjectAsync(_projectPath);
            
            if (workspace.Diagnostics.Any())
            {
                foreach (var diagnostic in workspace.Diagnostics)
                {
                    Console.WriteLine($"Workspace error: {diagnostic.Message}");
                }
            }

            var services = new List<WcfServiceInfo>();
            
            foreach (var document in project.Documents)
            {
                var syntaxTree = await document.GetSyntaxTreeAsync();
                var root = await syntaxTree.GetRootAsync();
                
                var serviceContracts = root
                    .DescendantNodes()
                    .OfType<InterfaceDeclarationSyntax>()
                    .Where(i => HasServiceContractAttribute(i));
                
                foreach (var contract in serviceContracts)
                {
                    var serviceInfo = ExtractServiceInfo(contract, document.FilePath);
                    services.Add(serviceInfo);
                }
            }
            
            Console.WriteLine($"Found {services.Count} WCF service contracts");
            return services;
        }

        private bool HasServiceContractAttribute(InterfaceDeclarationSyntax interfaceSyntax)
        {
            return interfaceSyntax.AttributeLists
                .SelectMany(al => al.Attributes)
                .Any(a => a.Name.ToString().Contains("ServiceContract"));
        }

        private WcfServiceInfo ExtractServiceInfo(InterfaceDeclarationSyntax contract, string filePath)
        {
            var serviceInfo = new WcfServiceInfo
            {
                ServiceName = contract.Identifier.Text,
                FilePath = filePath,
                Namespace = GetNamespace(contract),
                Operations = new List<WcfOperationInfo>()
            };

            foreach (var member in contract.Members.OfType<MethodDeclarationSyntax>())
            {
                if (HasOperationContractAttribute(member))
                {
                    var operationInfo = new WcfOperationInfo
                    {
                        Name = member.Identifier.Text,
                        ReturnType = member.ReturnType.ToString(),
                        Parameters = member.ParameterList.Parameters
                            .Select(p => new WcfParameterInfo
                            {
                                Name = p.Identifier.Text,
                                Type = p.Type.ToString()
                            })
                            .ToList()
                    };
                    
                    serviceInfo.Operations.Add(operationInfo);
                }
            }

            return serviceInfo;
        }

        private string GetNamespace(InterfaceDeclarationSyntax contract)
        {
            var parent = contract.Parent;
            while (parent != null && !(parent is NamespaceDeclarationSyntax))
            {
                parent = parent.Parent;
            }
            
            return parent is NamespaceDeclarationSyntax ns ? ns.Name.ToString() : string.Empty;
        }

        private bool HasOperationContractAttribute(MethodDeclarationSyntax methodSyntax)
        {
            return methodSyntax.AttributeLists
                .SelectMany(al => al.Attributes)
                .Any(a => a.Name.ToString().Contains("OperationContract"));
        }
    }
}