import BigNumber
import SeededRand
import GameController

class Data:
    def __init__(self, id):
        self.id = id
        self.level = 0
        self.buys = GameController.GetRandOf("slider", 0, id).id - 1
        self.discounts = GameController.GetRandOf("Multiplier Modules", 0, id).id - 1

    def updateProgress(self):
        self.discountText = str(100 * GameController.GetData(self.discounts).discount) + "%"
        self.nameText = GameController.GetSlider(self.buys).textName
        self.nameText += "/M" + str(self.discounts)

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
    data.updateProgress()
    return data

def destroyModule(data):
    return data

def onPrestige(data):
    return data

def loadSave(save, id):
    data = createModule(id)
    data.buys = int(save.split(",")[0])
    data.discounts = int(save.split(",")[1])
    data.level = int(save.split(",")[2])
    return data

def saveData(data):
    result = ""
    result += str(data.buys) + ","
    result += str(data.discounts) + ","
    result += str(data.level)
    return result

"""
end special functions
"""

def upgradeClick(data):
    GameController.GetSlider(data.buys).value -= 100000 * data.level
    GameController.GetData(data.discounts).discount *= 0.9
    data.level += 1
    return data

def upgradeAvail(data):
    return 100000 * data.level < GameController.GetSlider(data.buys).value