var {getRgbDevices, setColor, DEVICE_TYPE} = require("./index.js")

getRgbDevices(DEVICE_TYPE.KEYBOARD, (err, data) => {
  console.log(data)
})

setColor({deviceType: DEVICE_TYPE.KEYBOARD, color: '#0000FF'}, (err, data) => {
  console.log(data)
})
