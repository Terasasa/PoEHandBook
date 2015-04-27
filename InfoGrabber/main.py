__author__ = 'Tyrrrz'

import codecs
import urllib.request
from bs4 import BeautifulSoup

from objects.armor import Armor
from objects.shield import Shield

headers = {"User-Agent": "Mozilla/5.0 (Windows NT 6.3; rv:36.0) Gecko/20100101 Firefox/36.0"}


#region Parsers
def parse_armors(rarity, cache_images=True):
    itemtypes = {
        "Body Armour": "http://pathofexile.gamepedia.com/List_of_unique_body_armours",
        "Helmet": "http://pathofexile.gamepedia.com/List_of_unique_helmets",
        "Gloves": "http://pathofexile.gamepedia.com/List_of_unique_gloves",
        "Boots": "http://pathofexile.gamepedia.com/List_of_unique_boots"
    }

    result = []

    for itemtype in itemtypes.keys():
        req = urllib.request.Request(itemtypes[itemtype], headers=headers)
        with urllib.request.urlopen(req) as response:
            soup = BeautifulSoup(response.read())

        rows = soup.select("table.sortable tr")
        for row in rows:
            if len(row.select("th")) > 0:
                continue
            cols = row.select("td")
            armor = Armor()
            armor.name = cols[0].select("a")[0].get_text()
            armor.type = itemtype
            armor.imageUri = cols[0].select("img")[0].attrs["src"]
            armor.rarity = rarity
            armor.base = cols[1].select("a")[0].get_text()
            armor.lvl = cols[2].get_text()
            armor.str = cols[3].get_text()
            armor.dex = cols[4].get_text()
            armor.int = cols[5].get_text()
            armor.armourValue = cols[6].get_text()
            armor.evasionValue = cols[7].get_text()
            armor.energyShieldValue = cols[8].get_text()
            armor.mods = cols[9].get_text()
            result.append(armor)
            if cache_images:
                armor.cache_image()
            print("- parsed armor called " + armor.name)

    return result


def parse_shields(rarity, cache_images=True):
    uri = "http://pathofexile.gamepedia.com/List_of_unique_shields"

    result = []

    req = urllib.request.Request(uri, headers=headers)
    with urllib.request.urlopen(req) as response:
        soup = BeautifulSoup(response.read())

    rows = soup.select("table.sortable tr")
    for row in rows:
        if len(row.select("th")) > 0:
            continue
        cols = row.select("td")
        shield = Shield()
        shield.name = cols[0].select("a")[0].get_text()
        shield.type = "Shield"
        shield.imageUri = cols[0].select("img")[0].attrs["src"]
        shield.rarity = rarity
        shield.base = cols[1].select("a")[0].get_text()
        shield.lvl = cols[2].get_text()
        shield.str = cols[3].get_text()
        shield.dex = cols[4].get_text()
        shield.int = cols[5].get_text()
        shield.block = cols[6].get_text()
        shield.armourValue = cols[7].get_text()
        shield.evasionValue = cols[8].get_text()
        shield.energyShieldValue = cols[9].get_text()
        shield.mods = cols[10].get_text()
        result.append(shield)
        if cache_images:
            shield.cache_image()
        print("- parsed shield called " + shield.name)

    return result
#endregion


# Parse uniques and dump to file
def getuniques():
    inpt = input("Cache images? [Y/N]")
    cache_images = False
    if inpt.upper() == "Y":
        cache_images = True

    print("Parsing unique items")

    print("- armors")
    file = codecs.open("uniques_armors.xml", 'w', encoding="utf-8")
    items = parse_armors("Unique", cache_images)
    file.write('<?xml version="1.0" encoding="utf-8" ?>\n<Root>\n')
    for item in items:
        file.write(item.serialize() + "\n")
    file.write("</Root>")
    file.close()
    print()

    print("- shields")
    file = codecs.open("uniques_shields.xml", 'w', encoding="utf-8")
    items = parse_shields("Unique", cache_images)
    file.write('<?xml version="1.0" encoding="utf-8" ?>\n<Root>\n')
    for item in items:
        file.write(item.serialize() + "\n")
    file.write("</Root>")
    file.close()
    print()


# Entry point
getuniques()