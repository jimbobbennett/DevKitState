# DevKitState

## Steps to start

1. Setup development environment by following [Get Started](https://microsoft.github.io/azure-iot-developer-kit/docs/get-started/)
2. Open VS Code
3. Press **F1** or **Ctrl + Shift + P** - `IoT Workbench: Examples` and select DevKitState

## Provision Azure Services

1. Press **F1** or **Ctrl + Shift + P** in Visual Studio Code - **IoT Workbench:Cloud** and click **Azure Provision**
2. Select a subscription.
3. Select or choose a resource group.
4. Select or create an IoT Hub.
5. Wait for the deployment.
6. Select or create an IoT Hub device. Please take a note of the **device name**.
7. Create Function App. Please take a note of the **function app name**.
8. Wait for the deployment to finish.

## Deploy Function App
1. Modify the following line in **devkit-state\run.csx** with the device name you provisioned in previous step:
```
static string deviceName = "";
```
2. ress **F1** or **Ctrl + Shift + P** in Visual Studio Code - **IoT Workbench: Cloud** and click **Azure Deploy**.
3. Wait for function app code uploading.

## Configure IoT Hub Device Connection String in DevKit

1. Connect your DevKit to your machine.
2. Press **F1** or **Ctrl + Shift + P** in Visual Studio Code - **IoT Workbench: Device** and click **config-device-connection**.
3. Hold button A on DevKit, then press rest button, and then release button A to enter config mode.
4. Wait for connection string configuration to complete.

## Upload Arduino Code to DevKit

1. Connect your DevKit to your machine.
2. Press **F1** or **Ctrl + Shift + P** in Visual Studio Code - **IoT Workbench:Device** and click **Device Upload**.
3. Wait for arduino code uploading.

## Monitor DevKit State in Browser

1. Open `web\index.html` in browser
2. Input the function app name you write down
3. Click connect button
4. You should see DevKit state in a few seconds

## Control DevKit User LED

1. Click User LED on the web page
2. You should see user LED state changed in few seconds