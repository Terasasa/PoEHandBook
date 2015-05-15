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

            result.append('<Entity Type="Currency">\n')
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

            result.append('<Entity Type="Map">\n')
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

            result.append('<Entity Type="Jewel">\n')
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


def parse_flasks(uris, rarity, download_images=True):
    result = []

    for uri in uris:
        soup = BeautifulSoup(get_page(uri).read())

        recovery_types = []

        rows = soup.select("table.sortable tr")
        for row in rows:
            if len(row.select("th")) > 0:
                recovery_types.clear()
                for header in row.select("th"):
                    if header.get_text() == "Life":
                        recovery_types.append("LifeRecovery")
                    if header.get_text() == "Mana":
                        recovery_types.append("ManaRecovery")
                continue

            cols = row.select("td")

            result.append('<Entity Type="Flask">\n')
            name = cols[0].select("a")[0].get_text().strip()
            result.append(create_property_xml("Name", name, 1))
            result.append(create_property_xml("Rarity", rarity, 1))
            result.append(create_property_xml("Base", cols[1].select("a")[0].get_text(), 1))
            result.append(create_property_xml("Level", cols[2].get_text(), 1))

            i = 3
            for recovery_type in recovery_types:
                result.append(create_property_xml(recovery_type, cols[i].get_text(), 1))
                i = i + 1

            result.append(create_property_xml("Duration", cols[i].get_text(), 1))
            result.append(create_property_xml("Capacity", cols[i+1].get_text(), 1))
            result.append(create_property_xml("ChargesUsed", cols[i+2].get_text(), 1))
            result.append(create_property_xml("Mods", format_item_mods(cols[i+3].get_text()), 1))
            # all utility flasks have implicits but wiki stores it in very inconvinient format
            # .. so for now the implicits are not parsed
            #result.append(create_delim_xml("HasImplicitMod", 1))
            result.append("</Entity>\n")

            uri_detailed = "http://pathofexile.gamepedia.com" + cols[0].select("a")[0].attrs["href"]
            soup = BeautifulSoup(get_page(uri_detailed).read())

            if download_images:
                try:
                    imageUri = soup.select("div.itemboximage")[0].select("img")[0].attrs["src"]
                    download_file(imageUri, "cache", name + ".png")
                except:
                    print("Could not download image")

            print("- parsed flask called " + clean_off_unicode(name))

    return result


def parse_armours(uris, type, rarity, download_images=True):
    result = []

    for uri in uris:
        soup = BeautifulSoup(get_page(uri).read())

        rows = soup.select("table.sortable tr")
        for row in rows:
            if len(row.select("th")) > 0:
                continue

            cols = row.select("td")

            result.append('<Entity Type="Armour">\n')
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


def parse_accessories(uris, type, rarity, download_images=True):
    result = []

    for uri in uris:
        soup = BeautifulSoup(get_page(uri).read())

        rows = soup.select("table.sortable tr")
        for row in rows:
            if len(row.select("th")) > 0:
                continue

            cols = row.select("td")

            result.append('<Entity Type="Accessory">\n')
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

            result.append('<Entity Type="Shield">\n')
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


def parse_weapons(uris, type, rarity, download_images=True):
    result = []

    for uri in uris:
        soup = BeautifulSoup(get_page(uri).read())

        stats_requirements = []

        rows = soup.select("table.sortable tr")
        for row in rows:
            # Figure out what stat requirements this table has
            if len(row.select("th")) > 0:
                stats_requirements.clear()
                for header in row.select("th"):
                    if len(header.select("img")) > 0:
                        stat_type = header.select("img")[0].attrs["alt"]
                        if stat_type == "Str.":
                            stats_requirements.append("Strength")
                        if stat_type == "Dex.":
                            stats_requirements.append("Dexterity")
                        if stat_type == "Int.":
                            stats_requirements.append("Intelligence")
                continue

            cols = row.select("td")

            result.append('<Entity Type="Weapon">\n')
            name = cols[0].select("a")[0].get_text().strip()
            result.append(create_property_xml("Name", name, 1))
            result.append(create_property_xml("Type", type, 1))
            result.append(create_property_xml("Rarity", rarity, 1))
            result.append(create_property_xml("Base", cols[1].select("a")[0].get_text(), 1))
            result.append(create_property_xml("Level", cols[2].get_text(), 1))

            # Get stat requirements
            i = 3
            for stat_req in stats_requirements:
                result.append(create_property_xml(stat_req, cols[i].get_text(), 1))
                i = i + 1

            result.append(create_property_xml("PhysicalDamage", cols[i].get_text(), 1))
            result.append(create_property_xml("CriticalStrikeChance", cols[i+1].get_text(), 1))
            result.append(create_property_xml("AttacksPerSecond", cols[i+2].get_text(), 1))
            result.append(create_property_xml("DamagePerSecond", cols[i+3].get_text(), 1))
            result.append(create_property_xml("Mods", format_item_mods(cols[i+4].get_text()), 1))
            if len(cols[i+4].select("span.itemboxstatsgroup")) > 1:
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
# endregion


