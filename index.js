const DEVICE_TYPE = {
  KEYBOARD: "keyboard",
  MOUSE: "mouse",
  GRAPHIC_CARD: "graphiccard",
  MOTHERBOARD: "motherboard"
};

var { execFile } = require("child_process");
var path = require("path");

var rgbNetApp = path.join(
  __dirname,
  "node-rgb-net-win-app",
  "NodeRGBNet",
  "bin",
  "Release",
  "NodeRGBNet.exe"
);

function getRgbDevices(deviceType = null, callback) {
  let args = ["--getDevices", deviceType];
  execute(args, callback);
}

function setColor({ deviceType = null, color = null }, callback) {
  if (!deviceType) {
    callback("'deviceType' is required!", null);
    return;
  } else if (!color) {
    callback("'deviceType' is required!", null);
    return;
  }
  let args = ["--setColor", deviceType, color];
  execute(args, callback);
}

function execute(args, callback) {
  execFile(rgbNetApp, args, { shell: true }, (error, stdout, stderr) => {
    if (error) {
      callback(error, null);
      return;
    }

    if (stderr) {
      callback(stderr, null);
      return;
    }

    var response = JSON.parse(stdout);
    if (response.error) {
      callback(response.error, null);
      return;
    } else {
      callback(null, response.data);
      return;
    }
  });
}

module.exports = {
  DEVICE_TYPE,
  getRgbDevices,
  setColor
};
