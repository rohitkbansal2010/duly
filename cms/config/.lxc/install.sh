#!/usr/bin/env bash
#
# LXC Initialization script
#
# This script has been tailed to the specific application, and will be called on start by the container running the following tasks
#
# 1) Starts Wireguard peers in /etc/wireguard/*.conf, assuming the peer file name is in wgX.conf format.
# 2) If a /var/www/.env file is present, and DEFAULT_SITE_URL is defined, will create
#    an /etc/nginx/conf/DEFAULT_SITE_URL.conf file specific to the domain specified
#    and will instantiate it with /etc/nginx/conf/ssl/server.{crt,key} for TLS support
# 3) If a /var/www/.env file is present, and REDIS_HOST is defined, will create /etc/php/X.Y/conf.d/session.ini
#    and set the session.save_path and session.save_handler to the Redis configuration.
# 4) Run Craft 3 installation and migration scripts

# Verify the directory structure and necessary files exist
if [ ! -d /var/www/storage ]; then
    echo "The storage directory does not exist or was not mounted. Remount the directory then re-run this command.";
    exit 1;
fi

if [ ! -d /var/www/web/cpresources ]; then
    echo "The cpresources directory does not exist or was not mounted. Remount the directory then re-run this command.";
    exit 1;
fi

if [ ! -f /var/www/.env ]; then
    echo "The .env configuration file is not present. Remount this file then re-run this command.";
    exit 1;
fi

if [ ! -f /var/www/config/license.key ]; then
    echo "WARNING: Craft does not have a license.key. If this is the first time starting the container then you may ignore this warning";
fi

