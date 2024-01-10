import _ from 'lodash';

function formatAppointmentDateAndTime(date: string) {
  const dt = new Date(date);
  const dateFormat = Intl.DateTimeFormat('en-US', { weekday: 'long', month: 'long', day: 'numeric' });
  const timeFormat = Intl.DateTimeFormat('en-US', { hour: 'numeric', minute: 'numeric' });
  return `${dateFormat.format(dt)}, at ${timeFormat.format(dt).toLowerCase()}`;
}

function formatAddress(street: string, city: string, state: string, zip: string) {
  const parts = [];
  street && parts.push(street);
  city && parts.push(city);
  (state || zip) && parts.push([state, zip].filter((i) => !!i).join(' '));
  return parts.join(', ');
}

export function loadTemplates(key: string): any {
  if (!key.match(/^[-a-z0-9]+$/i)) {
    throw new Error(`Wrong templates key: ${key}`);
  }
  const asset = Runtime.getAssets()[`/${key}.js`];
  if (!asset) {
    throw new Error(`Templates for "${key}" were not found`);
  }
  // eslint-disable-next-line global-require, import/no-dynamic-require
  return require(asset.path);
}

export function getCompiledTemplate(key: string, variant?: string) {
  const templates = loadTemplates(key);
  const templateOptions: _.TemplateOptions = {
    imports: { formatAppointmentDateAndTime, formatAddress },
  };
  if (variant) {
    if (!Object.hasOwnProperty.call(templates, variant) || typeof templates[variant] !== 'string') {
      throw new Error(`Wrong variant "${variant}" for templates "${key}"`);
    }
    return _.template(templates[variant], templateOptions);
  }

  if (typeof templates !== 'string') {
    throw new Error(`Wrong template "${key}"`);
  }
  return _.template(templates, templateOptions);
}
