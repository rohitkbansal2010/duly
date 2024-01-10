SHELL := /bin/bash

PACKAGE_NAME=dupage-craft
DATE=$(shell date '+%F_%H-%M-%S')
DATE_DIR=$(shell date '+%B %d, %Y')
CURRENTDIR=$(shell pwd)

ROADRUNNER_VERSION=1.3.5

MYSQLSH := $(shell command -v mysqlsh 2> /dev/null)
PLATFORM := $(shell uname)
OPTIONS=--renew-anon-volumes --force-recreate --scale queue=3
DOCKER_FILES=-f docker-compose.yml

GIT_VERSION = $(shell git describe --dirty --always --tags)
SELECTED_IP := $(shell ifconfig | grep inet | grep -v inet6 | grep broadcast | head -n 1 | awk '{ print $$2 }')

mkfile_path := $(abspath $(lastword $(MAKEFILE_LIST)))
CURRENT_DIR := $(notdir $(patsubst %/,%,$(dir $(mkfile_path))))

MULTIPASS_CONTAINER := 0

TEST?=

UNAME_S := $(shell uname -s)
ifeq ($(UNAME_S),Linux)
DOCKER0=$(shell ip route | grep docker0 | awk '{print $$9}')
endif

include config/.env
export

ifeq ($(HOST),)
HOST=127.0.0.1
endif

.DEFAULT_GOAL := default

.PHONY: default
default: .env mutagen composer-install vendor migrate plugins web/assets config/osm/tileservergl/tiles/map.mbtiles ## Brings up a local development environment

help:	## Lists all available commands and a brief description.
	@grep -E '^[a-zA-Z/_-]+:.*?## .*$$' $(MAKEFILE_LIST) | sort | awk 'BEGIN {FS = ":.*?## "}; {printf "\033[36m%-30s\033[0m %s\n", $$1, $$2}'

reinstall:	## Does a clean re-install of craft from an existing project.yaml configuration. This should be used if you need to perform a complete reset of your environment to the last project-config
	$(MAKE) down
	@echo "Resetting the database, please wait..."
	rm -rf config/.docker/certs
	docker volume rm $(shell basename $(CURDIR))_mariadb 2>/dev/null; true
	docker volume rm $(shell basename $(CURDIR))_vendor 2>/dev/null; true
	docker volume rm $(shell basename $(CURDIR))_node_modules 2>/dev/null; true
	@echo "... done. Re-installing the application"
	rm -rf .installed
	$(MAKE) default

migrate:
	docker-compose exec php ./craft migrate --track=craft --interactive=0
	docker-compose exec php ./craft migrate --interactive=0
	docker-compose exec php ./craft migrate/all --interactive=0

web/assets:	## Creates the assets directory if it does not exist
	mkdir -p web/assets
	chmod -R 775 web/assets

plugins: .installed resync-project	## Installs plugins
	docker-compose exec php ./craft plugin/install ALL
	docker-compose exec php ./craft plugin/enable ALL
	$(MAKE) resync-project
	docker-compose exec php ./craft migrate --track=craft --interactive=0

craft-update: migrate resync-project 	 ## Command to update Craft
	docker-compose exec php ./craft update
	if [ ! -f ./config/plugins.yaml ]; then \
		docker-compose exec php php ./config/yamlhelper.php; \
		docker-compose exec php php ./config/yamlhelper.php; \
	fi;
	$(MAKE) migrate

resync-project:	## Resyncs the project yaml
	docker-compose exec php ./craft project-config/apply

.PHONY: .env
.env:	## Creates a local .env file
ifneq ($(CURRENT_DIR), cms)
$(error "The current working directory must be called 'cms'. Rename your local working directory to 'cms' then re-run your command.")
endif
	cp -n ./config/.env ./.env
	cp ./.env ./tests/.env
	sed -i -e "s/https:\/\/127.0.0.1/https:\/\/$(HOST)/g" .env

composer.lock: composer.json	## Updates composer dependencies
	docker-compose exec php /bin/bash -lc "/root/.bin/composer update -ovn"
	docker-compose restart queue

vendor: vendor/composer/autoload_classmap.php composer.lock	## Installs composer dependencies

vendor/composer/autoload_classmap.php:
	docker-compose exec php /bin/bash -lc "/root/.bin/composer install -ovn"
	docker-compose restart queue

view-logs:	## Opens up a streaming log output
	docker-compose exec php tail -f /var/www/storage/logs/phperrors.log

.installed:	## Installs Craft3 to your database. This command will only be run once unless your explicitly run `make reinstall`
	@echo "Waiting for MariaDB to startup..."

	docker-compose exec mariadb /bin/bash -lc "while ! mysqladmin ping -h"127.0.0.1" --silent; do sleep 1; done"
	docker-compose exec mariadb mysql -u root -proot -e "GRANT ALL PRIVILEGES ON root.* TO 'local'@'%' IDENTIFIED BY 'local' REQUIRE SSL;";
	docker-compose exec php php ./config/yamlhelper.php
	docker-compose exec php ./craft install/craft --email=admin@example.com --language=en-US --site-name="DuPage Medical Group" --password=administrator --username=admin --site-url=https://$(HOST) --interactive=0
	docker-compose exec php php ./config/yamlhelper.php
	docker-compose exec php ./craft project-config/apply
	touch .installed

