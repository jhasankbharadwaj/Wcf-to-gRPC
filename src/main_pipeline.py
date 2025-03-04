# src/main_pipeline.py
import os
import json
import sys
sys.path.append(os.path.dirname(os.path.abspath(__file__)))

from SyntaxAnalyzer import WcfSyntaxAnalyzer #changed 
from VectorStorage.chroma_repository import ChromaRepository
from ProtoGeneration.proto_generator import ProtoGenerator

def load_config(config_path):
    """Load configuration from JSON file"""
    with open(config_path, 'r') as f:
        return json.load(f)

def main():
    # Configuration paths
    CONFIG_PATHS = {
        'analyzer': './config/analyzer_config.json',
        'chroma': './config/chroma_config.json',
        'model': './config/model_config.json'
    }

    # Initialize components
    try:
        # Load configurations
        analyzer_config = load_config(CONFIG_PATHS['analyzer'])
        chroma_config = load_config(CONFIG_PATHS['chroma'])
        model_config = load_config(CONFIG_PATHS['model'])

        # Project to analyze (can be passed as argument or configured)
        project_path = sys.argv[1] if len(sys.argv) > 1 else analyzer_config.get('projectPath')
        
        if not project_path:
            raise ValueError("No project path specified. Please provide a .csproj file path.")
        
        print(f"Analyzing project: {project_path}")

        # 1. Syntax Analysis
        syntax_analyzer = WcfSyntaxAnalyzer(project_path, analyzer_config)
        wcf_services = syntax_analyzer.AnalyzeProjectAsync()

        # 2. Vector Storage
        chroma_repo = ChromaRepository(CONFIG_PATHS['chroma'])
        chroma_repo.store_service_info(wcf_services)

        # 3. Proto Generation
        proto_generator = ProtoGenerator(CONFIG_PATHS['model'])
        generated_protos = proto_generator.generate_proto_files(wcf_services)

        # 4. Generate Summary Report
        generate_summary_report(wcf_services, generated_protos)

    except Exception as e:
        print(f"Error in WCF Analyzer Pipeline: {e}")
        sys.exit(1)

def generate_summary_report(services, proto_files):
    """Generate a summary report of the analysis"""
    report = {
        'total_services': len(services),
        'total_operations': sum(len(service['operations']) for service in services),
        'generated_proto_files': proto_files
    }

    with open('./output/analysis_summary.json', 'w') as f:
        json.dump(report, f, indent=2)
    
    print("\n--- Analysis Summary ---")
    print(f"Total Services Analyzed: {report['total_services']}")
    print(f"Total Operations: {report['total_operations']}")
    print("Generated Proto Files:")
    for proto in proto_files:
        print(f"  - {proto}")

if __name__ == '__main__':
    main()