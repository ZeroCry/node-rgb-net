var {getRgbDevices, setColor, DEVICE_TYPE} = require("./index.js")

getRgbDevices(DEVICE_TYPE.KEYBOARD, (err, data) => {
  console.log(data)
})

setColor({deviceType: DEVICE_TYPE.KEYBOARD, color: '#0000ff'}, (err, data) => {
  console.log(data)
})
