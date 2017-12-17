import numpy as np
import matplotlib.pyplot as plt

class Plotter2:
    def __init__(self):
        self.X = []
        self.Y = []
        
        
    def load(self, filename):
        with open(filename) as f:
            content = f.readlines()
        
        content = [x.strip() for x in content] 
        
        for line in content:
            fields = line.split(" ")
            if len(fields) == 2:
                self.X.append(float(fields[0]))
                self.Y.append(float(fields[1]))
            if len(fields) < 2:
                self.plot()


    def plot(self):
        fig = plt.figure()
        ax = fig.add_subplot(111)

        plt.plot(self.X, self.Y, 'ro')
        
        plt.xticks(np.arange(0, 1.05 * max(self.X), 5000))
        plt.yticks(np.arange(0, 1.05 * max(self.Y), 0.2))
        for tick in ax.get_xticklabels():
            tick.set_rotation(45)
			
        plt.show()
        
        
            
            
            