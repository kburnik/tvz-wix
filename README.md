![CryptoCurrency Ticker icon](DemoApp/DemoApp.WPF/Assets/AppIcon.png)

--------------------------------------------------------------------------------

# Windows Installer XML Demo Application
## CryptoCurrency Ticker

A simple Crypto Currency Ticker application for Windows. Target market is
[Poloniex](https://www.poloniex.com).

This topic is used as a simple example of a "useful" application for showcasing
MSI installer building using the
[Windows Installer XML (WIX)](http://wixtoolset.org/).

--------------------------------------------------------------------------------

# Windows Installer XML demo aplikacija
## Praćenje cijena kriptovaluta

Jednostavna Windows aplikacija s pregledom stanja burze kriptovaluta. Ciljana
burza je [Poloniex](https://www.poloniex.com).

Ova tema je odabrana kao jednostavan primjer "korisne" aplikacije za koju se
pokazuju principi rada s [Windows Installer XML (WIX)](http://wixtoolset.org/).

--------------------------------------------------------------------------------

![CryptoCurrency Ticker app](docs/DemoApp.png)

--------------------------------------------------------------------------------

## Requirements

These are the requirements I've satisfied to make this project build. You might
be able to achieve the same with newer versions, if you manage, please create
a pull request with the setup you've used.

* [Visual Studio 2015 Community](https://www.microsoft.com/en-us/download/details.aspx?id=48146) - MS does a good job at burying older versions.
* [WIX toolset 3.10.3](http://wixtoolset.org/releases/)
* [Windows 10 SDK](https://developer.microsoft.com/en-us/windows/downloads/windows-10-sdk) - needed for SignTool.exe when
building in release.

### Troubleshooting

* [Missing MSBuild?](https://www.microsoft.com/en-us/download/confirmation.aspx?id=48159)


## Code signing

The app is already being signed in the Release mode builds. For this purpose
I've used the publisher name "GridWaves" and a self-signed certificate generated
on my machine.

In a real production use, you would have to buy a real certificate which
ensures you as a company or organization can be trusted by the user.

### Create a self-signed certificate

Run "Digital Certificate for VBA Projects" from the Microsoft Office Tools.
You should enter the publisher name, this should create a self signed
certificate with the name "Publisher Name" (i.e. whatever you enter).

You can examine the certificate in [Start] > "Manage user certificates".

This is an example of signing a file from git-bash on Windows:

```sh
signtool sign
    -n "Publisher Name" \
    -t http://timestamp.comodoca.com/authenticode \
     DemoApp/DemoApp.WIX/bin/Debug/DemoApp.WIX.msi
```
