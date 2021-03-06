# ATEM-WebTally
Turn your phone into a Tally-client, working together with ATEM Live Production Switchers.

## Requirements
1. ATEM Switchers Software. Tested with version 6.9, should work with earlier versions
1. .NET Framework version 4.5

The mentioned versions might not necessarily be used, but are recommended.

## Download
Download the application via this link:
[download](https://github.com/scebbers/ATEM-WebTally/raw/master/App/ATEM%20WebTally.zip)

## Usage
### SDK
This project makes use of the BMDSwitcherAPI.dll library, which comes together with ATEM Switchers Software. When editing the code, make sure you include a reference to this library.
### Application
This application is open-source and free to use.
1. Start the application and connect to your ATEM Switcher.
1. Start the webserver
1. Connect your client-handheld devices to the same network via WiFi.
1. On your client, go to the server ip-address.
1. Choose your input, and see if you are live.

## Bugs and issues
Please report any bugs and issues you encounter. When reporting, please include the contents of the log window, your OS version and setup of your network.

## Credits
The following libraries are used in this application
- QRCoder library. See https://github.com/codebude/QRCoder.
- Blackmagic ATEM Switchers SDK 6.9. See https://www.blackmagicdesign.com/support.
