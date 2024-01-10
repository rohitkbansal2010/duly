# DuPage Medical Group CMS

A Craft 3 CMS project for DuPage Medial Group

## Installation

Local development is done through [Docker](https://www.docker.com/).

1. Install Docker onto your system and ensure it is running.
2. Use the provided `Makefile` to install the project:
```
make
```

> On MacOS, `make` may need to be installed through [Homebrew](https://brew.sh/).

```
brew install make
```

3. Once the installer has finished, you may login to the admin panel at https://127.0.0.1/admin/login with the following credentials:

```
admin@example.com
administrator
```

## Developer Configuration

The following notes regarding the application configuration are made for developers.

### Docker

The development environment is managed through `Docker`. Docker will make several services available, namely the following:

| Service | Purpose | URL |
|---------|---------|-----|
| Website | Main Website | https://127.0.0.1 |
| Mailhog | SMTP Mock Relay | https://127.0.0.1/mail |
| Webpack | Webpack Dev Server | https://127.0.0.1/dist |
| MariaDB | MySQL Database | 127.0.0.1:3306 |
| Redis | Redis Database | 127.0.0.1:6379 |

### Webpack

Webpack is utilizes for asset generation. Source files are located in `src/` within their respective type directory. A default `webpack.config.js` is provided withithe project template in the `./config` with full ES6 and SCSS support.

Integration with Craft is achieved through the [TwigPack Plugin](https://github.com/nystudio107/craft-twigpack), which is already installed and configured.

### Craft 3

Craft 3 has several configuration files for it uses for configuration.

#### Settings

General, app, and plugin settings are stored in the `./config` directory. Known files are listed as follows:

| File | Purpose |
|------|---------|
| `app.php` | Yii2 config file format configuration that applies to all configurations |
| `app.web.php` | Yii2 config file format configuration for web. |
| `app.console.php` | Yii2 config file format configuration for console. |
| `db.php` | dotenv loaded database configuration. Do not touch this file. |
| `general.php` | General Craft 3 specific settings. Ref: https://docs.craftcms.com/v3/config/#general-config-settings |
| `routes.php` | Custom routes that wouldn't be defined in `project.yaml` |
| `twigpack.php` | [TwigPack Plugin](https://github.com/nystudio107/craft-twigpack) configuration file. Do not touch this file. |
| `password-policy.php` | Password policy configuration for [Password Policy Plugin](https://github.com/Rias500/craft-password-policy).

#### Project YAML

For end-users, Craft is intended to be managed from the web interface. For developers however, the `project.yaml` file serves as a way to store specific configuration settings across environments and programatically defined various components such as Channels, Singles, Entries, and other Craft 3 specific functionality.

Typically when any change is made in the admin portal, the `project.yaml` file will be updated. This allows interoperability between environments independent of the database schema.

For more information about the `project.yaml` file reference https://docs.craftcms.com/v3/project-config.html.

## Console Commands

### Makefile commands

The `Makefile` abstracts several console commands for ease of use. Important and common to use commands are listed as follows:

| Command | Description |
|---------|-------------|
| `plugins` | Installs and enables all plugins |
| `craft-update` | Updates craft |
| `view-logs` | Streams PHP errors |
| `composer-install` | Runs `composer install` |
| `composer-update` | Runs `composer update` |
| `sql` | Starts a MySQL interactive prompt |
| `redis` | Starts an interactive Redis prompt |
| `php` | Starts a PHP REPL |
| `lxc` | Creates a local LXC container using the current working directory |

### Craft commands

Craft exposes several commands through the `./craft` CLI command (which behaves identically to a standard `./yii` command.). In general you can run any command through the docker containers by running:

```
docker-compose exec php ./craft <command>
```

Without any command, `./craft` will dump all available commands and a brief description of that commands

## Production Release Commands

```
lxc image copy <image>
lxc launch <image> dmg/version
lxc file push ${DRONE_COMMIT_SHA}.tar.gz dmg/version/var/www/build.tar.gz
lxc exec dmg/version -- tar -zxf /var/www/build.tar.gz -C /var/www
lxc exec dmg/version -- chmod +x /var/www/config/.lxc/install.sh
lxc exec dmg/version -- mkdir -p /var/www/storage/runtime
lxc exec dmg/version -- mkdir -p /var/www/storage/logs
lxc exec dmg/version -- mkdir -p /var/www/storage/runtime/compiled_templates
lxc exec dmg/version -- mkdir -p /var/www/storage/runtime/temp
lxc exec dmg/version -- mkdir -p /var/www/storage/runtime/mutex
lxc exec dmg/version -- mkdir -p /var/www/storage/runtime/compiled_classes
lxc exec dmg/version -- mkdir -p /var/www/storage/rebrand
lxc exec dmg/version -- mkdir -p /var/www/config/osm/tileservergl/sprites
lxc exec dmg/version -- mkdir -p /var/www/config/osm/tileservergl/tiles
lxc exec dmg/version -- chmod -R 775 /var/www/config /var/www/storage/runtime /var/www/storage/logs /var/www/web/cpresources
lxc exec dmg/version -- chown -R www-data:www-data /var/www
lxc exec dmg/version -- setfacl -d -Rm u:www-data:rwx,g:www-data:rwx /var/www
lxc exec dmg/version -- setfacl -Rm u:www-data:rwx,g:www-data:rwx /var/www
lxc exec dmg/version -- rm -rf /var/www/build.tar.gz
lxc publish dmg/version --alias ${DRONE_REPO_OWNER}/cms/${DRONE_COMMIT_SHA}/${DRONE_BUILD_NUMBER} --force
lxc image export ${DRONE_BUILD_NUMBER}"
```

## Azure Container Registry

```
az acr login --name dulyweb
```