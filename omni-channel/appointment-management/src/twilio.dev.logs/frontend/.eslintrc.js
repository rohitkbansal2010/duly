module.exports = {
  env: {
    browser: true,
    es2021: true,
  },
  extends: [
    'react-app',
    'react-app/jest',
    'airbnb',
  ],
  rules: {
    'no-unused-expressions': 'off',
    'no-empty': 'warn',
    'no-nested-ternary': 'off',
    'no-unused-vars': 'off',
    'react/jsx-filename-extension': 'off',
    'react/jsx-props-no-spreading': 'warn',
    'react/no-unused-state': 'warn',
    'react/prop-types': 'warn',
    'react/react-in-jsx-scope': 'off',
    'react/state-in-constructor': 'off',
  },
};