# If a Wireguard directory is present and there are configurations present
# Start wireguard for the selected peers
if [ -d /etc/wireguard ]; then
    if [ "$(ls -A /etc/wireguard)" ]; then
        for file in $(ls /etc/wireguard/*.conf)
        do
            z=$(echo "$file" | rev | cut -c 6- | rev)
            systemctl enable wg-quick@$z;
            systemctl start wg-quick@$z;
        done
    fi
fi

cp /var/www/config/.lxc/system.d/consul.service /etc/systemd/system/consul.service;
cp /var/www/config/.lxc/consul.d/app.json /etc/consul.d/app.json;
systemctl daemon-reload;
systemctl enable consul;
systemctl start consul;

# If the /var/www/.env dotenv file is present
# Update the appropriate Nginx and PHP configuration
if [ -f /var/www/.env ]; then
    set -o allexport;
    source /var/www/.env;
    set +o allexport;

    DOMAIN=$(echo $DEFAULT_SITE_URL | sed 's/https:\/\///')

    # If the domain is set to localhost, replace it with the IP address
    if [ "$DOMAIN" == "127.0.0.1" ]; then
        DOMAIN=""

        # The LXC image may take a while to get an IP, wait until we get one instead of defaulting to null
        while [ "$DOMAIN" == "" ]; do
            echo "Internal IP address isn't yet defined, re-checking and waiting 5 more seconds..."
            DOMAIN=$(hostname -I | cut -d' ' -f1)
            sleep 5;
        done

        sed -i "s/127.0.0.1/$DOMAIN/g" /var/www/.env
    fi

    # Update and apply the site specifix Nginx configuration
    cp /var/www/config/.lxc/nginx/nginx.conf /etc/nginx/conf/nginx.conf;
    cp /var/www/config/.lxc/nginx/conf.d/site.conf /etc/nginx/conf/conf.d/$DOMAIN.conf;
    sed -i -e "s/__DOMAIN__/$DOMAIN/g" /etc/nginx/conf/conf.d/$DOMAIN.conf;
    EPIC_ENDPOINT_URL_SAFE=$(echo "$EPIC_ENDPOINT_URL" |  sed 's/\//\\\//g')
    OSM_CDN_URL_SAFE=$(echo "$OSM_CDN_URL" |  sed 's/\//\\\//g')
    OSM_BASE_URL_SAFE=$(echo "$OSM_CDN_URL_BASE" |  sed 's/\//\\\//g')
    ASSETS_CDN_URL_SAFE=$(echo "$ASSETS_CDN_URL" |  sed 's/\//\\\//g')
    DIST_CDN_URL_SAFE=$(echo "$DIST_CDN_URL" |  sed 's/\//\\\//g')
    DIST_MOUNT_SAFE=$(echo "$DIST_MOUNT_POINT" |  sed 's/\//\\\//g')
    sed -i -e "s/__EPIC_ENDPOINT_URL__/$EPIC_ENDPOINT_URL_SAFE/g" /etc/nginx/conf/conf.d/$DOMAIN.conf;
    sed -i -e "s/__OSM_CDN_URL__/$OSM_CDN_URL_SAFE/g" /etc/nginx/conf/conf.d/$DOMAIN.conf;
    sed -i -e "s/__ASSETS_CDN_URL__/$ASSETS_CDN_URL_SAFE/g" /etc/nginx/conf/conf.d/$DOMAIN.conf;
    sed -i -e "s/__DIST_ENDPOINT_URL__/$DIST_CDN_URL_SAFE/g" /etc/nginx/conf/conf.d/$DOMAIN.conf;
    sed -i -e "s/__DIST_MNT__/$DIST_MOUNT_SAFE/g" /etc/nginx/conf/conf.d/$DOMAIN.conf;
    sed -i -e "s/__OSM_HOST__/$OSM_BASE_URL_SAFE/g" /etc/nginx/conf/conf.d/$DOMAIN.conf;

    # Extract the PHP version information
    PHP_VERSION=$(php -r "echo phpversion();")
    major=$(echo $PHP_VERSION | cut -d. -f1)
    minor=$(echo $PHP_VERSION | cut -d. -f2)

    # If session.ini isn't defined, then check to see if the session.save_* configuration
    # needs to be updated to point to a shared Redis cache
    if [ ! -f /etc/php/$major.$minor/conf.d/session.ini ]; then
        echo "Creating /etc/php/$major.$minor/conf.d/session.ini";
        if test "$REDIS_CLUSTER"; then
            echo "session.save_handler=rediscluster" > /etc/php/$major.$minor/conf.d/session.ini;
            echo "session.save_path='seed[]=$REDIS_HOST:$REDIS_PORT?distribute=true&persistent=true&failover=error&database=1'" >> /etc/php/$major.$minor/conf.d/session.ini;
        elif test "$REDIS_HOST"; then
            echo "session.save_handler=redis" > /etc/php/$major.$minor/conf.d/session.ini;
            echo "session.save_path='tcp://$REDIS_HOST:$REDIS_PORT?database=1'" >> /etc/php/$major.$minor/conf.d/session.ini;
        fi
    fi

    # Adjust the max file upload size
    if [ ! -f /etc/php/$major.$minor/conf.d/upload.ini ]; then
        echo "post_max_size = 20M" > /etc/php/$major.$minor/conf.d/upload.ini
        echo "upload_max_filesize = 20M" >> /etc/php/$major.$minor/conf.d/upload.ini
    fi

	# igbinary php serializer replacement
	echo session.serialize_handler=igbinary | tee -a /etc/php/$major.$minor/conf.d/igbinary-settings.ini
	echo igbinary.compact_strings=On | tee -a /etc/php/$major.$minor/conf.d/igbinary-settings.ini

    # Restart the relevant services
    systemctl restart nginx;
    systemctl restart php-fpm-$major.$minor;
else
    echo ".env is not defined. Mount your .env file to the LXC container then re-run this command";
    exit 1;
fi

mkdir -p /var/www/storage/{runtime,logs,misc}
# If Craft 3 is not installed run the installation script
# Note that a default password will be created for the systems@punchkick.com email address
if [ ! -f /var/www/config/license.key ]; then
    echo "Craft 3 is not yet installed. Running installation script...";
    password=$(dd if=/dev/urandom count=200 bs=1 2>/dev/null | tr -cd "[:alnum:]" | cut -c-32);
    /usr/bin/php /var/www/config/yamlhelper.php;
    /var/www/craft install/craft --email=systems@punchkick.com --language=en-US --site-name="DuPage Medical Group" --password=$password --username=admin --site-url=https://$DOMAIN --interactive=0;
    /usr/bin/php /var/www/config/yamlhelper.php;
    /var/www/craft project-config/apply;

    echo "Your administrative login for https://$DOMAIN is systems@punchkick.com / $password. Once Craft has finished installing, login to the control panel to change these details.";
fi

# If Craft is installed, run the plugin installation and run the default migrations
/var/www/craft plugin/install ALL;
/var/www/craft plugin/enable ALL;
/var/www/craft migrate --track=craft --interactive=0;
/var/www/craft migrate/all --interactive=0;
/var/www/craft project-config/apply;
/var/www/craft clear-caches/twigpack-manifest-cache;
/var/www/craft clear-caches/compiled-templates;
/var/www/craft clear-caches/temp-files;

chmod -R 775 /var/www/config /var/www/storage/{runtime,logs} /var/www/web/cpresources
chown -R www-data:www-data /var/www

cp /var/www/config/.lxc/system.d/craft-physicians-schedule.* /etc/systemd/system/;
systemctl daemon-reload;
systemctl enable craft-physicians-schedule.timer;
systemctl start craft-physicians-schedule.timer;

cp /var/www/config/.lxc/system.d/craft-external-resource-schedule.* /etc/systemd/system/;
systemctl daemon-reload;
systemctl enable craft-external-resource-schedule.timer --now;
systemctl start craft-external-resource-schedule.timer;

# Sends an email export pf physicians form the CMS
cp /var/www/config/.lxc/system.d/craft-email-custom-entries-export-as-csv.* /etc/systemd/system/;
systemctl daemon-reload;
systemctl enable craft-email-custom-entries-export-as-csv.timer;
systemctl start craft-email-custom-entries-export-as-csv.timer;

# Submits Request Appointment Form to SFTP Server every hour
cp /var/www/config/.lxc/system.d/craft-request-appointment-forms.* /etc/systemd/system/;
systemctl daemon-reload;
systemctl enable craft-request-appointment-forms.timer --now;
systemctl start craft-request-appointment-forms.timer;

cp /var/www/config/.lxc/system.d/craft-queue@.service /etc/systemd/system/craft-queue@.service;
systemctl daemon-reload;
systemctl enable craft-queue\@{1..3};
systemctl start craft-queue\@{1..3};

# Download the maptiles
wget --output-file  /var/www/config/osm/tileservergl/tiles/map.mbtiles https://dmgwebdevstorage.blob.core.windows.net/dmgdevweb/illinois.mbtiles?sv=2020-08-04&ss=bqtf&srt=sco&sp=rwdlacuptfxi&se=2022-06-09T01:53:24Z&sig=MMlJUHMXV%2BAVq5FfFSNfoGPJJSzr%2BpNO7FXClSkBuXM%3D&_=1654710809484

cp /var/www/config/.lxc/system.d/tileserver-gl.service /etc/systemd/system/tileserver-gl.service;
systemctl daemon-reload;
systemctl enable tileserver-gl;
systemctl start tileserver-gl;

# Install the imgproxy golang service if it's not already installed and running
cp /var/www/config/.lxc/system.d/imgproxy@.service /etc/systemd/system/imgproxy@.service;

sed -i -e "s/__SALT__/$IMGPROXY_SALT/g" /etc/systemd/system/imgproxy@.service;
sed -i -e "s/__KEY__/$IMGPROXY_KEY/g" /etc/systemd/system/imgproxy@.service;

systemctl daemon-reload;
systemctl enable imgproxy\@{1..5};
systemctl start imgproxy\@{1..5};

cp /var/www/config/.lxc/system.d/vector.service /etc/systemd/system/vector.service;
systemctl daemon-reload;
systemctl enable vector;
systemctl restart vector;

timedatectl set-timezone Etc/UTC;