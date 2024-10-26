import math
import random
import pandas as pd


df = pd.read_csv("data.csv")
#print(df.loc[1][0])


def relu(x):
  if x<0:
    return 0
  return x
def sigmoid(x):
  return 1/(1+(math.e**-x))
def linear(x):
  return x
def dRelu(x):
  if x<0:
    return 0
  return 1
def dSigmoid(x):
  return (math.e**-x)/((1+math.e**-x)**2)
def dlinear(x):
  return 1
  
activationFunctions={"sig": sigmoid,"relu":relu, "linear": linear}
dActivationFunctions={"sig": dSigmoid,"relu":dRelu, "linear": dlinear}


class inputNueron:
  def __init__(self):
    self.inputValue=0
    self.neighbors=[]
  
class Neuron:
  def __init__(self,activation):
    self.inputs=[]
    self.weights=[]
    self.dweights=[]
    self.bias=random.random()
    self.activation=activation
    self.neighbors=[]
    self.backNeighbors=[]
    self.dA=[]
  def initializeWeight(self):
    self.weights.append(random.random())
  def forward(self):
    sum=0
    for i in range(len(self.weights)):
    
      sum+=self.weights[i]*self.inputs[i]+self.bias
    return activationFunctions[self.activation](sum)
  def dforward(self):
    sum=0
    for i in range(len(self.weights)):
      
      sum+=self.weights[i]*self.inputs[i]+self.bias
    return dActivationFunctions[self.activation](sum)


 
class graph:
  def __init__(self):
    self.train=[]
    self.test=[]
    self.network=[]
    self.out=[]
    self.learning_rate=0
    self.target=[]
    self.loss=0
    self.dloss=[0]
  def makeNet(self):
    f = open("in.txt", "r")
    hi= f.readlines()
    self.learning_rate=-1*float(hi[len(hi)-1].split()[1])
    
    for i in range(0,len(hi)-1):
      li=[]
      for j in range(int(hi[i].split()[0])):
        if i==0:
          
          li.append(inputNueron())
        else:
          #print(self.network)
          z=Neuron(hi[i].split()[1])
          for k in range(len(self.network[i-1])):
            z.initializeWeight()
            z.inputs.append(0)
            
            
          
          li.append(z)
          # for k in self.network[len(self.network)-1]:
          #   k.neighbors.append(z)
          #   z.backNeighbors.append(k)
            
          
      
      self.network.append(li)
    
  def sortData(self):
    rows=[]
    self.train=[]
    self.test=[]
    for i in range(len(df)):
      rows.append(df.loc[i])
  
    count=0
    f = open("in.txt", "r")
    hi= f.readlines()
    
    while count< int(float(hi[len(hi)-1].split()[2])*len(df)):
      randomIndex=random.randint(0,len(rows)-1)
      self.train.append(rows.pop(randomIndex))
      count+=1

    
    self.test=rows
  


  
  def setInputs(self,index):
    self.target=self.train[index][len(self.network[0]):]
    
    for i in range(len(self.train[index][0:len(self.network[0])])):
      self.network[0][i].inputValue=self.train[index][i]

     
  def Forward(self):
    for i in range(len(self.network)):
      if(i==0):
        for k in self.network[1]:
          for j in range(len(self.network[0])):
           
          
          
            k.inputs[j]=self.network[0][j].inputValue
        continue
       
      if i==len(self.network)-1:
        self.out=[]
        for j in range(len(self.network[i])):
          #print(self.network[i][j].forward())
          self.out.append(self.network[i][j].forward())    
          
        continue
      for j in range(len(self.network[i])):
        a=self.network[i][j].forward()
        for k in self.network[i+1]:
          k.inputs[j]=a
  # def loss(self,target):
  #   L=[]
  #   for i in range(len(self.train[0])-len(self.network[0])):
  #     L.append((self.out[i]-self.target[i])**2)
  #   return L
  def Loss(self):
    for i in range(len(self.train[0])-len(self.network[0])):
      self.dloss[i]+=(self.out[i]-self.target[i])*2
      self.loss+=(self.out[i]-self.target[i])**2

          
  def backword(self):
    self.loss/=len(self.train)
    for i in range(len(self.network)-1,0,-1):
      for j in range(len(self.network[i])):
        if j ==0:
          for k in range(len(self.network[i-1])):
            self.network[i-1][k].dA=[]
        for k in range(len(self.network[i-1])):
          if i==len(self.network)-1:
            self.network[i-1][k].dA.append(self.network[i][j].dforward()*self.network[i][j].weights[k])
            self.network[i][j].weights[k]+=(self.dloss[0]/len(self.train))*self.network[i][j].dforward()*self.network[i][j].inputs[k]*self.learning_rate
  
          else:
            if i-1!=0:
              self.network[i-1][k].dA.append(self.network[i][j].dforward()*self.network[i][j].weights[k]*self.network[i][j].dA[0])
            self.network[i][j].weights[k]+=(self.dloss[0]/len(self.train))*self.network[i][j].dforward()*self.network[i][j].inputs[k]*self.learning_rate*self.network[i][j].dA[0]
        
        
        


scene=graph()
scene.sortData()


scene.makeNet()

print(scene.network)
for j in range(47):
  for i in range(len(scene.train)):
    scene.setInputs(i)
    
    scene.Forward()

    
    scene.Loss()

  scene.backword()
  print(scene.out)
  print(scene.loss)
  
print(scene.out)


