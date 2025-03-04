# src/VectorStorage/chroma_repository.py
import os
import json
import chromadb
from chromadb.utils import embedding_functions
from typing import List, Dict, Any

class ChromaRepository:
    def __init__(self, config_path: str):
        with open(config_path, 'r') as f:
            self.config = json.load(f)
        
        self.persistence_dir = self.config.get("persistenceDirectory", "./chroma_db")
        self.collection_name = self.config.get("collectionName", "wcf_services")
        
        # Initialize ChromaDB client
        self.client = chromadb.PersistentClient(path=self.persistence_dir)
        
        # Use sentence-transformers for embeddings
        self.embedding_function = embedding_functions.SentenceTransformerEmbeddingFunction(
            model_name="all-MiniLM-L6-v2"
        )
        
        # Get or create the collection
        try:
            self.collection = self.client.get_collection(
                name=self.collection_name,
                embedding_function=self.embedding_function
            )
            print(f"Connected to existing collection: {self.collection_name}")
        except:
            self.collection = self.client.create_collection(
                name=self.collection_name,
                embedding_function=self.embedding_function
            )
            print(f"Created new collection: {self.collection_name}")
    
    def store_service_info(self, services: List[Dict[str, Any]]) -> None:
        """Store WCF service information in the vector database"""
        
        docs = []
        metadatas = []
        ids = []
        
        for idx, service in enumerate(services):
            # Create a textual representation of the service
            service_text = f"Service: {service['serviceName']}\n"
            service_text += f"Namespace: {service['namespace']}\n"
            service_text += "Operations:\n"
            
            for op in service['operations']:
                params = ", ".join([f"{p['type']} {p['name']}" for p in op['parameters']])
                service_text += f"  - {op['returnType']} {op['name']}({params})\n"
            
            # Create metadata
            metadata = {
                "service_name": service['serviceName'],
                "namespace": service['namespace'],
                "file_path": service['filePath'],
                "operation_count": len(service['operations'])
            }
            
            doc_id = f"service_{service['serviceName']}_{idx}"
            
            docs.append(service_text)
            metadatas.append(metadata)
            ids.append(doc_id)
            
            # Also store individual operations for more granular retrieval
            for op_idx, op in enumerate(service['operations']):
                op_text = f"Operation: {op['name']}\n"
                op_text += f"Service: {service['serviceName']}\n"
                op_text += f"Return Type: {op['returnType']}\n"
                op_text += f"Parameters:\n"
                
                for p in op['parameters']:
                    op_text += f"  - {p['type']} {p['name']}\n"
                
                op_metadata = {
                    "service_name": service['serviceName'],
                    "operation_name": op['name'],
                    "return_type": op['returnType'],
                    "parameter_count": len(op['parameters']),
                    "parent_id": doc_id
                }
                
                docs.append(op_text)
                metadatas.append(op_metadata)
                ids.append(f"operation_{service['serviceName']}_{op['name']}_{op_idx}")
        
        # Add documents to collection
        self.collection.add(
            documents=docs,
            metadatas=metadatas,
            ids=ids
        )
        
        print(f"Stored {len(services)} services with {sum(len(s['operations']) for s in services)} operations")
    
    def query_similar_services(self, query_text: str, n_results: int = 5) -> List[Dict[str, Any]]:
        """Query for services similar to the input text"""
        results = self.collection.query(
            query_texts=[query_text],
            n_results=n_results,
            where={"$contains": "service_name"}  # Only return service documents
        )
        
        return results
    
    def get_all_services(self) -> List[Dict[str, Any]]:
        """Retrieve all stored services"""
        results = self.collection.get(
            where={"$contains": "service_name"}
        )
        
        return {
            "ids": results["ids"],
            "documents": results["documents"],
            "metadatas": results["metadatas"]
        }