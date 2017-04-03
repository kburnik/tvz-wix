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

## Code signing

The app is already being signed in the Release mode builds. For this purpose
I've used the publisher name "GridWaves" and a self-signed certificate generated
on my machine.

In a real production use, you would have to buy a real certificate which
ensures you as a company or organization should be trusted by the user.

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
