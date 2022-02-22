## LiveLines

This projects houses the codebase for LiveLines, a web app that lets you post a line a day. It has various features such as streaks, being able to post the day before, oauth integration with GitHub and Spotify, and more.

// TODO: add an image here

It's hosted at https://livelin.es, on Azure. Build in .NET 6, with a React frontend and Postgres for the database.

# Building the project

When first building the project, you'll want to install Rider, the latest version of .NET 6 (LTS), and Node (for the web build, see README in `LiveLines/ClientApp`)

Then the environment variables need to be set up, which for the developer build live in `LiveLines/Properties/launchSettings.json`.

Starting with the `HOST_NAME`, you can leave this as is unless hosting.

For the database env vars, you will need a postgres database that has all of the sql in `/db.sql` (in the root directory).

To get the GitHub client id and secret you'll need to create an oauth application in GitHub (https://github.com/settings/applications).

For localhost you'll need a `Homepage URL` of `https://localhost:44492/` and `Authorization callback URL` of `https://localhost:44492/`.

For production you'll need a `Homepage URL` of `https://livelin.es/` and `Authorization callback URL` of `http://livelin.es/`. This is HTTP because of the TLS redirect I have in production.

Similarly, for the Spotify client id and secret you'll need to create an oauth application in Spotify (https://developer.spotify.com/dashboard/applications).

For localhost, the Spotify app needs a `RedirectUri` set to `https://localhost:44492/api/spotify/callback`, for production `https://livelin.es/api/spotify/callback`. If you have problems with production it might need to be `http` depending on your TLS redirect configuration.

Once the web build is initialised and all of the environment variables have been set, the build in rider should run the backend, and then create the proxy for the frontend. Tada.
