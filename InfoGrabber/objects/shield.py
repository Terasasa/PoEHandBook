from objects.armor import Armor

__author__ = 'Tyrrrz'


class Shield(Armor):
    block = ""

    def serialize(self, include=""):
        return Armor.serialize(self, '  <Property id="Block">' + self.block + '</Property>\n')
