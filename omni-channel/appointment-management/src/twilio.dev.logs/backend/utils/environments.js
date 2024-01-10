const fs = require('fs');
const path = require('path');
const dotenv = require('dotenv');

const ENV_REG_EXP = /^\.env\.([^.]+)$/;
const RESTRICTED_ENV_FILES = ['.env.example'];

function Environments(dir) {
  this.dir = dir;
  this.data = {};
}

Environments.prototype.get = function get(type) {
  if (!this.data[type]) {
    this.data[type] = dotenv.parse(
      fs.readFileSync(path.join(this.dir, `.env.${type}`)),
    );
  }
  return this.data[type];
};

Environments.prototype.listTypes = function listTypes() {
  const files = fs.readdirSync(this.dir)
    .filter((file) => ENV_REG_EXP.test(file) && !RESTRICTED_ENV_FILES.includes(file));
  return files.map((file) => file.match(ENV_REG_EXP)[1]);
};

module.exports = (directory, template) => new Environments(directory, template);
