import sys
import os
import codecs
import re
import urllib.request
from bs4 import BeautifulSoup

def clean_off_unicode(str):
    str = urllib.request.unquote(str)
    return str \
        .replace(u'ö', 'o') \
        .replace(u'ü', 'u') \
        .replace(u'ß', 'b')


def get_page(uri):
    # PoE wiki forbids access if user agent does not belong to one of the popular browsers
    # .. so we're Google Chrome now
    headers = {
        "User-Agent": "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.90 Safari/537.36",
        "Accept": "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8"}
    uri = clean_off_unicode(uri)
    req = urllib.request.Request(uri, headers=headers)
    return urllib.request.urlopen(req)


def download_file(uri, directory, name):
    if os.path.exists(os.path.join(directory, name)):
        return
    if not os.path.exists(directory):
        os.makedirs(directory)
    urllib.request.urlretrieve(uri, os.path.join(directory, name))


def create_property_xml(name, value, indent=0):
    name = str(name).strip()
    value = str(value).strip().replace("N/A", "0")
    return '  '*indent + '<Property id="' + name + '">' + value + '</Property>\n'


def create_delim_xml(name, indent=0):
    name = str(name).strip()
    return '  '*indent + '<' + name + ' />\n'


def dump_xml(nodes, file_name):
    file = codecs.open(file_name, 'w', encoding="utf-8")
    file.write('<?xml version="1.0" encoding="utf-8" ?>\n<Root>\n')
    file.writelines(nodes)
    file.write("</Root>")
    file.close()


def format_item_mods(mods):
    mods = re.sub(r"([a-z])([\+\-\(\dA-Z])", r"\1 | \2", mods)
    return mods.replace("N/A", "")\
            .replace("<", "[")\
            .replace(">", "]")\
            .replace("  ", " | ")


# region Parsers
def parse_currency(uris, download_images=True):
    result = []

    for uri in uris:
        soup = BeautifulSoup(get_page(uri).read())

        rows = soup.select("table.sortable tr")
        for row in rows:
            if len(row.select("th")) > 0:
                continue

            cols = row.select("td")

            result.append("<Entity>\n")
            name = cols[0].get_text().strip()
            result.append(create_property_xml("Name", name, 1))

            uri_detailed = "http://pathofexile.gamepedia.com" + cols[0].select("a")[0].attrs["href"]
            soup = BeautifulSoup(get_page(uri_detailed).read())
            result.append(create_property_xml("Description", soup.select("span.itemboxstatsgroup")[1].get_text(), 1))
            result.append("</Entity>\n")

            if download_images:
                try:
                    imageUri = soup.select("div.itemboximage")[0].select("img")[0].attrs["src"]
                    download_file(imageUri, "cache", name + ".png")
                except:
                    print("Could not download image")

            print("- parsed currency called " + clean_off_unicode(name))

    return result


def parse_maps(uris, rarity, download_images=True):
    result = []

    for uri in uris:
        soup = BeautifulSoup(get_page(uri).read())

        rows = soup.select("table.sortable tr")
        for row in rows:
            if len(row.select("th")) > 0:
                continue

            cols = row.select("td")

            result.append("<Entity>\n")
            name = cols[0].select("a")[0].get_text().strip()
            result.append(create_property_xml("Name", name, 1))
            result.append(create_property_xml("Rarity", rarity, 1))
            result.append(create_property_xml("Base", cols[1].select("a")[0].get_text(), 1))
            result.append(create_property_xml("Level", cols[2].get_text(), 1))
            result.append(create_property_xml("Quantity", cols[3].get_text(), 1))
            result.append(create_property_xml("Mods", format_item_mods(cols[4].get_text()), 1))
            if len(cols[4].select("span.itemboxstatsgroup")) > 1:
                result.append(create_delim_xml("HasImplicitMod", 1))
            result.append("</Entity>\n")

            uri_detailed = "http://pathofexile.gamepedia.com" + cols[0].select("a")[0].attrs["href"]
            soup = BeautifulSoup(get_page(uri_detailed).read())

            if download_images:
                try:
                    imageUri = soup.select("div.itemboximage")[0].select("img")[0].attrs["src"]
                    download_file(imageUri, "cache", name + ".png")
                except:
                    print("Could not download image")

            print("- parsed map called " + clean_off_unicode(name))

    return result


