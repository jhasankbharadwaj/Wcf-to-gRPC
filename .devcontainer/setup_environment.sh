#!/bin/bash

echo "Setting up WCF Analyzer environment..."

# Install .NET SDK
echo "Installing .NET SDK..."
wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --channel 7.0
export PATH="$HOME/.dotnet:$PATH"
echo 'export PATH="$HOME/.dotnet:$PATH"' >> ~/.bashrc

# Install Python dependencies
echo "Installing Python dependencies..."
python -m pip install --upgrade pip
pip install sentence-transformers chromadb protobuf grpcio-tools

# Install Ollama
echo "Installing Ollama..."
curl -fsSL https://ollama.com/install.sh | sh

# Pull DeepSeek 32B model
echo "Downloading DeepSeek model (this will take some time)..."
ollama pull deepseek-coder:33b

# Create project directories
echo "Creating project structure..."
mkdir -p src/{SyntaxAnalyzer,VectorStorage,ProtoGeneration,Common}
mkdir -p scripts config

echo "Environment setup complete!"
