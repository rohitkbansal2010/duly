module.exports = {
  bracketSpacing: true,
  endOfLine: 'lf',
  printWidth: 100,
  singleQuote: true,
  overrides: [
    {
      files: ["**/*.css", "**/*.scss", "**/*.html"],
      options: {
        singleQuote: false
      }
    }
  ],
  jsxSingleQuote: false,
  tabWidth: 2,
  useTabs: false,
};