def parse_jewels(uris, rarity, download_images=True):
    result = []

    for uri in uris:
        soup = BeautifulSoup(get_page(uri).read())

        rows = soup.select("table.sortable tr")
        for row in rows:
            if len(row.select("th")) > 0:
                continue

            cols = row.select("td")

            result.append("<Entity>\n")
            name = cols[0].select("a")[0].get_text().strip()
            result.append(create_property_xml("Name", name, 1))
            result.append(create_property_xml("Rarity", rarity, 1))
            result.append(create_property_xml("Base", cols[1].select("a")[0].get_text(), 1))
            result.append(create_property_xml("Limit", cols[2].get_text(), 1))
            result.append(create_property_xml("Radius", cols[3].get_text(), 1))
            result.append(create_property_xml("Mods", format_item_mods(cols[4].get_text()), 1))
            if len(cols[4].select("span.itemboxstatsgroup")) > 1:
                result.append(create_delim_xml("HasImplicitMod", 1))
            result.append("</Entity>\n")

            uri_detailed = "http://pathofexile.gamepedia.com" + cols[0].select("a")[0].attrs["href"]
            soup = BeautifulSoup(get_page(uri_detailed).read())

            if download_images:
                try:
                    imageUri = soup.select("div.itemboximage")[0].select("img")[0].attrs["src"]
                    download_file(imageUri, "cache", name + ".png")
                except:
                    print("Could not download image")

            print("- parsed jewel called " + clean_off_unicode(name))

    return result


def parse_armors(uris, type, rarity, download_images=True):
    result = []

    for uri in uris:
        soup = BeautifulSoup(get_page(uri).read())

        rows = soup.select("table.sortable tr")
        for row in rows:
            if len(row.select("th")) > 0:
                continue

            cols = row.select("td")

            result.append("<Entity>\n")
            name = cols[0].select("a")[0].get_text().strip()
            result.append(create_property_xml("Name", name, 1))
            result.append(create_property_xml("Type", type, 1))
            result.append(create_property_xml("Rarity", rarity, 1))
            result.append(create_property_xml("Base", cols[1].select("a")[0].get_text(), 1))
            result.append(create_property_xml("Level", cols[2].get_text(), 1))
            result.append(create_property_xml("Strength", cols[3].get_text(), 1))
            result.append(create_property_xml("Dexterity", cols[4].get_text(), 1))
            result.append(create_property_xml("Intelligence", cols[5].get_text(), 1))
            result.append(create_property_xml("ArmourValue", cols[6].get_text(), 1))
            result.append(create_property_xml("EvasionValue", cols[7].get_text(), 1))
            result.append(create_property_xml("EnergyShieldValue", cols[8].get_text(), 1))
            result.append(create_property_xml("Mods", format_item_mods(cols[9].get_text()), 1))
            if len(cols[9].select("span.itemboxstatsgroup")) > 1:
                result.append(create_delim_xml("HasImplicitMod", 1))
            result.append("</Entity>\n")

            uri_detailed = "http://pathofexile.gamepedia.com" + cols[0].select("a")[0].attrs["href"]
            soup = BeautifulSoup(get_page(uri_detailed).read())

            if download_images:
                try:
                    imageUri = soup.select("div.itemboximage")[0].select("img")[0].attrs["src"]
                    download_file(imageUri, "cache", name + ".png")
                except:
                    print("Could not download image")

            print("- parsed " + type + " called " + clean_off_unicode(name))

    return result


