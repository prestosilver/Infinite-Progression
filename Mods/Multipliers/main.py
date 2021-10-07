import clr
clr.AddReference('IP.Lib')
clr.AddReference('IP.Game')

import BigNumber
import SeededRand
import GameController

class Data:
    def __init__(self):
        self.level = 0
        self.nameText = "/M"
        GameController.GetRandOf("slider")

    def updateProgress(self):
        self.discountText = str(self.amount.mantissa)

def onLoad():
    return  "Success Loading"

def onUnload():
    return "Success Unloading"

def createModule():
    data = Data()
    data.result = "Created InsaneLock"
    data.updateProgress()
    return data

def tick(data):
    data.amount += 1
    data.updateProgress()
    return data

def bulkTick(data, amount):
    data.amount += amount
    data.updateProgress()
    return data

def destroyModule(data):
    data.result = "Success Unloading"
    data.updateProgress()
    return data

def resetClick(data):
    data.amount = 0
    data.updateProgress()
    return data