# -=-=-=-=-=-=-=-=-=-=-=- ENTRY POINT -=-=-=-=-=-=-=-=-=-=-=- #

# -- All URIs
uris_currency = ["http://pathofexile.gamepedia.com/Currency"]
uris_unique_maps = ["http://pathofexile.gamepedia.com/List_of_unique_maps"]
uris_unique_jewels = ["http://pathofexile.gamepedia.com/List_of_unique_jewels"]
uris_unique_flasks = ["http://pathofexile.gamepedia.com/List_of_unique_flasks"]

uris_unique_bodyarmour = ["http://pathofexile.gamepedia.com/List_of_unique_body_armours"]
uris_unique_helmet = ["http://pathofexile.gamepedia.com/List_of_unique_helmets"]
uris_unique_gloves = ["http://pathofexile.gamepedia.com/List_of_unique_gloves"]
uris_unique_boots = ["http://pathofexile.gamepedia.com/List_of_unique_boots"]

uris_unique_shields = ["http://pathofexile.gamepedia.com/List_of_unique_shields"]

uris_unique_amulets = ["http://pathofexile.gamepedia.com/List_of_unique_amulets"]
uris_unique_belts = ["http://pathofexile.gamepedia.com/List_of_unique_belts"]
uris_unique_rings = ["http://pathofexile.gamepedia.com/List_of_unique_rings"]
uris_unique_quivers = ["http://pathofexile.gamepedia.com/List_of_unique_quivers"]

uris_unique_axes = ["http://pathofexile.gamepedia.com/List_of_unique_axes"]
uris_unique_bows = ["http://pathofexile.gamepedia.com/List_of_unique_bows"]
uris_unique_claws = ["http://pathofexile.gamepedia.com/List_of_unique_claws"]
uris_unique_daggers = ["http://pathofexile.gamepedia.com/List_of_unique_daggers"]
uris_unique_rods = ["http://pathofexile.gamepedia.com/List_of_unique_fishing_rods"]
uris_unique_maces = ["http://pathofexile.gamepedia.com/List_of_unique_maces"]
uris_unique_swords = ["http://pathofexile.gamepedia.com/List_of_unique_swords"]
uris_unique_staves = ["http://pathofexile.gamepedia.com/List_of_unique_staves"]
uris_unique_wands = ["http://pathofexile.gamepedia.com/List_of_unique_wands"]

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
if not os.path.exists("unique_maps.xml"):
    print("#Unique maps")
    nodes = parse_maps(uris_unique_maps, "Unique", need_download_images)
    dump_xml(nodes, "unique_maps.xml")
    print()
if not os.path.exists("unique_jewels.xml"):
    print("#Unique jewels")
    nodes = parse_jewels(uris_unique_jewels, "Unique", need_download_images)
    dump_xml(nodes, "unique_jewels.xml")
    print()
if not os.path.exists("unique_flasks.xml"):
    print("#Unique flasks")
    nodes = parse_flasks(uris_unique_flasks, "Unique", need_download_images)
    dump_xml(nodes, "unique_flasks.xml")
    print()

if not os.path.exists("unique_body_armours.xml"):
    print("#Unique Body Armours")
    nodes = parse_armours(uris_unique_bodyarmour, "Body Armour", "Unique", need_download_images)
    dump_xml(nodes, "unique_body_armours.xml")
    print()