def parse_trinkets(uris, type, rarity, download_images=True):
    result = []

    for uri in uris:
        soup = BeautifulSoup(get_page(uri).read())

        rows = soup.select("table.sortable tr")
        for row in rows:
            if len(row.select("th")) > 0:
                continue

            cols = row.select("td")

            result.append("<Entity>\n")
            name = cols[0].select("a")[0].get_text().strip()
            result.append(create_property_xml("Name", name, 1))
            result.append(create_property_xml("Type", type, 1))
            result.append(create_property_xml("Rarity", rarity, 1))
            result.append(create_property_xml("Base", cols[1].select("a")[0].get_text(), 1))
            result.append(create_property_xml("Level", cols[2].get_text(), 1))
            result.append(create_property_xml("Mods", format_item_mods(cols[3].get_text()), 1))
            if len(cols[3].select("span.itemboxstatsgroup")) > 1:
                result.append(create_delim_xml("HasImplicitMod", 1))
            result.append("</Entity>\n")

            uri_detailed = "http://pathofexile.gamepedia.com" + cols[0].select("a")[0].attrs["href"]
            soup = BeautifulSoup(get_page(uri_detailed).read())

            if download_images:
                try:
                    imageUri = soup.select("div.itemboximage")[0].select("img")[0].attrs["src"]
                    download_file(imageUri, "cache", name + ".png")
                except:
                    print("Could not download image")

            print("- parsed " + type + " called " + clean_off_unicode(name))

    return result


def parse_shields(uris, rarity, download_images=True):
    result = []

    for uri in uris:
        soup = BeautifulSoup(get_page(uri).read())

        rows = soup.select("table.sortable tr")
        for row in rows:
            if len(row.select("th")) > 0:
                continue

            cols = row.select("td")

            result.append("<Entity>\n")
            name = cols[0].select("a")[0].get_text().strip()
            result.append(create_property_xml("Name", name, 1))
            result.append(create_property_xml("Rarity", rarity, 1))
            result.append(create_property_xml("Base", cols[1].select("a")[0].get_text(), 1))
            result.append(create_property_xml("Level", cols[2].get_text(), 1))
            result.append(create_property_xml("Strength", cols[3].get_text(), 1))
            result.append(create_property_xml("Dexterity", cols[4].get_text(), 1))
            result.append(create_property_xml("Intelligence", cols[5].get_text(), 1))
            result.append(create_property_xml("Block", cols[6].get_text(), 1))
            result.append(create_property_xml("ArmourValue", cols[7].get_text(), 1))
            result.append(create_property_xml("EvasionValue", cols[8].get_text(), 1))
            result.append(create_property_xml("EnergyShieldValue", cols[9].get_text(), 1))
            result.append(create_property_xml("Mods", format_item_mods(cols[10].get_text()), 1))
            if len(cols[10].select("span.itemboxstatsgroup")) > 1:
                result.append(create_delim_xml("HasImplicitMod", 1))
            result.append("</Entity>\n")

            uri_detailed = "http://pathofexile.gamepedia.com" + cols[0].select("a")[0].attrs["href"]
            soup = BeautifulSoup(get_page(uri_detailed).read())

            if download_images:
                try:
                    imageUri = soup.select("div.itemboximage")[0].select("img")[0].attrs["src"]
                    download_file(imageUri, "cache", name + ".png")
                except:
                    print("Could not download image")

            print("- parsed shield called " + clean_off_unicode(name))

    return result
# endregion


# -=-=-=-=-=-=-=-=-=-=-=- ENTRY POINT -=-=-=-=-=-=-=-=-=-=-=- #

# -- All URIs
uris_currency = ["http://pathofexile.gamepedia.com/Currency"]
uris_unique_maps = ["http://pathofexile.gamepedia.com/List_of_unique_maps"]
uris_unique_jewels = ["http://pathofexile.gamepedia.com/List_of_unique_jewels"]

