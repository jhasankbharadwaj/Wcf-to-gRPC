import json
import random

# --- Configuration ---
MAX_CONTEXTS = 200
MAX_PATH_LENGTH = 8
MAX_PATH_WIDTH = 2
SEPARATOR = '|'

class AstPathExtractor:
    def __init__(self, ast_json_path, output_path):
        self.ast_json_path = ast_json_path
        self.output_path = output_path
        with open(self.ast_json_path, 'r') as f:
            self.ast = json.load(f)

    def _get_terminals_and_method_name(self, root):
        """
        Traverse the tree to find all terminal nodes and the method name.
        Also builds a parent map for easy traversal up the tree.
        """
        terminals = []
        method_name = "METHOD_NAME_UNKNOWN"
        
        # We use a stack for non-recursive traversal
        stack = [(root, [])] # (node, path_to_node)
        
        # Build parent references for easy LCA finding
        root['parent'] = None
        q = [root]
        while q:
            current = q.pop(0)
            if 'Children' in current:
                for child in current['Children']:
                    child['parent'] = current
                    q.append(child)

        # Find terminals and method name
        q = [root]
        while q:
            node = q.pop(0)
            
            # Find method name
            if node['Kind'] == 'MethodDeclaration':
                for child in node.get('Children', []):
                    if child['Kind'] == 'IdentifierToken':
                        method_name = child['Text']
                        break
            
            # Check for terminal
            if node.get('IsTerminal', False) and node.get('Text'):
                 # We don't want punctuation as terminals
                if node['Kind'] not in ['OpenBraceToken', 'CloseBraceToken', 'SemicolonToken', 
                                         'OpenParenToken', 'CloseParenToken', 'OpenBracketToken', 
                                         'CloseBracketToken']:
                    terminals.append(node)

            if 'Children' in node:
                for child in node['Children']:
                    q.append(child)

        return terminals, method_name

    def _get_path(self, node_a, node_b):
        """Finds the path from node_a up to the LCA and then down to node_b."""
        path_a = []
        curr = node_a
        while curr is not None:
            path_a.append(curr)
            curr = curr.get('parent')

        path_b = []
        curr = node_b
        while curr is not None:
            path_b.append(curr)
            curr = curr.get('parent')

        # Find Lowest Common Ancestor (LCA)
        lca = None
        for a_node in path_a:
            if any(b_node is a_node for b_node in path_b):
                lca = a_node
                break
        
        if not lca:
            return None

        # Path from A up to LCA
        up_path = []
        curr = node_a
        while curr is not lca:
            up_path.append(curr['Kind'])
            curr = curr['parent']
        
        # Path from LCA down to B
        down_path = []
        curr = node_b
        while curr is not lca:
            down_path.insert(0, curr['Kind']) # Prepend to reverse the path
            curr = curr['parent']

        # The full path
        full_path = up_path + [lca['Kind']] + down_path
        
        if len(full_path) > MAX_PATH_LENGTH:
            return None
            
        return SEPARATOR.join(full_path)

    def process(self):
        """Main processing function to generate the c2v file."""
        all_methods_contexts = []
        
        # Assuming the root contains method declarations. 
        # In a real scenario, you'd loop through all MethodDeclaration nodes.
        # For this example, we assume one method in the file.
        terminals, method_name = self._get_terminals_and_method_name(self.ast)
        
        if len(terminals) < 2:
            print("Not enough terminals to create paths.")
            return

        contexts = []
        # Sample pairs of terminals to create paths
        for _ in range(MAX_CONTEXTS):
            t1, t2 = random.sample(terminals, 2)
            
            path_str = self._get_path(t1, t2)
            if path_str:
                context = f"{t1['Text']},{path_str},{t2['Text']}"
                contexts.append(context)

        # The final line format: method_name context1 context2 ...
        if contexts:
            line = f"{method_name} {' '.join(contexts)}"
            all_methods_contexts.append(line)

        with open(self.output_path, 'w') as f:
            for line in all_methods_contexts:
                f.write(line + '\n')
        print(f"code2vec input file created at: {self.output_path}")


if __name__ == '__main__':
    extractor = AstPathExtractor('roslyn_ast.json', 'csharp_code.c2v.txt')
    extractor.process()