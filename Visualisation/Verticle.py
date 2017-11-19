class Verticle:
    def __init__(self):
        self.Value = 0
        self.X = 0
        self.Y = 0
        self.Index = 0
		
		
    def __str__(self):
        return "%s(%s)" % (self.Index, self.Value)
