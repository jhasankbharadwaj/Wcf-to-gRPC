# src/ProtoGeneration/proto_generator.py (continued)
import json
import os
import re
import subprocess
import time
from typing import Dict, List, Any
import ollama

class ProtoGenerator:
    def __init__(self, config_path: str):
        with open(config_path, 'r') as f:
            self.config = json.load(f)
        
        self.model_name = self.config.get("modelName", "deepseek-coder:33b")
        self.max_tokens = self.config.get("maxTokens", 2048)
        self.temperature = self.config.get("temperature", 0.1)
        self.prompt_template = self.config.get(
            "promptTemplate", 
            "Generate a Protocol Buffer definition for the following WCF service:\n{service_description}"
        )
        
        # Ensure Ollama is running
        self._ensure_ollama_running()
        
        # Create output directory if it doesn't exist
        os.makedirs("./output/proto", exist_ok=True)
    
    def _ensure_ollama_running(self):
        """Check if Ollama is running, if not start it"""
        try:
            # Simple check to see if Ollama is responding
            result = subprocess.run(
                ["ollama", "list"], 
                capture_output=True, 
                text=True, 
                timeout=5
            )
            
            if "deepseek-coder" not in result.stdout:
                print("Model not found in Ollama. Pulling the model...")
                subprocess.run(["ollama", "pull", self.model_name], check=True)
            
            print("Ollama is running with the required model")
        except (subprocess.SubprocessError, subprocess.TimeoutExpired):
            print("Ollama not running or not responding. Starting Ollama...")
            # Start Ollama in the background
            subprocess.Popen(["ollama", "serve"], stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL)
            time.sleep(5)  # Give some time for Ollama to start
    
    def _sanitize_proto_identifier(self, name: str) -> str:
        """
        Sanitize names to be valid Proto identifiers:
        - Convert to PascalCase
        - Remove non-alphanumeric characters
        - Ensure it starts with a letter
        """
        # Remove non-alphanumeric characters
        sanitized = re.sub(r'[^a-zA-Z0-9]', '', name)
        
        # Ensure starts with a letter, capitalize first letter
        if not sanitized or not sanitized[0].isalpha():
            sanitized = 'Service' + sanitized
        
        return sanitized[0].upper() + sanitized[1:]
    
    def generate_proto_definition(self, service_info: Dict[str, Any]) -> str:
        """
        Generate Protocol Buffer definition for a WCF service
        
        :param service_info: Dictionary containing service details
        :return: Generated .proto file content
        """
        # Prepare service description for the model
        service_description = f"Service Name: {service_info['serviceName']}\n"
        service_description += f"Namespace: {service_info.get('namespace', 'Unknown')}\n\n"
        service_description += "Operations:\n"
        
        for op in service_info.get('operations', []):
            params = ", ".join([f"{p['type']} {p['name']}" for p in op.get('parameters', [])])
            service_description += f"- {op['name']}({params}) : {op['returnType']}\n"
        
        # Prepare prompt for the model
        prompt = self.prompt_template.format(service_description=service_description)
        
        # Generate proto definition using Ollama
        try:
            response = ollama.generate(
                model=self.model_name,
                prompt=prompt,
                options={
                    'temperature': self.temperature,
                    'max_tokens': self.max_tokens
                }
            )
            
            generated_proto = response['response']
            
            # Basic validation and cleanup
            generated_proto = self._clean_proto_definition(generated_proto)
            
            return generated_proto
        except Exception as e:
            print(f"Error generating proto definition: {e}")
            return self._generate_fallback_proto(service_info)
    
    def _clean_proto_definition(self, proto_content: str) -> str:
        """
        Clean and validate the generated proto definition
        """
        # Remove any code blocks or markdown formatting
        proto_content = re.sub(r'```(protobuf)?', '', proto_content)
        
        # Ensure syntax
        if not re.search(r'syntax\s*=\s*"proto3";', proto_content):
            proto_content = 'syntax = "proto3";\n\n' + proto_content
        
        return proto_content
    
    def _generate_fallback_proto(self, service_info: Dict[str, Any]) -> str:
        """
        Generate a basic proto definition if model generation fails
        """
        service_name = self._sanitize_proto_identifier(service_info['serviceName'])
        
        proto_content = f"""syntax = "proto3";

package {service_info.get('namespace', 'wcf').lower()};

message {service_name}Request {{
    // Fallback request message
    string operation_name = 1;
    map<string, string> parameters = 2;
}}

message {service_name}Response {{
    // Fallback response message
    bool success = 1;
    string result = 2;
    string error_message = 3;
}}

service {service_name}Service {{
    rpc GenericOperation({service_name}Request) returns ({service_name}Response) {{}}
}}
"""
        return proto_content
    
    def save_proto_file(self, service_info: Dict[str, Any]) -> str:
        """
        Generate and save proto file for a service
        
        :param service_info: Dictionary containing service details
        :return: Path to the generated .proto file
        """
        # Sanitize service name for filename
        service_name = self._sanitize_proto_identifier(service_info['serviceName'])
        
        # Generate proto content
        proto_content = self.generate_proto_definition(service_info)
        
        # Prepare file path
        output_filename = f"./output/proto/{service_name}.proto"
        
        # Save the proto file
        with open(output_filename, 'w') as f:
            f.write(proto_content)
        
        print(f"Generated proto file: {output_filename}")
        return output_filename
    
    def generate_proto_files(self, services: List[Dict[str, Any]]) -> List[str]:
        """
        Generate proto files for multiple services
        
        :param services: List of service information dictionaries
        :return: List of generated proto file paths
        """
        generated_files = []
        for service in services:
            try:
                proto_file = self.save_proto_file(service)
                generated_files.append(proto_file)
            except Exception as e:
                print(f"Failed to generate proto for service {service.get('serviceName')}: {e}")
        
        return generated_files