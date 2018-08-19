const rgbDeviceTypes = {
  Keyboard: "keyboard",
  Mouse: "mouse",
  GraphicCard: "graphic"
};
var { execFile } = require("child_process");
var path = require("path");

var rgbNetApp = "./NodeRGBNet.exe";
console.log("aaa");
function getRgbDevices(deviceType = null, callback) {
  let args = ["--getDevices", deviceType];
  console.log(args);
  execute(args, callback);
}

function setColor(color = "#ffffff", callback) {
  let args = ["--setColor", "keyboard", color];
  console.log(args);
  execute(args, callback);
}

function execute(args, callback) {
  execFile(rgbNetApp, args, { shell: true }, (error, stdout, stderr) => {
    if (error) {
      callback(error);
      return;
    }

    if (stderr) {
      callback(stderr, null);
      return;
    }

    var response = JSON.parse(stdout);
    console.log(response);
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
  rgbDeviceTypes,
  getRgbDevices,
  setColor
};
