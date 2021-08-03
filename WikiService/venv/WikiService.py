from flask import Flask
from flask import request
import wikipedia
import requests
from bs4 import BeautifulSoup
import re

app = Flask(__name__)

@app.route("/getWikiData", methods=['GET'])
def getWikiData():

    name = request.args.get('name')

    if name == None:
        return {"error": "must provide a name"}

    try: 
        
        wiki = wikipedia.page(name)

    except:

        return {"error": "there was an error getting your page"}

    summary = wiki.summary
    
    if wiki.section('History') == None:

        history = ""

    else:

        history = wiki.section('History')

    html = wiki.html()
    soup = BeautifulSoup(html, 'html.parser')

    coordinates = soup.find_all("span", class_="geo-dec")

    if len(coordinates) == 0 or len(coordinates) > 1:

        lat = "not found"
        lon = "not found"
    
    coordinateString = coordinates[0].string
    
    lat = coordinateString.split(' ')[0]
    lon = coordinateString.split(' ')[1]

    data = {
        "history": history,
        "summary": summary,
        "location": {
            "lat": lat,
            "lon": lon
            }
        }

    return data