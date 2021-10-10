import BigNumber
import SeededRand
import GameController

class Data:
    """
    This is a class for storing data, this is required

    Attributes:
        id (int): the id of the module
    """
    def __init__(self, id):
        """
        The constructor for the Data class
        Parameters:
            id (int): the id
        """
        self.id = id

def onLoad():
    """
    loads the mod, only ran when the game is started
    """
    return  "Success Loading"

def onUnload():
    """
    unloads the mod, only ran when the home button is pressed
    """
    return "Success Unloading"

def createModule(id):
    """
    creates a module

    Parameters:
        id (int): the id

    Returns:
        data: the new data variable
    """
    data = Data(id)
    data.result = "Created Template"
    return data

def tick(data):
    """
    processes one tick

    Parameters:
        data (Data): the data variable

    Returns:
        data: the new data variable
    """
    return data

def bulkTick(data, amount):
    """
    processes multiple ticks

    Parameters:
        data (Data): the data variable
        amount (BigNumber): the amount of ticks to run

    Returns:
        data: the new data variable
    """
    return data

def destroyModule(data):
    """
    destroys the module currently not run

    Parameters:
        data (Data): the data variable

    Returns:
        data: the new data variable
    """
    return data

def onPrestige(data):
    """
    processes prestige

    Parameters:
        data (Data): the data variable

    Returns:
        data: the new data variable
    """
    return data

def loadSave(save, id):
    """
    loads a save

    Parameters:
        save (string): the save Data
        id (int): the id of the module

    Returns:
        data: the new data variable
    """
    data = createModule(id)
    return data

def saveData(data):
    """
    gets save data

    Parameters:
        data (Data): the data variable

    Returns:
        string: the save Data
    """
    result = ""
    return result