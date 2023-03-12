<p align="center">
<img src="https://raw.githubusercontent.com/ShadowGroveGames/Simple-HTTP-and-REST-Server/master/ShadowGroveGames/Simple%20HTTP%20and%20REST%20Server/Resources/simple-http-and-rest-server-banner.png"><h3 align="center"><a href="https://assetstore.unity.com/packages/tools/utilities/simple-http-and-rest-server-244127?utm_source=github">Download from Unity Asset Store</a></h3>
</p>

#### 🏁 How can this asset help me?
Exchanging information between Unity and another program is complicated. This asset provides a simple and fast way to ensure the exchange of information in both directions.

#### Installation
There are 3 ways to install this plugin:
- Import [SimpleHTTPandRESTServer.unitypackage](https://github.com/ShadowGroveGames/Simple-HTTP-and-REST-Server/releases/latest "SimpleHTTPandRESTServer.unitypackage") via Assets-Import Package
- Clone or [download](https://github.com/ShadowGroveGames/Simple-HTTP-and-REST-Server/archive/master.zip "download") this repository and move the Plugins folder to your Unity project's Assets folder
- Add the following line to Packages/manifest.json:
  - `"org.shadow-grove.simple-http-and-rest-server": "https://github.com/ShadowGroveGames/Simple-HTTP-and-REST-Server.git",`
- Via [OpenUPM](https://openupm.com/) after installing [openupm-cli](https://github.com/openupm/openupm-cli#openupm-cli), run the following command:
    - `openupm add org.shadow-grove.simple-http-and-rest-server`
- [Download from Unity Asset Store](https://assetstore.unity.com/packages/tools/utilities/simple-http-and-rest-server-244127?utm_source=github "Download from Unity Asset Store")

━━━━━━━━━━━━━━━━━━━━━━━━━━

💬 [Discord](https://discord.com/invite/hrTXpR3zaA "Discord") · 🌐 [Website](https://shadow-grove.org/ "Website")

━━━━━━━━━━━━━━━━━━━━━━━━━━

#### 🧭 Simple Routing
Implementing routes in Unity has never been easier! With a simple attribute that you write over the function you can specify both the HTTP method and the path.


#### ⭐ Event Based
All route handlers are registered via an event-based system on the server. You only need to drag and drop the route handler into the server.


#### 📦 Resource Loading
Using Resource Loading you can load files from the Resources folder of your game and output them via the HTTP server. See the example "2. Load Files from Resource Folder" in the Examples folder.


#### ⚡ Fast
The speed of the server is overwhelming, which is achieved by using only native implementations. To ensure this, external libraries were deliberately avoided during development.


#### 📚 Multiple Instances
The event based architecture makes it possible to run many different servers over different ports at the same time.


#### 🏠 Listening Addresses
Under Listening Addresses you can not only select the ports to listen on, but also specify whether the server is accessible from the home network or only from the same computer.


#### 🖥️ Multi-platform support
- Windows (Mono/IL2CPP Builds)
- Linux (Mono/IL2CPP Builds)
- Mac OS M1 (Mono/IL2CPP Builds)
- Mac OS Intel (Mono/IL2CPP Builds)
- Android (Not Tested Yet)
- iOS (Not Tested Yet)
