from Verticle import Verticle
import numpy as np
import matplotlib.pyplot as plt

class Plotter:
    def __init__(self):
        self.Vertices = []
        self.Edges = []
        
        
    def load(self, filename):
        with open(filename) as f:
            content = f.readlines()
        
        
        content = [x.strip() for x in content] 
        
        for line in content:
            fields = line.split(" ")
            if len(fields) == 2:
                self.Edges.append([int(fields[0]), int(fields[1])])
            if len(fields) == 4:
                ver = Verticle()
                ver.Index = int(fields[0])
                ver.Value = int(fields[1])
                ver.X = int(fields[2])
                ver.Y = int(fields[3])
                self.Vertices.append(ver)


    def plot(self, steptime):
        fig = plt.figure()
        ax = fig.add_subplot(111)

        X = list(v.X for v in self.Vertices)
        Y = list(v.Y for v in self.Vertices)
        plt.plot(X, Y, 'ro')
        
        plt.xticks(np.arange(0, max(X)+100, 500))
        plt.yticks(np.arange(0, max(Y)+100, 500))
        for v in self.Vertices:
            if any(160 > v2.X - v.X > 0 and 40 > abs(v2.Y - v.Y) >= 0 for v2 in self.Vertices):
                plt.annotate(v, (v.X - 15, v.Y - 5), horizontalalignment='right')
            else:
                plt.annotate(v, (v.X + 15, v.Y - 5))
                
            
        
        for edge in self.Edges:
            print(edge[1],end=" -> ")
            #print (self.Vertices[edge[1]])
            v1 = self.Vertices[edge[0]]
            v2 = self.Vertices[edge[1]]
            plt.plot([v1.X, v2.X],  [v1.Y, v2.Y], color='lightgray', linestyle='-')
        
        plt.show()
        
        
            
            
            