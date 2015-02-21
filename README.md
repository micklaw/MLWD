# Michael Law Company Websie

[![Build status](https://ci.appveyor.com/api/projects/status/2v10wdobwlhvw5wn?svg=true)](https://ci.appveyor.com/project/MichaelLaw/mlwd)

The source for the website of my company. Feel free to have a poke around and fix anything that i have goosed.

The site is is built using Umbraco and also makes use of the package Yomego.Cms, which provides a code first approach to Umbraco in regards
to document types. This is completed via a custom build of uSiteBuilder and synced via a section in the CMS. All other aspects of the CMS 
are managed via a custom build of uSync, which again is run via the same custom section in the CMS.

This allows us to keep all aspects of the CMS in source and with our custom route handler, allows to keep to an exact, true representation
of ASp.Net MVC.

