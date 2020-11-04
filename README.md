# MLWD Ltd website

This is the website for my company, yes its rubbish and unmaintained (kinda) but its built in a kinda cool way.

## How?

Well the static site is managed via contentful and then updated by a nuget package I have here called micklaw/Dotented. It allows you to build out a static site using the contentful GraphQL api and then render the content pages via POCOs and Razor.

If you wan to find out how it works RTFM on Dotented and simply look at this repo, the other stuff I'll details below, but it comprises off:

- Webhook setup in github for this project
- Webhook is consumed via contentful
- A github action fires on hit of the webhook
- Static site generation code is ran in action
- Uploaded to gh-pages branch

Knickers