uris_unique_bodyarmour = ["http://pathofexile.gamepedia.com/List_of_unique_body_armours"]
uris_unique_helmet = ["http://pathofexile.gamepedia.com/List_of_unique_helmets"]
uris_unique_gloves = ["http://pathofexile.gamepedia.com/List_of_unique_gloves"]
uris_unique_boots = ["http://pathofexile.gamepedia.com/List_of_unique_boots"]

uris_unique_shields = ["http://pathofexile.gamepedia.com/List_of_unique_shields"]

uris_unique_amulets = ["http://pathofexile.gamepedia.com/List_of_unique_amulets"]
uris_unique_belts = ["http://pathofexile.gamepedia.com/List_of_unique_belts"]
uris_unique_rings = ["http://pathofexile.gamepedia.com/List_of_unique_rings"]
uris_unique_quivers = ["http://pathofexile.gamepedia.com/List_of_unique_quivers"]

# -- Get settings
inpt = input("Download images? [Y/N] ")
need_download_images = inpt.upper() == "Y"
print()

# -- Start parsing
if not os.path.exists("currency.xml"):
    print("#Currency")
    nodes = parse_currency(uris_currency, need_download_images)
    dump_xml(nodes, "currency.xml")
    print()
if not os.path.exists("maps.xml"):
    print("#Unique maps")
    nodes = parse_maps(uris_unique_maps, "Unique", need_download_images)
    dump_xml(nodes, "maps.xml")
    print()
if not os.path.exists("jewels.xml"):
    print("#Unique jewels")
    nodes = parse_jewels(uris_unique_jewels, "Unique", need_download_images)
    dump_xml(nodes, "jewels.xml")
    print()

if not os.path.exists("unique_body_armours.xml"):
    print("#Unique Body Armours")
    nodes = parse_armors(uris_unique_bodyarmour, "Body Armour", "Unique", need_download_images)
    dump_xml(nodes, "unique_body_armours.xml")
    print()
if not os.path.exists("unique_helmets.xml"):
    print("#Unique Helmets")
    nodes = parse_armors(uris_unique_helmet, "Helmet", "Unique", need_download_images)
    dump_xml(nodes, "unique_helmets.xml")
    print()
if not os.path.exists("unique_gloves.xml"):
    print("#Unique Gloves")
    nodes = parse_armors(uris_unique_gloves, "Gloves", "Unique", need_download_images)
    dump_xml(nodes, "unique_gloves.xml")
    print()
if not os.path.exists("unique_boots.xml"):
    print("#Unique Boots")
    nodes = parse_armors(uris_unique_boots, "Boots", "Unique", need_download_images)
    dump_xml(nodes, "unique_boots.xml")
    print()

if not os.path.exists("unique_amulets.xml"):
    print("#Unique Amulets")
    nodes = parse_trinkets(uris_unique_amulets, "Amulet", "Unique", need_download_images)
    dump_xml(nodes, "unique_amulets.xml")
    print()
if not os.path.exists("unique_belts.xml"):
    print("#Unique Belts")
    nodes = parse_trinkets(uris_unique_belts, "Belt", "Unique", need_download_images)
    dump_xml(nodes, "unique_belts.xml")
    print()
if not os.path.exists("unique_rings.xml"):
    print("#Unique Rings")
    nodes = parse_trinkets(uris_unique_rings, "Ring", "Unique", need_download_images)
    dump_xml(nodes, "unique_rings.xml")
    print()
if not os.path.exists("unique_quivers.xml"):
    print("#Unique Quivers")
    nodes = parse_trinkets(uris_unique_quivers, "Quiver", "Unique", need_download_images)
    dump_xml(nodes, "unique_quivers.xml")
    print()

if not os.path.exists("unique_shields.xml"):
    print("#Unique Shields")
    nodes = parse_shields(uris_unique_shields, "Unique", need_download_images)
    dump_xml(nodes, "unique_shields.xml")
    print()