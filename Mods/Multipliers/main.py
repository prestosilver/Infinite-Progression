import BigNumber
import SeededRand
import GameController

class Data:
    def __init__(self, id):
        self.id = id
        self.level = 0
        self.nameText = ""
        self.buys = GameController.GetRandOf("slider", 0, id).id - 1
        self.muls = GameController.GetRandOf("slider", 1, id).id - 1
        if (self.muls == self.buys):
            if self.muls == 0:
                self.buys = 1
            else:
                self.buys = 0
        self.discount = 1

    def updateProgress(self):
        self.nameText = GameController.GetSlider(self.buys).textName + "*" + GameController.GetSlider(self.muls).textName 

def onLoad():
    return  "Success Loading"

def onUnload():
    return "Success Unloading"

def createModule(id):
    data = Data(id)
    data.result = "Created InsaneLock"
    return data

def tick(data):
    data.updateProgress()
    return data

def bulkTick(data, amount):
    data.updateProgress()
    return data

def destroyModule(data):
    data.result = "Success Unloading"
    data.updateProgress()
    return data

def upgradeClick(data):
    GameController.GetSlider(data.buys).Buy(500 * data.level * data.discount)
    GameController.GetSlider(data.muls).BuyMuls()
    data.level += 1
    data.updateProgress()
    return data

def betterUpgradeClick(data):
    GameController.GetSlider(data.buys).Buy(2500 * data.level * data.discount)
    GameController.GetSlider(data.muls).BuyMuls()
    data.level += 20
    data.updateProgress()
    return data

def upgradeAvail(data):
    return (500 * data.level * data.discount) < GameController.GetSlider(data.buys).value

def betterUpgradeAvail(data):
    return (2500 * data.level * data.discount) < GameController.GetSlider(data.buys).value

def buyDiscount(data):
    data.discount *= 0.9
    
def onPrestige(data):
    data.discount = 1
    data.level = 0

def loadSave(save, id):
    data = createModule(id)
    saveData = save.split(",")
    data.level = int(saveData[0])
    data.buys = int(saveData[1])
    data.muls = int(saveData[2])
    return data

def saveData(data):
    result = ""
    result += str(data.level) + ","
    result += str(data.buys) + ","
    result += str(data.muls)
    return result