composer-install:	## Intalls composer dependencies
	docker-compose exec php /bin/bash -lc "/root/.bin/composer install -ovn --prefer-source"

composer-update:	## Updates composer dependencies
	docker-compose exec php /bin/bash -lc "/root/.bin/composer update -ovn --prefer-source"

sql:	## Opens a MySQL prompt
ifeq (, $(shell which mysqlsh))
	docker-compose exec mariadb mysql -u local -plocal root -h 127.0.0.1 --ssl-ca=/var/lib/mysql/crt/ca.pem --ssl-key=/var/lib/mysql/crt/client-key.pem --ssl-cert=/var/lib/mysql/crt/client-cert.pem
else
	mysqlsh -u local -plocal -h 127.0.0.1 --database=root --sql --ssl-ca=config/.docker/certs/mysql/ca.pem --ssl-key=config/.docker/certs/mysql/client-key.pem --ssl-cert=config/.docker/certs/mysql/client-cert.pem
endif

redis:	## Opens a redis-cli prompt
	docker-compose exec redis redis-cli

php:	## Opens up a PHP readline sheel
	docker-compose exec php php -a

docker: down config/.docker/certs/server.crt config/saml/certs/saml.crt config/saml/certs/sp.crt config/.docker/certs/mysql/server-cert.pem	## Starts the docker environment
	docker-compose $(DOCKER_FILES) pull --parallel

	# Composer can start a process
	docker-compose run --no-deps --entrypoint=/root/.bin/composer php install -ovn
	$(MAKE) down

	HOST=$(HOST) docker-compose $(DOCKER_FILES) up -d --remove-orphans $(OPTIONS)

	docker-compose exec php /bin/bash -lc "echo session.save_handler=redis > /etc/php/7.4/conf.d/session-save-handler.ini"
	docker-compose exec php /bin/bash -lc "echo session.save_path="tcp://redis:6379" | tee -a /etc/php/7.4/conf.d/session-save-handler.ini"
	docker-compose exec php /bin/bash -lc "echo post_max_size=10M\; > /etc/php/7.4/conf.d/files.ini"
	docker-compose exec php /bin/bash -lc "echo upload_max_filesize=10M\; >> /etc/php/7.4/conf.d/files.ini"
	docker-compose exec php /bin/sed -i.bak 's/request_terminate_timeout = 30s/request_terminate_timeout = 300/g' /etc/php/7.4/php-fpm.d/pool.conf

	# igbinary php serializer replacement
	docker-compose exec php /bin/bash -lc "echo session.serialize_handler=igbinary | tee -a /etc/php/7.4/conf.d/igbinary-settings.ini"
	docker-compose exec php /bin/bash -lc "echo igbinary.compact_strings=On | tee -a /etc/php/7.4/conf.d/igbinary-settings.ini"

	# igbinary php serializer replacement
	docker-compose exec php /bin/bash -lc "echo session.serialize_handler=igbinary | tee -a /etc/php/7.4/conf.d/igbinary-settings.ini"
	docker-compose exec php /bin/bash -lc "echo igbinary.compact_strings=On | tee -a /etc/php/7.4/conf.d/igbinary-settings.ini"

	# trust self-signed cert
	docker-compose exec php /bin/bash -lc "cp /var/www/config/.docker/certs/server.crt /usr/local/share/ca-certificates/dmgserver.crt"
	docker-compose exec php /bin/bash -lc "update-ca-certificates"

	docker-compose restart php

ifeq ($(UNAME_S),Linux) # host.docker.internal doesn't exist on Linux, redefine if necessary for xdebug automation
	docker-compose exec php /bin/bash -lc "if grep -r 'host.docker.internal' /etc/hosts; then echo 'Linux host.docker.internal is already set'; else echo $(DOCKER0) host.docker.internal | tee -a /etc/hosts; fi"
endif

mutagen: config/osm/tileservergl/tiles/map.mbtiles docker

down:	## Brings down the docker environment
	docker-compose down --remove-orphans

config/.docker/certs/mysql: ## Creates the MySQL certificates directory
	mkdir -p config/.docker/certs/mysql

