import os
import codecs
import urllib.request
from bs4 import BeautifulSoup


def get_page(uri):
    # PoE wiki forbids access if user agent does not belong to one of the popular browsers
    # .. so we're Google Chrome now
    headers = {
        "User-Agent": "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.90 Safari/537.36",
        "Accept": "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8"}
    req = urllib.request.Request(uri, headers=headers)
    return urllib.request.urlopen(req)


def download_file(uri, directory, name):
    if not os.path.exists(directory):
        os.makedirs(directory)
    urllib.request.urlretrieve(uri, os.path.join(directory, name))


def create_xml_node(name, value, indentations=0):
    name = str(name).strip()
    value = str(value).strip().replace("N/A", "0")
    return '  '*indentations + '<Property id="' + name + '">' + value + '</Property>\n'


def create_xml_document(nodes, file_name):
    file = codecs.open(file_name, 'w', encoding="utf-8")
    file.write('<?xml version="1.0" encoding="utf-8" ?>\n<Root>\n')
    file.writelines(nodes)
    file.write("</Root>")
    file.close()


def format_item_mods(mods):
    return mods.replace("N/A", "")\
            .replace("<", "[")\
            .replace(">", "]")\
            .replace("  ", " | ")


# region Parsers
def parse_currency(download_images=True):
    uri = "http://pathofexile.gamepedia.com/Currency"

    result = []

    soup = BeautifulSoup(get_page(uri).read())

    rows = soup.select("table.sortable tr")
    for row in rows:
        if len(row.select("th")) > 0:
            continue

        cols = row.select("td")

        result.append("<Entity>\n")
        name = cols[0].get_text().strip()
        result.append(create_xml_node("Name", name, 1))

        uri_detailed = "http://pathofexile.gamepedia.com" + cols[0].select("a")[0].attrs["href"]
        soup = BeautifulSoup(get_page(uri_detailed).read())
        result.append(create_xml_node("Description", soup.select("span.itemboxstatsgroup")[1].get_text(), 1))
        result.append("</Entity>\n")

        if download_images:
            imageUri = soup.select("div.itemboximage")[0].select("img")[0].attrs["src"]
            download_file(imageUri, "cache", name + ".png")

        print("- parsed currency called " + name)

    return result


def parse_armors(download_images=True):
    item_types = {
        "Body Armour": "http://pathofexile.gamepedia.com/List_of_unique_body_armours",
        "Helmet": "http://pathofexile.gamepedia.com/List_of_unique_helmets",
        "Gloves": "http://pathofexile.gamepedia.com/List_of_unique_gloves",
        "Boots": "http://pathofexile.gamepedia.com/List_of_unique_boots"
    }

    result = []

    for item_type in item_types.keys():
        soup = BeautifulSoup(get_page(item_types[item_type]).read())

        rows = soup.select("table.sortable tr")
        for row in rows:
            if len(row.select("th")) > 0:
                continue

            cols = row.select("td")

            result.append("<Entity>\n")
            name = cols[0].select("a")[0].get_text().strip()
            result.append(create_xml_node("Name", name, 1))
            result.append(create_xml_node("Type", item_type, 1))
            result.append(create_xml_node("Rarity", "Unique", 1))
            result.append(create_xml_node("Base", cols[1].select("a")[0].get_text(), 1))
            result.append(create_xml_node("Level", cols[2].get_text(), 1))
            result.append(create_xml_node("Strength", cols[3].get_text(), 1))
            result.append(create_xml_node("Dexterity", cols[4].get_text(), 1))
            result.append(create_xml_node("Intelligence", cols[5].get_text(), 1))
            result.append(create_xml_node("ArmourValue", cols[6].get_text(), 1))
            result.append(create_xml_node("EvasionValue", cols[7].get_text(), 1))
            result.append(create_xml_node("EnergyShieldValue", cols[8].get_text(), 1))
            result.append(create_xml_node("Mods", format_item_mods(cols[9].get_text()), 1))
            result.append("</Entity>\n")

            uri_detailed = "http://pathofexile.gamepedia.com" + cols[0].select("a")[0].attrs["href"]
            soup = BeautifulSoup(get_page(uri_detailed).read())

            if download_images:
                imageUri = soup.select("div.itemboximage")[0].select("img")[0].attrs["src"]
                download_file(imageUri, "cache", name + ".png")

            print("- parsed armor called " + name)

    return result


def parse_shields(download_images=True):
    uri = "http://pathofexile.gamepedia.com/List_of_unique_shields"

    result = []

    soup = BeautifulSoup(get_page(uri).read())

    rows = soup.select("table.sortable tr")
    for row in rows:
        if len(row.select("th")) > 0:
            continue

        cols = row.select("td")

        result.append("<Entity>\n")
        name = cols[0].select("a")[0].get_text().strip()
        result.append(create_xml_node("Name", name, 1))
        result.append(create_xml_node("Type", "Shield", 1))
        result.append(create_xml_node("Rarity", "Unique", 1))
        result.append(create_xml_node("Base", cols[1].select("a")[0].get_text(), 1))
        result.append(create_xml_node("Level", cols[2].get_text(), 1))
        result.append(create_xml_node("Strength", cols[3].get_text(), 1))
        result.append(create_xml_node("Dexterity", cols[4].get_text(), 1))
        result.append(create_xml_node("Intelligence", cols[5].get_text(), 1))
        result.append(create_xml_node("Block", cols[6].get_text(), 1))
        result.append(create_xml_node("ArmourValue", cols[7].get_text(), 1))
        result.append(create_xml_node("EvasionValue", cols[8].get_text(), 1))
        result.append(create_xml_node("EnergyShieldValue", cols[9].get_text(), 1))
        result.append(create_xml_node("Mods", format_item_mods(cols[10].get_text()), 1))
        result.append("</Entity>\n")

        uri_detailed = "http://pathofexile.gamepedia.com" + cols[0].select("a")[0].attrs["href"]
        soup = BeautifulSoup(get_page(uri_detailed).read())

        if download_images:
            imageUri = soup.select("div.itemboximage")[0].select("img")[0].attrs["src"]
            download_file(imageUri, "cache", name + ".png")

        print("- parsed shield called " + name)

    return result

# endregion


# -=-=-=-=-=-=-=-=-=-=-=- ENTRY POINT -=-=-=-=-=-=-=-=-=-=-=- #
inpt = input("Download images? [Y/N] ")
need_download_images = inpt.upper() == "Y"
print()

print("Parsing currency items")
nodes = parse_currency(need_download_images)
create_xml_document(nodes, "currency.xml")
print()

print("Parsing unique items")

print("- armours")
nodes = parse_armors(need_download_images)
create_xml_document(nodes, "unique_armours.xml")
print()

print("- shields")
nodes = parse_shields(need_download_images)
create_xml_document(nodes, "unique_shields.xml")
print()