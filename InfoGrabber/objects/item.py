import os
import re
from objects.entity import Entity

__author__ = 'Tyrrrz'


class Item(Entity):
    type = ""
    rarity = ""
    base = ""

    lvl = ""
    str = ""
    dex = ""
    int = ""

    mods = ""

    def cache_image(self):
        import urllib.request
        if not os.path.exists("cache"):
            os.makedirs("cache")
        urllib.request.urlretrieve(self.imageUri, "cache\\" + self.name + ".png")

    def serialize(self, include=""):
        output = ''

        output += '  <Property id="Type">' + self.type + '</Property>\n'
        output += '  <Property id="Rarity">' + self.rarity + '</Property>\n'
        output += '  <Property id="Base">' + self.base + '</Property>\n'

        output += '  <Property id="Level">' + self.lvl.replace("N/A", "0") + '</Property>\n'
        output += '  <Property id="Strength">' + self.str.replace("N/A", "0") + '</Property>\n'
        output += '  <Property id="Dexterity">' + self.dex.replace("N/A", "0") + '</Property>\n'
        output += '  <Property id="Intelligence">' + self.int.replace("N/A", "0") + '</Property>\n'

        mods_str = '  <Property id="Mods">' + self.mods\
            .replace("N/A", "")\
            .replace("<", "[")\
            .replace(">", "]")\
            .replace("  ", " | ") + '</Property>\n'

        mods_str = re.sub(r"([a-z])([\+\-\(\dA-Z])", r"\1 | \2", mods_str)
        output += mods_str

        output += include

        return Entity.serialize(self, output)