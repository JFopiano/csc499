import sys
import numpy as np
import sklearn
import pandas as pd

def test_imports():
    print("Python version:", sys.version)
    print("NumPy version:", np.__version__)
    print("scikit-learn version:", sklearn.__version__)
    print("pandas version:", pd.__version__)
    print("All required packages are available!")

if __name__ == "__main__":
    test_imports()