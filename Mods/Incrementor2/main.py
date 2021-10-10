import BigNumber
import SeededRand
import GameController

class Data:
    def __init__(self, id):
        self.id = id
        self.level = 0
        self.current = BigNumber(0)
        self.goal = 1000.0
        self.progress = BigNumber(0)
        self.adds = GameController.GetRandOf("Incrementor Incrementor", 0, id).id - 1
        if self.adds == 0:
            self.adds = GameController.GetRandOf("Incrementor Module", 0, id).id - 1
        self.inclevel = GameController.GetData(self.adds).inclevel + 1
        self.root = GameController.GetData(self.adds).root

    def updateProgress(self):
        self.progress = self.current / BigNumber(self.goal)
        if (self.goal <= 50.0):
            self.progress = BigNumber(1)
        self.nameText = GameController.GetData(self.adds).nameText + " += " + str(BigNumber.Pow(BigNumber(self.level), 2))

def onLoad():
    return  "Success Loading"

def onUnload():
    return "Success Unloading"

def createModule(id):
    data = Data(id)
    data.result = "Created Template"
    return data

def tick(data):
    data.current += BigNumber(1) 
    data.updateProgress()
    return data

def bulkTick(data, amount):
    data.current += amount
    while (data.current > BigNumber(data.goal)):
        GameController.GetData(data.adds).current += BigNumber.Pow(BigNumber(data.level), 2)
        data.current -= BigNumber(data.goal)
    data.updateProgress()
    return data

def destroyModule(data):
    return data

def onPrestige(data):
    data.goal = 1000.0
    data.current = BigNumber(0)
    return data

def loadSave(save, id):
    data = createModule(id)
    data.level = int(save.split(",")[0])
    data.goal = float(save.split(",")[1])
    data.adds = int(save.split(",")[2])
    return data

def saveData(data):
    result = ""
    result += str(data.level) + ","
    result += str(data.goal) + ","
    result += str(data.adds)
    return result

"""
END special functions
"""

def upgradeClick(data):
    data.goal /= 1.1
    if data.goal <= 1:
        data.goal = 1
    data.level += 1
    return data

def upgradeAvail(data):
    return (10000 * data.level * data.inclevel) < GameController.GetSlider(data.root).value