# MLWD Ltd website

This is the website for my company, yes its rubbish and unmaintained (kinda) but its built in a cool way.

## How

Well the static site is managed via contentful and then updated by a nuget package I have here on github called [micklaw/Dotented.](https://github.com/micklaw/Dotented) It allows you to build out a static site using the contentful GraphQL api and then render the content pages via POCOs and Razor.

If you want to find out how it works RTFM on [micklaw/Dotented.](https://github.com/micklaw/Dotented) and simply look at this repo to see how to configure it for actual use, the other stuff I'll details below, but it comprises off:

- Webhook is fired on contentful
- Action is kicked off here see .github/workflows/example.yml
- If the webhook event matched this then deploy
- Static site generation code is ran in action
- Uploaded to gh-pages branch

![Contentful webhook config](/contentful-webhook.png)

Its actually pretty easy to configure, simply digging about through here should do it. My example.yml has some of the old NPM stuff from when I don't this about 4 years ago, but I'm sure there is a better way now, I just couldnt be bothered changing it.