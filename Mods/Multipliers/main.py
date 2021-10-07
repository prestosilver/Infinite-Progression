import clr
clr.AddReference('IP.Lib')
clr.AddReference('IP.Game')

import BigNumber
import SeededRand
import GameController

class Data:
    def __init__(self, id):
        self.id = id
        self.level = 0
        self.nameText = ""
        self.buys = GameController.GetRandOf("slider", 0, id)
        self.muls = GameController.GetRandOf("slider", 1, id)
        self.discount = 1

    def updateProgress(self):
        self.nameText = self.buys.textName + "*" + self.muls.textName

def onLoad():
    return  "Success Loading"

def onUnload():
    return "Success Unloading"

def createModule(id):
    data = Data(id)
    data.result = "Created InsaneLock"
    data.updateProgress()
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
    GameController.GetSlider(data.buys.id).Buy(500 * data.level * data.discount)
    GameController.GetSlider(data.muls.id).BuyMuls()
    data.level += 1
    data.updateProgress()
    return data

def upgradeAvail(data):
    return (500 * data.level * data.discount) < GameController.GetSlider(data.buys.id).value

def betterUpgradeAvail(data):
    return False

def buyDiscount(data):
    data.discount *= 0.9
    
def prestige(data):
    data.level = 0
    data.discount = 1