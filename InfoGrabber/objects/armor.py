from objects.item import Item

__author__ = 'Tyrrrz'


class Armor(Item):
    armourValue = ""
    evasionValue = ""
    energyShieldValue = ""

    def serialize(self, include=""):
        output = ""

        output += '  <Property id="ArmourValue">' + self.armourValue.replace("N/A", "") + '</Property>\n'
        output += '  <Property id="EvasionValue">' + self.evasionValue.replace("N/A", "") + '</Property>\n'
        output += '  <Property id="EnergyShieldValue">' + self.energyShieldValue.replace("N/A", "") + '</Property>\n'
        output += include

        return Item.serialize(self, output)
