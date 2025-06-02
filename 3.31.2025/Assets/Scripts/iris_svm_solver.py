import numpy as np
from sklearn import svm
from sklearn.preprocessing import StandardScaler
import pandas as pd
import json
import sys

class IrisSVMSolver:
    def __init__(self):
        self.svm_model = None
        self.scaler = StandardScaler()
        self.features = ['sepal_length', 'sepal_width', 'petal_length', 'petal_width']
        
    def load_data(self, csv_path):
        """Load and preprocess the Iris dataset."""
        df = pd.read_csv(csv_path)  # Load the dataset from a CSV file
        df.columns = ['id', 'sepal_length', 'sepal_width', 'petal_length', 'petal_width', 'species']  # Rename the columns
        df['species'] = df['species'].str.replace('Iris-', '')  # Remove 'Iris-' prefix from species names
        return df
        
    def train_svm(self, df, selected_features, kernel='linear'):
        """Train SVM model on selected features."""
        X = df[selected_features]  # Select the features for training
        y = df['species']  # Select the target variable
        
        X_scaled = self.scaler.fit_transform(X)  # Scale the features
        self.svm_model = svm.SVC(kernel=kernel, C=1.0)  # Create an SVM model
        self.svm_model.fit(X_scaled, y)  # Train the SVM model
        return self.svm_model
    
    def get_separation_line(self, feature_indices):
        """Calculate separation line parameters including margin for 2D visualization."""
        if self.svm_model is None or len(feature_indices) != 2:
            return None
            
        if hasattr(self.svm_model, 'coef_'):
            # Get the normal vector
            w = self.svm_model.coef_[0]
            b = self.svm_model.intercept_[0]
            
            # Normalize the normal vector
            w_norm = np.linalg.norm(w)
            if w_norm != 0:
                w = w / w_norm
                
            # Get support vectors
            support_vectors = self.svm_model.support_vectors_
            
            # Calculate margin width
            margin = 2 / np.sqrt(np.sum(self.svm_model.coef_[0] ** 2))
            
            # Calculate center point between support vectors
            center = np.mean(support_vectors, axis=0)
            
            # Create the response dictionary
            response = {
                'normal': w.tolist(),
                'point': center.tolist(),
                'margin': float(margin),
                'bias': float(b)
            }
            
            # Convert to JSON and print
            print(json.dumps(response))
            return response
            
        return None

def main():
    if len(sys.argv) < 3:
        print(json.dumps({'error': 'Not enough features selected'}))
        return
        
    feature_indices = [int(sys.argv[1]), int(sys.argv[2])]  # Get the feature indices from command line arguments
    solver = IrisSVMSolver()  # Create an instance of the IrisSVMSolver class
    
    try:
        df = solver.load_data('Assets/iris.csv')  # Load and preprocess the Iris dataset
        selected_features = [solver.features[i] for i in feature_indices]  # Get the selected feature names
        solver.train_svm(df, selected_features)  # Train the SVM model
        solver.get_separation_line(feature_indices)  # Calculate separation line parameters
    except Exception as e:
        print(json.dumps({'error': str(e)}))

if __name__ == '__main__':
    main()
