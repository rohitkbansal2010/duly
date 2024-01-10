# Configuring local OSM tiles and tile server

Overview steps:
1. Generate one `.mbtiles` file storing all necessary map tiles using the `tilemaker` binary.
2. Spin up a local TileServer GL server. It consumes the map tiles `.mbtiles` file and produces vector and raster maps with GL styles for web usage.
3. Configure nginx to proxy web map requests to the TileServer GL server.

### 1. Generating the `.mbtiles` file

1. Download necesary OSM files from https://download.geofabrik.de/. To generate a map tiles file covering the entire USA, download US Midwest, US Northeast, US Pacific, US South and US Wests OSM PBF files. 
  
 **Note: PBF Format is a highly compressed, optimized binary format containing raw OSM data. Depending on the covered area, each file may take upwards of 1GB.*

2. Install the Tilemaker binary on your system. Follow original instructions from github: https://github.com/systemed/tilemaker. 

3. Generate the `.mbtiles` file by running the following command:

  `tilemaker <downloaded-osm-pbf-filename>.osm.pbf --output=usa.mbtiles --process=process.lua --config=config.json`
   
 **Note#1: If you have multiple osm.pbf files, simply list them in any order in the command above. Tilemaker will comcatinate and compress all of the data in the final .mbtiles file.*
 
  **Note#2: Tilemaker requires two files: a json config file and a lua processing script. Included in this repo are example files that can be used. The json file defines layers and zoom levels, whereas the lua file assists processing the final `.mbtiles` file. Unless specific changes (e.g. adding/removing layers) are needed, provided example config and lua files can be used as-is. *
  
  
### 2. Configuring the TileServer GL server instance

1. Install Node.js. Check the documentation to see the latest installation instructions: https://nodejs.org/en/download/package-manager/#debian-and-ubuntu-based-linux-distributions

2. Install the tileserver-gl package with npm 
```sudo npm install -g tileserver-gl-light```

3. Setup your local system and service manager to run and manage the server. Below are example instructions using `systemd`.
    - Create a service file that describes the service
	```sudo nano /lib/systemd/system/tileserver-gl-light.service```
	- Paste the following code into the editor, save (ctrl+o) and close (ctrl+x) it.
	```
	[Unit]
    Description=TileServer GL Light Service
    After=network.target
    
    [Service]
    Type=simple
    User=www-data
    ExecStart=/usr/bin/tileserver-gl-light -p 10001 # change port if necessary
    WorkingDirectory=/opt/maps #change if necessary
    Restart=on-failure
    
    [Install]
    WantedBy=multi-user.target
    ```
	- Make systemd aware of the new configuration file and enable the service, so it automatically starts whenever the server reboots.
	```
sudo systemctl daemon-reload
sudo systemctl enable tileserver-gl-light
```
    - Start the service and check the status. If everything is okay you should see a green active (running) text
	```
sudo systemctl start tileserver-gl-light
sudo systemctl status tileserver-gl-light
```
4. Copy and paste provided tileserver resources into `/opt/maps` (if not changed above). 
    - `fonts`, `sprites`, and `styles` directories provide necessary resources to style map tiles
	- `tiles` directory should contain the `.mbtiles` file generated earlier.
	- `config.json` tells the TileServer GL instance where to find the map tiles and the map styles. `options.paths.` defines the locations of all the resources. `data.usa.` defines the file used as a tiles source. `options.domains` configures the domain that will be ussed to serve the tiles.
	Provided configuration adds the `/osm` prefix to all map-related requests. This can be changed in the `config.json` file and the `/styles/basic.json` file.


### 3. Configure nginx to serve the tiles and their styles.

1. Add the following location block to your nginx site configuration
	```
	    location /osm/ {
            proxy_set_header Host $host;
            proxy_set_header X-Forwarded-Proto $scheme;
            proxy_set_header X-Forwarded-Host $host;
            proxy_set_header X-Forwarded-Server $host;
            proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
            proxy_pass http://localhost:10001; # port set up in the systemd service
    }
	```