if not os.path.exists("unique_helmets.xml"):
    print("#Unique Helmets")
    nodes = parse_armours(uris_unique_helmet, "Helmet", "Unique", need_download_images)
    dump_xml(nodes, "unique_helmets.xml")
    print()
if not os.path.exists("unique_gloves.xml"):
    print("#Unique Gloves")
    nodes = parse_armours(uris_unique_gloves, "Gloves", "Unique", need_download_images)
    dump_xml(nodes, "unique_gloves.xml")
    print()
if not os.path.exists("unique_boots.xml"):
    print("#Unique Boots")
    nodes = parse_armours(uris_unique_boots, "Boots", "Unique", need_download_images)
    dump_xml(nodes, "unique_boots.xml")
    print()

if not os.path.exists("unique_amulets.xml"):
    print("#Unique Amulets")
    nodes = parse_accessories(uris_unique_amulets, "Amulet", "Unique", need_download_images)
    dump_xml(nodes, "unique_amulets.xml")
    print()
if not os.path.exists("unique_belts.xml"):
    print("#Unique Belts")
    nodes = parse_accessories(uris_unique_belts, "Belt", "Unique", need_download_images)
    dump_xml(nodes, "unique_belts.xml")
    print()
if not os.path.exists("unique_rings.xml"):
    print("#Unique Rings")
    nodes = parse_accessories(uris_unique_rings, "Ring", "Unique", need_download_images)
    dump_xml(nodes, "unique_rings.xml")
    print()
if not os.path.exists("unique_quivers.xml"):
    print("#Unique Quivers")
    nodes = parse_accessories(uris_unique_quivers, "Quiver", "Unique", need_download_images)
    dump_xml(nodes, "unique_quivers.xml")
    print()

if not os.path.exists("unique_shields.xml"):
    print("#Unique Shields")
    nodes = parse_shields(uris_unique_shields, "Unique", need_download_images)
    dump_xml(nodes, "unique_shields.xml")
    print()

if not os.path.exists("unique_axes.xml"):
    print("#Unique Axes")
    nodes = parse_weapons(uris_unique_axes, "Axe", "Unique", need_download_images)
    dump_xml(nodes, "unique_axes.xml")
    print()
if not os.path.exists("unique_bows.xml"):
    print("#Unique Bows")
    nodes = parse_weapons(uris_unique_bows, "Bow", "Unique", need_download_images)
    dump_xml(nodes, "unique_bows.xml")
    print()
if not os.path.exists("unique_claws.xml"):
    print("#Unique Claws")
    nodes = parse_weapons(uris_unique_claws, "Claw", "Unique", need_download_images)
    dump_xml(nodes, "unique_claws.xml")
    print()
if not os.path.exists("unique_daggers.xml"):
    print("#Unique Daggers")
    nodes = parse_weapons(uris_unique_daggers, "Dagger", "Unique", need_download_images)
    dump_xml(nodes, "unique_daggers.xml")
    print()
if not os.path.exists("unique_rods.xml"):
    print("#Unique Rods")
    nodes = parse_weapons(uris_unique_rods, "Rod", "Unique", need_download_images)
    dump_xml(nodes, "unique_rods.xml")
    print()
if not os.path.exists("unique_maces.xml"):
    print("#Unique Maces")
    nodes = parse_weapons(uris_unique_maces, "Mace", "Unique", need_download_images)
    dump_xml(nodes, "unique_maces.xml")
    print()
if not os.path.exists("unique_swords.xml"):
    print("#Unique Swords")
    nodes = parse_weapons(uris_unique_swords, "Sword", "Unique", need_download_images)
    dump_xml(nodes, "unique_swords.xml")
    print()
if not os.path.exists("unique_staves.xml"):
    print("#Unique Staves")
    nodes = parse_weapons(uris_unique_staves, "Staff", "Unique", need_download_images)
    dump_xml(nodes, "unique_staves.xml")
    print()
if not os.path.exists("unique_wands.xml"):
    print("#Unique Wands")
    nodes = parse_weapons(uris_unique_wands, "Wand", "Unique", need_download_images)
    dump_xml(nodes, "unique_wands.xml")
    print()