using System.Collections.Generic;

namespace WcfAnalyzer.Common
{
    public class WcfServiceInfo
    {
        public string ServiceName { get; set; }
        public string FilePath { get; set; }
        public string Namespace { get; set; }
        public List<WcfOperationInfo> Operations { get; set; }
    }

    public class WcfOperationInfo
    {
        public string Name { get; set; }
        public string ReturnType { get; set; }
        public List<WcfParameterInfo> Parameters { get; set; }
    }

    public class WcfParameterInfo
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public class AnalyzerConfig
    {
        public bool IncludeComments { get; set; } = true;
        public bool AnalyzeDataContracts { get; set; } = true;
        public string[] ExcludedNamespaces { get; set; } = new string[0];
    }

    public class ChromaConfig
    {
        public string CollectionName { get; set; } = "wcf_services";
        public string PersistenceDirectory { get; set; } = "./chroma_db";
        public int EmbeddingDimension { get; set; } = 384;
    }

    public class ModelConfig
    {
        public string ModelName { get; set; } = "deepseek-coder:33b";
        public int MaxTokens { get; set; } = 2048;
        public float Temperature { get; set; } = 0.1;
        public string PromptTemplate { get; set; } = "Generate a Protocol Buffer definition for the following WCF service:\n{service_description}";
    }
}