config/.docker/certs/mysql/server-cert.pem: config/.docker/certs/mysql ## Creates the MySQL TLS certificate
	touch ~/.rnd
	# Generate the root CA
	openssl genrsa 2048 > config/.docker/certs/mysql/ca-key.pem
	openssl req -sha256 -new -x509 -nodes -days 3650 -subj '/C=US/ST=Illinois/L=Chicago/O=Punchkick Interactive/OU=Development DuPage Medical Group, Craft 3 CMS, MySQL)/CN=mariadb/emailAddress=systems.dupagecms@punchkick.com' -key config/.docker/certs/mysql/ca-key.pem > config/.docker/certs/mysql/ca-cert.pem

	# Generate server certificates
	openssl req -sha256 -newkey rsa:2048 -days 730 -nodes  -subj '/C=US/ST=Illinois/L=Chicago/O=Punchkick Interactive/OU=Development DuPage Medical Group, Craft 3 CMS, MySQL)/CN=mariadb/emailAddress=systems.dupagecms@punchkick.com' -keyout config/.docker/certs/mysql/server-key.pem > config/.docker/certs/mysql/server-req.pem
	openssl rsa -in config/.docker/certs/mysql/server-key.pem -out config/.docker/certs/mysql/server-key.pem
	openssl x509 -sha256 -req -in config/.docker/certs/mysql/server-req.pem -days 730 -CA config/.docker/certs/mysql/ca-cert.pem -CAkey config/.docker/certs/mysql/ca-key.pem -set_serial 01 > config/.docker/certs/mysql/server-cert.pem

	# Generate Client certificates
	openssl req -sha256 -newkey rsa:2048 -days 730 -nodes -subj '/C=US/ST=Illinois/L=Chicago/O=Punchkick Interactive/OU=Development DuPage Medical Group, Craft 3 CMS, MySQL)/CN=mariadb-client/emailAddress=systems.dupagecms@punchkick.com' -keyout config/.docker/certs/mysql/client-key.pem > config/.docker/certs/mysql/client-req.pem
	openssl rsa -in config/.docker/certs/mysql/client-key.pem -out config/.docker/certs/mysql/client-key.pem
	openssl x509 -sha256 -req -in config/.docker/certs/mysql/client-req.pem -days 730 -CA config/.docker/certs/mysql/ca-cert.pem -CAkey config/.docker/certs/mysql/ca-key.pem -set_serial 01 > config/.docker/certs/mysql/client-cert.pem

	cat config/.docker/certs/mysql/server-cert.pem config/.docker/certs/mysql/client-cert.pem > config/.docker/certs/mysql/ca.pem

	chmod -R 777 ./config/.docker/certs/mysql || true

config/.docker/certs: ## Creates the certificates directory
	mkdir -p config/.docker/certs

config/.docker/certs/server.crt: config/.docker/certs ## Creates the Nginx TLS certificate
	openssl req -x509 -nodes -newkey rsa:4096 \
        -keyout ./config/.docker/certs/server.key \
        -out ./config/.docker/certs/server.crt \
        -subj '/C=US/ST=Illinois/L=Chicago/O=Punchkick Interactive/OU=Development DuPage Medical Group, Craft 3 CMS)/CN=$(HOST)/emailAddress=systems.dupagecms@punchkick.com'

config/osm/tileservergl/tiles/map.mbtiles: config/osm/tileservergl/sprites ## Prepares the TileServer GL environment
	if [ ! -f ./config/osm/tileservergl/tiles/map.mbtiles ]; then \
		mkdir -p config/osm/tileservergl/tiles; \
		wget --output-file config/osm/tileservergl/tiles/map.mbtiles https://dmgwebdevstorage.blob.core.windows.net/dmgdevweb/illinois.mbtiles?sv=2020-08-04&ss=bqtf&srt=sco&sp=rwdlacuptfxi&se=2022-06-09T01:53:24Z&sig=MMlJUHMXV%2BAVq5FfFSNfoGPJJSzr%2BpNO7FXClSkBuXM%3D&_=1654710809484; \
	fi;

config/osm/tileservergl/sprites: ## Creates the tielservergl sprites directory
	mkdir -p config/osm/tileservergl/sprites

test:
	docker-compose exec php /bin/bash -lc "./vendor/bin/codecept run $(TEST)"

config/saml/certs: ## Creates the certificates directory
	mkdir -p config/saml/certs

config/saml/certs/saml.crt: config/saml/certs ## Creates the Nginx TLS certificate
	openssl req -x509 -nodes -newkey rsa:4096 \
        -keyout ./config/saml/certs/saml.key \
        -out ./config/saml/certs/saml.crt \
        -subj '/C=US/ST=Illinois/L=Chicago/O=Wipfli Digital/OU=Dupage Medical Group - SAML2 IDP)/CN=$(HOST)/emailAddress=systems.genesis@punchkick.com'

config/saml/certs/sp.crt: config/saml/certs ## Creates the Nginx TLS certificate
	openssl req -x509 -nodes -newkey rsa:4096 \
        -keyout ./config/saml/certs/sp.key \
        -out ./config/saml/certs/sp.crt \
        -subj '/C=US/ST=Illinois/L=Chicago/O=Wipfli Digital/OU=Dupage Medical Group - SAML2 SP)/CN=$(HOST)/emailAddress=systems.genesis@punchkick.com'
