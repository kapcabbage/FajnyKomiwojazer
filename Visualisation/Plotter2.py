import numpy as np
import matplotlib.pyplot as plt
from math import sqrt

class Plotter2:
	def __init__(self):
		self.X = []
		self.Y = []
		self.A = 0.0
		self.B = 0.0
		
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
				self.X = []
				self.Y = []
				


	def plot(self):
		self.X = np.array(self.X)
		self.Y = np.array(self.Y)
		self.Y = self.Y * 100
		self.X = self.X / 1000
		
		fig = plt.figure()
		ax = fig.add_subplot(111)

		plt.plot(self.X, self.Y, 'o', markersize=1.5)
		
		sy = sum(self.Y) / len(self.Y)
		slogx = sum(np.log(self.X)) / len(self.X) 
		SXX = sum(np.square(np.log(self.X) - slogx)) / len(self.X) 
		SYY = sum(np.square(self.Y - sy)) / len(self.Y) 
		SXY = sum(np.multiply(self.Y - sy, np.log(self.X) - slogx)) / len(self.X)
		print("SXX = %f; SYY = %f; SXY = %f;" % (SXX, SYY, SXY))
		B = SXY/SXX
		A = sum(np.log(self.X)) / len(self.X) * B * -1 + sum(self.Y) / len(self.Y)
		r = SXY / (sqrt(SXX) * sqrt(SYY))
		print("A = %f; B = %f; r = %f;" % (A, B, r))
		
		t = np.arange(min(self.X), max(self.X), (max(self.X) - min(self.X))/100)
		plt.plot(t, np.log(t) * B + A , '-')
		
		
		plt.xticks(np.arange(0, 1.05 * max(self.X), 10))
		plt.yticks(np.arange(0, 1.05 * max(self.Y), 10))
		ax.set_ylim(ymin=0)
		ax.set_xlim(xmin=0)
		plt.ylabel('Podobieństwo [%]')
		plt.xlabel('Wynik [tys]')
		'''for tick in ax.get_xticklabels():
			tick.set_rotation(45)'''
			
		plt.show()
		
		
			
			
			