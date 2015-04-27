import os

__author__ = 'Tyrrrz'


class Entity:
    name = ""
    text = ""
    imageUri = ""

    def cache_image(self):
        import urllib.request
        if not os.path.exists("cache"):
            os.makedirs("cache")
        urllib.request.urlretrieve(self.imageUri, "cache\\" + self.name + ".png")

    def serialize(self, include=""):
        output = '<Entity>\n'

        output += '  <Property id="Name">' + self.name + '</Property>\n'
        output += '  <Property id="Text">' + self.text + '</Property>\n'

        output += include

        output += '</Entity>'

        return output