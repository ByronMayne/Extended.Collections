// @ts-check
const lightCodeTheme = require('prism-react-renderer/themes/github');
const darkCodeTheme = require('prism-react-renderer/themes/oceanicNext');


/** @type {import('@docusaurus/types').Config} */
const config = {
  title: 'Extended.Collections',
  tagline: 'The missing C# collections you did not know you needed',
  favicon: 'img/favicon.ico',
  url: 'https://github.com',
  baseUrl: '/Extended.Collections/',
  organizationName: 'ByronMayne', 
  projectName: 'Extended.Collections', // Usually your repo name.
  trailingSlash: false,
  onBrokenLinks: 'throw',
  onBrokenMarkdownLinks: 'warn',
  themeConfig: {
     prism: {
      theme: lightCodeTheme,
      darkTheme: darkCodeTheme,
      additionalLanguages: [ 'csharp' ]
    },
  },
  plugins: [
    ['@docusaurus/plugin-debug', {}],
    ['@docusaurus/theme-classic', {}],
    ['@docusaurus/plugin-sitemap', {}],
    ['@docusaurus/plugin-content-docs', {
      path: '../documentation',
      breadcrumbs: true, 
      routeBasePath: '/',
      showLastUpdateTime: true,
      editUrl: ({docPath}) =>
      `https://github.com/ByronMayne/Extended.Collections/edit/main/documentation/${docPath}`,

    }]
  ]
};

module.exports = config;
