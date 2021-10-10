import BigNumber
import SeededRand
import GameController

class Data:
    def __init__(self, id):
        self.id = id
        self.buys = GameController.GetRandOf("slider", 0, id)
        self.discounts = GameController.GetRandOf("Multiplier Modules", 0, id)

    def updateProgress(self):
        self.nameText = GameController.GetSlider(self.buys).textName
        self.nameText += "/M" + self.discounts

def onLoad():
    return  "Success Loading"

def onUnload():
    return "Success Unloading"

def createModule(id):
    data = Data(id)
    data.result = "Created Template"
    return data

def tick(data):
    return data

def bulkTick(data, amount):
    return data

def destroyModule(data):
    return data

def onPrestige(data):
    return data

def loadSave(save, id):
    data = createModule(id)
    return data

def saveData(data):
    result = ""
    return result

"""
end special functions
"""

def upgradeClick(data):
    
    return data